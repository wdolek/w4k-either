using System;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration;

[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    private static readonly string EitherAttributeFullyQualifiedName = typeof(EitherAttribute).FullName!;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<EitherStructGenerationContext> structsToGenerate = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                EitherAttributeFullyQualifiedName,
                static (node, ct) => IsStructDeclarationSyntax(node, ct),
                static (ctx, ct) => GetTypeToGenerate(ctx, ct))
            .Where(c => c is not null)!;
        
        context.RegisterSourceOutput(
            structsToGenerate,
            static (ctx, structToGenerate) => Execute(ctx, structToGenerate));
    }

    private static bool IsStructDeclarationSyntax(SyntaxNode node, CancellationToken cancellationToken) =>
        node is StructDeclarationSyntax;

    private static EitherStructGenerationContext? GetTypeToGenerate(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        // type not available in current compilation
        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var targetName = typeSymbol.Name;
        var targetNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : typeSymbol.ContainingNamespace.ToDisplayString();

        // must be `partial struct`
        if (!IsPartial(typeSymbol, cancellationToken))
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.TypeMustBePartial,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // get type parameters
        var attrTypeParams = GetAttributeTypeParameters(context.Attributes[0], cancellationToken);
        var (targetTypeParams, diagnostic) = GetTargetTypeParameters(typeSymbol, cancellationToken);

        if (diagnostic is not null)
        {
            return EitherStructGenerationContext.Invalid(targetNamespace, targetName, diagnostic);
        }

        // user has not specified any type parameter
        if (attrTypeParams.Length == 0 && targetTypeParams.Length == 0)
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NoTypeParameter,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // user specified types both using attribute and making type generic
        if (attrTypeParams.Length > 0 && targetTypeParams.Length > 0)
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        return targetTypeParams.Length > 0
            ? EitherStructGenerationContext.Generic(targetNamespace, targetName, targetTypeParams)
            : EitherStructGenerationContext.NonGeneric(targetNamespace, targetName, attrTypeParams);
    }

    private static void Execute(SourceProductionContext context, EitherStructGenerationContext structToGenerate)
    {
        var diagnostics = structToGenerate.Diagnostics;
        if (diagnostics.Count > 0)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            return;
        }

        var sb = new StringBuilder(4096);
        
        var writer = new EitherStructWriter(structToGenerate, sb);
        writer.Write();
        
        context.AddSource($"{structToGenerate.TargetTypeName}.generated.cs", sb.ToString());
    }

    private static bool IsPartial(INamedTypeSymbol namedTypeSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var syntaxRef in namedTypeSymbol.DeclaringSyntaxReferences)
        {
            var syntaxNode = syntaxRef.GetSyntax();
            if (syntaxNode is StructDeclarationSyntax structDeclaration)
            {
                if (structDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    return true;
                }
            }
        }

        return false;
    }    
 
    private static string[] GetAttributeTypeParameters(AttributeData attribute, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var ctor = attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return Array.Empty<string>();
        }

        var ctorParameters = ctor.Parameters;
        var @params = new string[ctorParameters.Length];

        for (var i = 0; i < @params.Length; i++)
        {
            var p = ctorParameters[i];
            @params[i] = p.Type.ToDisplayString();
        }

        return @params;
    }

    private static (string[], Diagnostic?) GetTargetTypeParameters(
        INamedTypeSymbol typeSymbol,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (typeSymbol.Arity == 0)
        {
            return (Array.Empty<string>(), null);
        }

        if (typeSymbol.Arity < 2)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TooFewTypeParameters,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return (Array.Empty<string>(), diagnostic);
        }

        var typeParameters = typeSymbol.TypeParameters;
        var @params = new string[typeParameters.Length];

        for (var i = 0; i < @params.Length; i++)
        {
            var p = typeParameters[i];
            @params[i] = p.Name;

            // check if the type parameter has a notnull constraint
            if (!p.HasNotNullConstraint)
            {
                var diagnostic = Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NotNullConstraintMissing,
                    location: p.Locations[0],
                    messageArgs: p.Name);

                return (Array.Empty<string>(), diagnostic);
            }
        }

        return (@params, null);
    }
}

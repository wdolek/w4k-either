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
                static (node, _) => IsStructDeclarationSyntax(node),
                static (ctx, ct) => GetTypeToGenerate(ctx, ct))
            .Where(c => c is not null)!;
        
        context.RegisterSourceOutput(
            structsToGenerate,
            static (ctx, structToGenerate) => Execute(ctx, structToGenerate));
    }

    private static bool IsStructDeclarationSyntax(SyntaxNode node) =>
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
        var attrTypeParams = GetAttributeTypeParameters(context.Attributes[0], context.SemanticModel, cancellationToken);
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
        EitherStructWriter.Write(structToGenerate, sb);
        
        context.AddSource(CreateGeneratedFileName(structToGenerate), sb.ToString());
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

    private static EitherStructGenerationContext.TypeParameter[] GetAttributeTypeParameters(
        AttributeData attribute,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ctor = attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return Array.Empty<EitherStructGenerationContext.TypeParameter>();
        }

        var displayFormat = new SymbolDisplayFormat(
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var ctorArguments = attribute.ConstructorArguments;
        var @params = new EitherStructGenerationContext.TypeParameter[ctorArguments.Length];

        for (var i = 0; i < @params.Length; i++)
        {
            var arg = ctorArguments[i];
            if (arg.Value is INamedTypeSymbol typeSymbol)
            {
                @params[i] = new EitherStructGenerationContext.TypeParameter(
                    index: i + 1,
                    name: typeSymbol.ToMinimalDisplayString(semanticModel, 0, displayFormat),
                    isValueType: typeSymbol.IsValueType,
                    isNullable: false);
            }
        }

        return @params;
    }

    private static (EitherStructGenerationContext.TypeParameter[] TypeParameters, Diagnostic? Diagnostic) GetTargetTypeParameters(
        INamedTypeSymbol typeSymbol,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (typeSymbol.Arity == 0)
        {
            return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), null);
        }

        if (typeSymbol.Arity < 2)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TooFewTypeParameters,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), diagnostic);
        }

        var typeParameters = typeSymbol.TypeParameters;
        var @params = new EitherStructGenerationContext.TypeParameter[typeParameters.Length];

        for (var i = 0; i < @params.Length; i++)
        {
            var p = typeParameters[i];
            @params[i] = new EitherStructGenerationContext.TypeParameter(
                index: i + 1,
                name: p.Name,
                isValueType: p.HasValueTypeConstraint || p.IsValueType,
                isNullable: !p.HasNotNullConstraint);
        }

        return (@params, null);
    }

    private static string CreateGeneratedFileName(EitherStructGenerationContext structToGenerate) =>
        structToGenerate.IsGenericType
            ? $"{structToGenerate.TargetTypeName}`{structToGenerate.TypeParameters.Count}.g.cs"
            : $"{structToGenerate.TargetTypeName}.g.cs";
}

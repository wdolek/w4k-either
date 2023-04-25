using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using W4k.Either.CodeGeneration.Context;
using W4k.Either.CodeGeneration.Processors;

namespace W4k.Either.CodeGeneration;

[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    private const string EitherAttributeFullyQualifiedName = "W4k.Either.Abstractions.EitherAttribute";

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
        
        var processingContext = new ProcessorContext(context.Attributes[0], context.SemanticModel, typeSymbol);

        // [Either(typeof(string), typeof(int))] partial struct E { }
        var attrTypeParamsResult = AttributeTypeParamsProcessor.GetAttributeTypeParameters(processingContext, cancellationToken);
        if (!attrTypeParamsResult.IsSuccess)
        {
            return EitherStructGenerationContext.Invalid(targetNamespace, targetName, attrTypeParamsResult.Diagnostic!);
        }

        // [Either] partial struct E<T1, T2> { }
        var targetTypeParamsResult = GenericTypeParamsProcessor.GetTargetTypeParameters(processingContext, cancellationToken);
        if (!targetTypeParamsResult.IsSuccess)
        {
            return EitherStructGenerationContext.Invalid(targetNamespace, targetName, targetTypeParamsResult.Diagnostic!);
        }

        var attrTypeParams = attrTypeParamsResult.TypeParameters;
        var genericTypeParams = targetTypeParamsResult.TypeParameters;

        // user has not specified any type parameter
        if (attrTypeParams.Length == 0 && genericTypeParams.Length == 0)
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
        if (attrTypeParams.Length > 0 && genericTypeParams.Length > 0)
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        return genericTypeParams.Length > 0
            ? EitherStructGenerationContext.Generic(targetNamespace, targetName, genericTypeParams)
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

        context.AddSource(
            structToGenerate.FileName,
            SourceText.From(sb.ToString(), Encoding.UTF8));
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
}

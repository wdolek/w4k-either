using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using W4k.Either.CodeGeneration.Context;
using W4k.Either.CodeGeneration.Processors;

namespace W4k.Either.CodeGeneration;

[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    private const string EitherAttributeFullyQualifiedName = "W4k.Either.EitherAttribute";

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

        // containing type must be `partial`
        var (parentTypeDeclaration, parentDiagnostic) = GeneratorHelpers.GetContainingTypeDeclaration(typeSymbol, cancellationToken);
        if (parentDiagnostic is not null)
        {
            return EitherStructGenerationContext.Invalid(parentDiagnostic);
        }

        // target type must be `partial struct`
        if (!GeneratorHelpers.IsPartial(typeSymbol, cancellationToken))
        {
            return EitherStructGenerationContext.Invalid(
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
            return EitherStructGenerationContext.Invalid(attrTypeParamsResult.Diagnostic!);
        }

        // [Either] partial struct E<T1, T2> { }
        var targetTypeParamsResult = GenericTypeParamsProcessor.GetTargetTypeParameters(processingContext, cancellationToken);
        if (!targetTypeParamsResult.IsSuccess)
        {
            return EitherStructGenerationContext.Invalid(targetTypeParamsResult.Diagnostic!);
        }

        var attrTypeParams = attrTypeParamsResult.TypeParameters;
        var genericTypeParams = targetTypeParamsResult.TypeParameters;

        // user has not specified any type parameter
        if (attrTypeParams.Length == 0 && genericTypeParams.Length == 0)
        {
            return EitherStructGenerationContext.Invalid(
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NoTypeParameter,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // user specified types both using attribute and making type generic
        if (attrTypeParams.Length > 0 && genericTypeParams.Length > 0)
        {
            return EitherStructGenerationContext.Invalid(
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        var isGeneric = genericTypeParams.Length > 0;
        var typeParams = isGeneric ? genericTypeParams : attrTypeParams;

        // find already declared constructors by user
        // (generating same constructors will be skipped later)
        var declaredConstructors = CtorProcessor.CollectDeclaredConstructors(typeSymbol, typeParams, cancellationToken);

        return isGeneric
            ? EitherStructGenerationContext.Generic(targetNamespace, parentTypeDeclaration, targetName, typeParams, declaredConstructors)
            : EitherStructGenerationContext.NonGeneric(targetNamespace, parentTypeDeclaration, targetName, typeParams, declaredConstructors);
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
}

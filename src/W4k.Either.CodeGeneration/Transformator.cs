using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.TypeDeclaration;
using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration;

internal sealed class Transformator
{
    public static TransformationResult? Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        // type not available in current compilation
        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        // analyze marked type itself
        // e.g. `partial struct Either<TLeft, TRight> { }`
        var typeDeclarationResult = DeclarationAnalyzer.Analyze(typeSymbol, cancellationToken);
        if (!typeDeclarationResult.IsValid)
        {
            return TransformationResult.Invalid(typeDeclarationResult.Diagnostic);
        }

        // optionally analyze containing type (if any, when marked type is nested)
        // e.g. `partial class ContainingType { private partial struct Either<TLeft, TRight> { } }`
        var containingTypeDeclarationResult = DeclarationAnalyzer.Analyze(typeSymbol.ContainingType, cancellationToken);
        if (!containingTypeDeclarationResult.IsValid)
        {
            return TransformationResult.Invalid(containingTypeDeclarationResult.Diagnostic);
        }

        // analyze and retrieve type parametrization
        var paramAnalysisContext = new ParamAnalysisContext(
            FindAttribute(context.Attributes, cancellationToken),
            context.SemanticModel,
            typeSymbol);

        var paramAnalysisResult = ParamAnalyzer.Analyze(paramAnalysisContext, cancellationToken);
        if (!paramAnalysisResult.IsSuccess)
        {
            return TransformationResult.Invalid(paramAnalysisResult.Diagnostic);
        }

        var typeDeclaration = typeDeclarationResult.TypeDeclaration!;
        var containingTypeDeclaration = containingTypeDeclarationResult.TypeDeclaration;

        // generator matches only `struct` and `class` declarations - we don't need to check `TypeKind`
        return TransformationResult.Valid(
            typeSymbol.TypeKind,
            typeDeclaration,
            containingTypeDeclaration,
            paramAnalysisResult.ParametrizationKind,
            paramAnalysisResult.TypeParameters);
    }

    private static AttributeData FindAttribute(ImmutableArray<AttributeData> attributes, CancellationToken cancellationToken)
    {
        AttributeData? foundAttribute = null;
        if (attributes.Length == 1)
        {
            foundAttribute = attributes[0];
            goto ReturnAttribute;
        }

        cancellationToken.ThrowIfCancellationRequested();
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeClass?.ToDisplayString() == Constants.EitherAttributeFullyQualifiedName)
            {
                foundAttribute = attribute;
                break;
            }
        }

        // Should Never Happen™ as generator already matched type with attribute
        if (foundAttribute is null)
        {
            throw new InvalidOperationException($"Cannot find attribute of type `{Constants.EitherAttributeFullyQualifiedName}`.");
        }

        ReturnAttribute:
        return foundAttribute;
    }
}

using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.TypeDeclaration;
using W4k.Either.TypeParametrization;

namespace W4k.Either;

internal sealed class Transformator
{
    public static TransformationResult? Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        // type not available in current compilation
        if (context.SemanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        // type not marked with `EitherAttribute`
        var eitherAttributeResult = AttributeFinder.FindAttribute(typeSymbol, cancellationToken);
        if (!eitherAttributeResult.IsFound)
        {
            if (eitherAttributeResult.Diagnostic is not null)
            {
                return TransformationResult.Invalid(eitherAttributeResult.Diagnostic);
            }

            // type not marked -> nothing to do here
            return null;
        }

        var eitherAttribute = eitherAttributeResult.AttributeData;

        // analyze marked type itself
        // e.g. `partial struct Either<TLeft, TRight> { }`
        var typeDeclarationResult = DeclarationAnalyzer.Analyze(typeSymbol, cancellationToken);
        if (!typeDeclarationResult.IsValid)
        {
            return TransformationResult.Invalid(typeDeclarationResult.Diagnostic);
        }

        // analyze containing type (if any, when marked type is nested)
        // e.g. `partial class ContainingType { private partial struct Either<TLeft, TRight> { } }`
        var containingTypeDeclarationResult = DeclarationAnalyzer.Analyze(typeSymbol.ContainingType, cancellationToken);
        if (!containingTypeDeclarationResult.IsValid)
        {
            return TransformationResult.Invalid(containingTypeDeclarationResult.Diagnostic);
        }

        // analyze and retrieve type parametrization
        var paramAnalysisContext = new ParamAnalysisContext(eitherAttribute, context.SemanticModel, typeSymbol);

        var paramAnalysisResult = ParamAnalyzer.Analyze(paramAnalysisContext, cancellationToken);
        if (!paramAnalysisResult.IsSuccess)
        {
            return TransformationResult.Invalid(paramAnalysisResult.Diagnostic);
        }

        var typeDeclaration = typeDeclarationResult.TypeDeclaration!;
        var containingTypeDeclaration = containingTypeDeclarationResult.TypeDeclaration;

        // generator matches only `struct` and `class` declarations - we don't need to check `TypeKind` further
        return TransformationResult.Valid(
            typeSymbol.TypeKind,
            typeDeclaration,
            containingTypeDeclaration,
            eitherAttribute,
            paramAnalysisResult.ParametrizationKind,
            paramAnalysisResult.TypeParameters);
    }
}
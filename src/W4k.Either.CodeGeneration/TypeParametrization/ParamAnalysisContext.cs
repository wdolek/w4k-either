using Microsoft.CodeAnalysis;

namespace W4k.Either.TypeParametrization;

internal readonly struct ParamAnalysisContext
{
    public ParamAnalysisContext(
        AttributeData attribute,
        SemanticModel semanticModel,
        INamedTypeSymbol typeSymbol)
    {
        var location = attribute
            .ApplicationSyntaxReference
            !.SyntaxTree
            .GetLocation(attribute.ApplicationSyntaxReference.Span);

        var isNullRefTypeScopeEnabled =
            semanticModel
                .GetNullableContext(location.SourceSpan.Start)
                .AnnotationsEnabled();

        Attribute = attribute;
        AttributeLocation = location;
        TypeSymbol = typeSymbol;
        IsNullRefTypeScopeEnabled = isNullRefTypeScopeEnabled;
    }

    public AttributeData Attribute { get; }
    public Location AttributeLocation { get; }
    public INamedTypeSymbol TypeSymbol { get; }
    public bool IsNullRefTypeScopeEnabled { get; }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Either.TypeParametrization;

internal static class AttributeFinder
{
    private const string EitherAttributeFullyQualifiedName = "Either.EitherAttribute";

    public static AttributeFindResult FindAttribute(INamedTypeSymbol typeSymbol, CancellationToken cancellationToken)
    {
        AttributeData? foundAttribute = null;

        cancellationToken.ThrowIfCancellationRequested();
        foreach (var attribute in typeSymbol.GetAttributes())
        {
            var attrTypeName = attribute.AttributeClass?.ToDisplayString();
            if (attrTypeName is null)
            {
                continue;
            }

            if (attrTypeName.StartsWith(EitherAttributeFullyQualifiedName, StringComparison.Ordinal))
            {
                // we already found marking attribute
                // (it is technically possible to decorate type with `[Either]` and `[Either<Foo, Bar>]` attributes, we need to prevent that)
                if (foundAttribute is not null)
                {
                    return AttributeFindResult.Ambiguous(typeSymbol);
                }

                foundAttribute = attribute;
            }
        }

        return foundAttribute is null
            ? AttributeFindResult.NotFound()
            : AttributeFindResult.Found(foundAttribute);
    }
}

internal readonly struct AttributeFindResult
{
    private AttributeFindResult(AttributeData? attributeData, Diagnostic? diagnostic)
    {
        IsFound = attributeData is not null;
        AttributeData = attributeData;
        Diagnostic = diagnostic;
    }

    [MemberNotNullWhen(true, nameof(AttributeData))]
    public bool IsFound { get; }
    public AttributeData? AttributeData { get; }
    public Diagnostic? Diagnostic { get; }

    public static AttributeFindResult Found(AttributeData attributeData) =>
        new(attributeData, null);

    public static AttributeFindResult Ambiguous(INamedTypeSymbol typeSymbol)
    {
        var diagnostic = Diagnostic.Create(
            descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
            location: typeSymbol.Locations[0],
            messageArgs: typeSymbol.Name);

        return new AttributeFindResult(null, diagnostic);
    }

    public static AttributeFindResult NotFound() =>
        new(null, null);
}
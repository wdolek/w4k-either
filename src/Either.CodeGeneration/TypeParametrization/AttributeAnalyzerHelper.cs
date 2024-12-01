using System;
using Microsoft.CodeAnalysis;

namespace Either.TypeParametrization;

internal static class AttributeAnalyzerHelper
{
    public static readonly SymbolDisplayFormat DisplayFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public static (bool IsTypeUsed, Diagnostic? diagnostic) IsTypeUsed(
        Span<TypeParameter> collectedParams,
        INamedTypeSymbol typeSymbol,
        Location location,
        string typeParamName)
    {
        foreach (var typeParameter in collectedParams)
        {
            if (typeParameter.Name != typeParamName)
            {
                continue;
            }

            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeMustBeUnique,
                location: location,
                messageArgs: typeSymbol.Name);

            return (true, diagnostic);
        }

        return (false, null);
    }

    public static bool IsTypeNullable(INamedTypeSymbol typeSymbol, bool isNullRefTypesScopeEnabled)
    {
        // #nullable enable -> reference type is not nullable
        // #nullable disable -> reference type is nullable
        if (typeSymbol.IsReferenceType)
        {
            return !isNullRefTypesScopeEnabled;
        }

        // check whether value type is nullable using `Nullable<T>`, e.g. `int?`
        if (typeSymbol.IsValueType)
        {
            return typeSymbol.IsGenericType && typeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T;
        }

        return false;
    }
}
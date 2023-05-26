using System;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.TypeParametrization;

internal static class AttributeAnalyzer
{
    private static readonly SymbolDisplayFormat DisplayFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public static ParamAnalysisResult Analyze(ParamAnalysisContext context, CancellationToken cancellationToken)
    {
        var attribute = context.Attribute;
        var ctor = attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return ParamAnalysisResult.Empty(ParametrizationKind.Attribute);
        }

        var attrCtorArguments = attribute.ConstructorArguments;

        var typeParams = new TypeParameter[attrCtorArguments.Length];
        var typeParamsSpan = typeParams.AsSpan();

        for (var i = 0; i < typeParams.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var arg = attrCtorArguments[i];
            if (arg.Value is not INamedTypeSymbol typeSymbol)
            {
                continue;
            }

            var typeParamName = typeSymbol.ToDisplayString(DisplayFormat);

            // use of `[Either(typeof(MyGenericType<>))]` is forbidden, type using open generics couldn't be generated
            if (typeSymbol.IsUnboundGenericType)
            {
                var unboundGenericTypeDiagnostic = Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.TypeParameterMustBeBound,
                    location: context.AttributeLocation,
                    messageArgs: typeSymbol.Name);

                return ParamAnalysisResult.Failure(ParametrizationKind.Attribute, unboundGenericTypeDiagnostic);
            }

            // check that types are unique, `[Either(typeof(int), typeof(int))]` is forbidden,
            // it wouldn't be possible to determine which field to use based on its type
            var (isTypeUsed, typeUsedDiagnostic) = IsTypeUsed(typeParamsSpan.Slice(0, i), typeSymbol, context.AttributeLocation, typeParamName);
            if (isTypeUsed)
            {
                return ParamAnalysisResult.Failure(ParametrizationKind.Attribute, typeUsedDiagnostic!);
            }

            typeParams[i] = new TypeParameter(
                typeSymbol,
                index: i + 1,
                name: typeParamName,
                isReferenceType: typeSymbol.IsReferenceType,
                isValueType: typeSymbol.IsValueType,
                isNullable: IsTypeNullable(typeSymbol, context.IsNullRefTypeScopeEnabled));
        }

        return ParamAnalysisResult.Success(ParametrizationKind.Attribute, typeParams);
    }

    private static (bool IsTypeUsed, Diagnostic? diagnostic) IsTypeUsed(
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
    
    private static bool IsTypeNullable(INamedTypeSymbol typeSymbol, bool isNullRefTypesScopeEnabled)
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

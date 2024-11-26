using System.Threading;
using Microsoft.CodeAnalysis;

namespace W4k.Either.TypeParametrization;

internal static class GenericAnalyzer
{
    public static ParamAnalysisResult Analyze(ParamAnalysisContext context, CancellationToken cancellationToken)
    {
        var typeSymbol = context.TypeSymbol;
        if (typeSymbol.Arity == 0)
        {
            return ParamAnalysisResult.Empty(ParametrizationKind.Generic);
        }

        var typeParams = CollectGenericTypeParameters(typeSymbol, cancellationToken);

        return ParamAnalysisResult.Success(ParametrizationKind.Generic, typeParams);
    }

    private static TypeParameter[] CollectGenericTypeParameters(INamedTypeSymbol typeSymbol, CancellationToken cancellationToken)
    {
        var typeParams = new TypeParameter[typeSymbol.TypeParameters.Length];
        for (var i = 0; i < typeParams.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typeParam = typeSymbol.TypeParameters[i];
            var typeParamName = typeParam.Name;

            // NB! it's not possible to combine `notnull` and `class`/`struct` constraints
            var isReferenceType = typeParam.HasReferenceTypeConstraint || typeParam.IsReferenceType;
            var isValueType = typeParam.HasValueTypeConstraint || typeParam.IsValueType;
            var isNullable = IsTypeNullable(typeParam, isValueType);

            typeParams[i] = new TypeParameter(
                typeParam,
                index: i + 1,
                name: typeParamName,
                isReferenceType: isReferenceType,
                isValueType: isValueType,
                isNullable: isNullable);
        }

        return typeParams;
    }

    private static bool IsTypeNullable(ITypeParameterSymbol typeParam, bool isValueType)
    {
        // `notnull` constraint is present
        if (typeParam.HasNotNullConstraint)
        {
            return false;
        }

        // it's not possible to declare nullable value type when using generics
        // NB! type parameter may be unconstrained - not `class` nor `struct` - in such case both flags are `false` and we will decide
        //     based on nullable reference type scope in next step
        if (isValueType)
        {
            return false;
        }

        // `class?` constraint or no constraint at all
        // (when no constraint is present, user is on its own) 
        return typeParam.ReferenceTypeConstraintNullableAnnotation is NullableAnnotation.None or NullableAnnotation.Annotated;
    }
}
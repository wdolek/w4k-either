using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration;

internal static class GenericTypeParamsProcessor
{
    public static (TypeParameter[] TypeParameters, Diagnostic? Diagnostic) GetTargetTypeParameters(
        INamedTypeSymbol typeSymbol,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (typeSymbol.Arity == 0)
        {
            return (Array.Empty<TypeParameter>(), null);
        }

        if (typeSymbol.Arity < 2)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TooFewTypeParameters,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return (Array.Empty<TypeParameter>(), diagnostic);
        }

        var typeParams = new TypeParameter[typeSymbol.TypeParameters.Length];

        for (var i = 0; i < typeParams.Length; i++)
        {
            var typeParam = typeSymbol.TypeParameters[i];
            var typeParamName = typeParam.Name;

            // NB! it's not possible to combine `notnull` and `class`/`struct` constraints
            var isReferenceType = typeParam.HasReferenceTypeConstraint || typeParam.IsReferenceType;
            var isValueType = typeParam.HasValueTypeConstraint || typeParam.IsValueType;
            var isNullable = IsGenericTypeParameterNullable(typeParam, isReferenceType);

            typeParams[i] = new TypeParameter(
                index: i + 1,
                name: typeParamName,
                isReferenceType: isReferenceType,
                isValueType: isValueType,
                isNullable: isNullable);
        }

        return (typeParams, null);
    }
    
    private static bool IsGenericTypeParameterNullable(ITypeParameterSymbol typeParam, bool isReferenceType)
    {
        // `notnull` constraint is present
        if (typeParam.HasNotNullConstraint)
        {
            return false;
        }

        // it's not possible to declare nullable value type when using generics
        if (!isReferenceType)
        {
            return false;
        }

        // special case of nullable reference type constraint: `class?`
        return typeParam.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated;
    }
}

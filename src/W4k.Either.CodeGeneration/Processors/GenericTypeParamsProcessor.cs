using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration.Processors;

internal static class GenericTypeParamsProcessor
{
    public static ProcessorResult GetTargetTypeParameters(ProcessorContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var typeSymbol = context.TypeSymbol;
        if (typeSymbol.Arity == 0)
        {
            return ProcessorResult.Empty();
        }

        if (typeSymbol.Arity < 2)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TooFewTypeParameters,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return ProcessorResult.Failure(diagnostic);
        }

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
                index: i + 1,
                name: typeParamName,
                isReferenceType: isReferenceType,
                isValueType: isValueType,
                isNullable: isNullable);
        }

        return ProcessorResult.Success(typeParams);
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
        return typeParam.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.None
            || typeParam.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated;
    }
}

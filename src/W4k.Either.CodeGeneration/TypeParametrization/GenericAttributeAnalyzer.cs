using System;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.TypeParametrization;

internal static class GenericAttributeAnalyzer
{
    public static ParamAnalysisResult Analyze(ParamAnalysisContext context, CancellationToken cancellationToken)
    {
        var attribute = context.Attribute;
        var attributeClass = attribute.AttributeClass;
        if (attributeClass is null || attributeClass.TypeArguments.Length == 0)
        {
            return ParamAnalysisResult.Empty(ParametrizationKind.GenericAttribute);
        }
        
        var attrTypeParams = attributeClass.TypeArguments;

        var typeParams = new TypeParameter[attrTypeParams.Length];
        var typeParamsSpan = typeParams.AsSpan();

        for (var i = 0; i < typeParams.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typeSymbol = attrTypeParams[i] as INamedTypeSymbol;
            if (typeSymbol == null)
            {
                continue;
            }

            var typeParamName = typeSymbol.ToDisplayString(AttributeAnalyzerHelper.DisplayFormat);

            // use of `[Either<MyGenericType<>>]` is forbidden, type using open generics couldn't be generated
            if (typeSymbol.IsUnboundGenericType)
            {
                var unboundGenericTypeDiagnostic = Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.TypeParameterMustBeBound,
                    location: context.AttributeLocation,
                    messageArgs: typeSymbol.Name);

                return ParamAnalysisResult.Failure(ParametrizationKind.Attribute, unboundGenericTypeDiagnostic);
            }

            // check that types are unique, `[Either<int, int>]` is forbidden,
            // it wouldn't be possible to determine which field to use based on its type
            var (isTypeUsed, typeUsedDiagnostic) = AttributeAnalyzerHelper.IsTypeUsed(
                typeParamsSpan.Slice(0, i),
                typeSymbol,
                context.AttributeLocation,
                typeParamName);

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
                isNullable: AttributeAnalyzerHelper.IsTypeNullable(typeSymbol, context.IsNullRefTypeScopeEnabled));
        }

        return ParamAnalysisResult.Success(ParametrizationKind.Attribute, typeParams);
    }
}

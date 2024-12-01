using System;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Either.TypeParametrization;

internal static class AttributeAnalyzer
{
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

            var typeParamName = typeSymbol.ToDisplayString(AttributeAnalyzerHelper.DisplayFormat);

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
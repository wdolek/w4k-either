using System.Threading;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.TypeParametrization;

internal static class ParamAnalyzer
{
    public static ParamAnalysisResult Analyze(ParamAnalysisContext context, CancellationToken cancellationToken)
    {
        // [Either] partial struct E<T1, T2> { }
        var genericAnalysisResult = GenericAnalyzer.Analyze(context, cancellationToken);
        if (!genericAnalysisResult.IsSuccess)
        {
            return genericAnalysisResult;
        }

        // [Either(typeof(string), typeof(int))] partial struct E { }
        var attributeAnalysisResult = AttributeAnalyzer.Analyze(context, cancellationToken);
        if (!attributeAnalysisResult.IsSuccess)
        {
            return attributeAnalysisResult;
        }

        var genericTypeParams = genericAnalysisResult.TypeParameters;
        var attrTypeParams = attributeAnalysisResult.TypeParameters;
        
        // user has not specified any type parameter
        if (genericTypeParams.Length == 0 && attrTypeParams.Length == 0)
        {
            return ParamAnalysisResult.Failure(
                ParametrizationKind.Unknown,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NoTypeParameter,
                    location: context.TypeSymbol.Locations[0],
                    messageArgs: context.TypeSymbol.Name));
        }

        // user specified types both using attribute and making type generic
        if (genericTypeParams.Length > 0 && attrTypeParams.Length > 0)
        {
            return ParamAnalysisResult.Failure(
                ParametrizationKind.Unknown,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: context.TypeSymbol.Locations[0],
                    messageArgs: context.TypeSymbol.Name));
        }

        return genericTypeParams.Length > 0
            ? genericAnalysisResult
            : attributeAnalysisResult;
    }
}

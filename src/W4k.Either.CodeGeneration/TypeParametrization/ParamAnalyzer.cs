using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace W4k.Either.TypeParametrization;

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
        
        // [Either<string, int>] partial struct E { }
        var genericAttributeAnalysisResult = GenericAttributeAnalyzer.Analyze(context, cancellationToken);
        if (!genericAttributeAnalysisResult.IsSuccess)
        {
            return genericAttributeAnalysisResult;
        }

        var typeSymbol = context.TypeSymbol;

        // handle `boolean` information about value as 0 or 1
        var hasGenericTypes = genericAnalysisResult.TypeParameters.HasValue();
        var hasAttrTypes = attributeAnalysisResult.TypeParameters.HasValue();
        var hasGenericAttrTypes = genericAttributeAnalysisResult.TypeParameters.HasValue();

        var sum = hasGenericTypes + hasAttrTypes + hasGenericAttrTypes;

        // user has not specified any type parameter
        if (sum == 0)
        {
            return ParamAnalysisResult.Failure(
                ParametrizationKind.Unknown,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NoTypeParameter,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // user specified types using attribute and generic type parameters
        // (this extends analysis of found marking attribute)
        if (sum > 1)
        {
            return ParamAnalysisResult.Failure(
                ParametrizationKind.Unknown,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        return hasGenericTypes == 1
            ? genericAnalysisResult
            : hasAttrTypes == 1
                ? attributeAnalysisResult
                : genericAttributeAnalysisResult;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int HasValue(this TypeParameter[] typeParameters) => 
        typeParameters.Length > 0 ? 1 : 0;
}

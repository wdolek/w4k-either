using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Either.TypeParametrization;

internal readonly struct ParamAnalysisResult
{
    private ParamAnalysisResult(bool isSuccess, ParametrizationKind kind, TypeParameter[] typeParameters, Diagnostic? diagnostic)
    {
        IsSuccess = isSuccess;
        ParametrizationKind = kind;
        TypeParameters = typeParameters;
        Diagnostic = diagnostic;
    }

    [MemberNotNullWhen(false, nameof(Diagnostic))]
    public bool IsSuccess { get; }

    public ParametrizationKind ParametrizationKind { get; }
    public TypeParameter[] TypeParameters { get; } = null!;
    public Diagnostic? Diagnostic { get; }

    public static ParamAnalysisResult Empty(ParametrizationKind kind) =>
        new(true, kind, [], null);

    public static ParamAnalysisResult Success(ParametrizationKind kind, TypeParameter[] typeParameters) =>
        new(true, kind, typeParameters, null);

    public static ParamAnalysisResult Failure(ParametrizationKind kind, Diagnostic diagnostic) =>
        new(false, kind, [], diagnostic);
}

internal enum ParametrizationKind
{
    Unknown = 0,
    Generic,
    Attribute,
    GenericAttribute,
}
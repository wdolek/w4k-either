using System;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration.Processors;

internal readonly struct ProcessorResult
{
    private ProcessorResult(bool isSuccess, TypeParameter[]? typeParameters, Diagnostic? diagnostic)
    {
        IsSuccess = isSuccess;
        TypeParameters = typeParameters;
        Diagnostic = diagnostic;
    }

    public bool IsSuccess { get; }
    public TypeParameter[]? TypeParameters { get; }
    public Diagnostic? Diagnostic { get; }

    public static ProcessorResult Empty() => new(true, Array.Empty<TypeParameter>(), null);
    public static ProcessorResult Success(TypeParameter[] typeParameters) => new(true, typeParameters, null);
    public static ProcessorResult Failure(Diagnostic diagnostic) => new(false, null, diagnostic);
}

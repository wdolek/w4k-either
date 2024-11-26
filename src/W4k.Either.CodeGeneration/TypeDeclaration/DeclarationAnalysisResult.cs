using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace W4k.Either.TypeDeclaration;

internal sealed class DeclarationAnalysisResult
{
    private DeclarationAnalysisResult(bool isValid, bool hasValue, Declaration? typeDeclaration, Diagnostic? diagnostic)
    {
        IsValid = isValid;
        HasValue = hasValue;
        TypeDeclaration = typeDeclaration;
        Diagnostic = diagnostic;
    }

    [MemberNotNullWhen(false, nameof(Diagnostic))]
    public bool IsValid { get; }

    [MemberNotNullWhen(true, nameof(TypeDeclaration))]
    public bool HasValue { get; }

    public Declaration? TypeDeclaration { get; }
    public Diagnostic? Diagnostic { get; }

    public static DeclarationAnalysisResult Valid(Declaration typeDeclaration) =>
        new(true, true, typeDeclaration, null);

    public static DeclarationAnalysisResult None() =>
        new(true, false, null, null);

    public static DeclarationAnalysisResult Invalid(Diagnostic diagnostic) =>
        new(false, false, null, diagnostic);
}
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.TypeDeclaration;

internal class Declaration
{
    public required INamedTypeSymbol TypeSymbol { get; init; } = null!;
    public required string DeclaredTypeName { get; init; } = null!;
    public required string FullDeclaration { get; init; } = null!;

    public string TargetNamespace =>
        TypeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : TypeSymbol.ContainingNamespace.ToDisplayString();
}

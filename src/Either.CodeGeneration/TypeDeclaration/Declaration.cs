using Microsoft.CodeAnalysis;

namespace Either.TypeDeclaration;

/// <summary>
/// Representation of type declaration.
/// </summary>
internal sealed class Declaration
{
    /// <summary>
    /// Initializes new instance of <see cref="Declaration"/>.
    /// </summary>
    /// <param name="typeSymbol">Named type symbol for type declaration</param>
    /// <param name="typeName">Declared type name</param>
    /// <param name="fullDeclaration">Full declaration of type</param>
    public Declaration(INamedTypeSymbol typeSymbol, string typeName, string fullDeclaration)
    {
        TypeSymbol = typeSymbol;
        DeclaredTypeName = typeName;
        FullDeclaration = fullDeclaration;
    }

    /// <summary>
    /// Gets named type symbol for type declaration.
    /// </summary>
    public INamedTypeSymbol TypeSymbol { get; }

    /// <summary>
    /// Gets declared type name, including generic type parameters, e.g. <c>Either&lt;TLeft, TRight&gt;</c>.
    /// </summary>
    public string DeclaredTypeName { get; }

    /// <summary>
    /// Gets full declaration of type, including access modifiers, e.g. <c>public partial struct Either&lt;TLeft, TRight&gt;</c>.
    /// </summary>
    public string FullDeclaration { get; }

    /// <summary>
    /// Gets namespace of declared type.
    /// </summary>
    public string TargetNamespace =>
        TypeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : TypeSymbol.ContainingNamespace.ToDisplayString();
}
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration;

internal static class GeneratorHelpers
{
    public static bool IsPartial(INamedTypeSymbol namedTypeSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var syntaxRef in namedTypeSymbol.DeclaringSyntaxReferences)
        {
            var syntaxNode = syntaxRef.GetSyntax();
            if (syntaxNode is TypeDeclarationSyntax structDeclaration)
            {
                if (structDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static (ContainingTypeDeclaration?, Diagnostic?) GetContainingTypeDeclaration(
        INamedTypeSymbol targetSymbol,
        CancellationToken cancellationToken)
    {
        var parentTypeSymbol = targetSymbol.ContainingType;

        // target type is not nested
        if (parentTypeSymbol is null)
        {
            return (null, null);
        }

        // containing type is not partial
        if (!IsPartial(parentTypeSymbol, cancellationToken))
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeMustBePartial,
                location: parentTypeSymbol.Locations[0],
                messageArgs: parentTypeSymbol.Name);

            return (null, diagnostic);
        }

        cancellationToken.ThrowIfCancellationRequested();
        
        // find whole type declaration
        foreach (var declaringSyntax in parentTypeSymbol.DeclaringSyntaxReferences)
        {
            var syntaxNode = declaringSyntax.GetSyntax(cancellationToken);
            if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
            {
                continue;
            }

            return (GetDeclaration(typeDeclarationSyntax), null);
        }

        return (null, null);
    }

    private static ContainingTypeDeclaration GetDeclaration(TypeDeclarationSyntax typeDeclaration)
    {
        var sb = new StringBuilder();

        // modifiers
        foreach (var modifier in typeDeclaration.Modifiers)
        {
            sb.Append(modifier.Text);
            sb.Append(" ");
        }

        // class or struct keyword
        sb.Append(typeDeclaration.Keyword.Text);
        sb.Append(" ");

        // type name
        var typeNamePos = sb.Length;
        sb.Append(typeDeclaration.Identifier.Text);

        // type parameters  
        if (typeDeclaration.TypeParameterList is not null)
        {
            sb.Append(typeDeclaration.TypeParameterList);
        }     

        var fullDeclaration = sb.ToString();
        var typeName = fullDeclaration.Substring(typeNamePos);
        
        return new(typeName, fullDeclaration);
    }
}

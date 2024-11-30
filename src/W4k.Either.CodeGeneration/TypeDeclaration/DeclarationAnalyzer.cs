using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace W4k.Either.TypeDeclaration;

internal static class DeclarationAnalyzer
{
    public static DeclarationAnalysisResult Analyze(INamedTypeSymbol? typeSymbol, CancellationToken cancellationToken)
    {
        if (typeSymbol is null)
        {
            return DeclarationAnalysisResult.None();
        }

        var (declarationSyntax, isPartial, isStatic) = FindDeclarationSyntax(typeSymbol, cancellationToken);

        if (!isPartial)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeMustBePartial,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return DeclarationAnalysisResult.Invalid(diagnostic);
        }

        if (isStatic)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeCannotBeStatic,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return DeclarationAnalysisResult.Invalid(diagnostic);
        }

        var (declaredTypeName, fullDeclaration) = GetDeclaration(declarationSyntax!);
        var typeDeclaration = new Declaration(typeSymbol, declaredTypeName, fullDeclaration);

        return DeclarationAnalysisResult.Valid(typeDeclaration);
    }

    private static (TypeDeclarationSyntax? DeclarationSyntax, bool IsPartial, bool IsStatic) FindDeclarationSyntax(
        INamedTypeSymbol typeSymbol,
        CancellationToken cancellationToken)
    {
        TypeDeclarationSyntax? foundTypeDeclarationSyntax = null;
        var modifiersFound = 0;
        var isStatic = false;
        var isPartial = false;

        foreach (var syntaxRef in typeSymbol.DeclaringSyntaxReferences)
        {
            var syntaxNode = syntaxRef.GetSyntax(cancellationToken);
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                foundTypeDeclarationSyntax = typeDeclarationSyntax;

                if (typeDeclarationSyntax.Modifiers.Any(SyntaxKind.StaticKeyword))
                {
                    isStatic = true;
                    ++modifiersFound;
                }

                if (typeDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    isPartial = true;
                    ++modifiersFound;
                }

                // don't check modifiers which are irrelevant for analyzer
                if (modifiersFound == 2)
                {
                    break;
                }
            }
        }

        return (foundTypeDeclarationSyntax, isPartial, isStatic);
    }

    private static (string DeclaredTypeName, string FullDeclaration) GetDeclaration(TypeDeclarationSyntax typeDeclaration)
    {
        var sb = new StringBuilder();

        // modifiers
        foreach (var modifier in typeDeclaration.Modifiers)
        {
            sb.Append(modifier.Text);
            sb.Append(' ');
        }

        // class or struct keyword
        sb.Append(typeDeclaration.Keyword.Text);
        sb.Append(' ');

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

        return (typeName, fullDeclaration);
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.TypeDeclaration;
using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration.Generator;

internal sealed class GeneratorContext
{
    public GeneratorContext(TransformationResult transformationResult)
    {
        if (!transformationResult.IsValid)
        {
            ThrowOnInvalidTransformationResult(transformationResult);
        }

        TypeKind = transformationResult.TypeKind;
        TypeDeclaration = transformationResult.TypeDeclaration;
        ContainingTypeDeclaration = transformationResult.ContainingTypeDeclaration;
        ParametrizationKind = transformationResult.ParametrizationKind;
        TypeParameters = transformationResult.TypeParameters;
    }
    
    public TypeKind TypeKind { get; }
    public Declaration TypeDeclaration { get; }
    public Declaration? ContainingTypeDeclaration { get; }
    public ParametrizationKind ParametrizationKind { get; }
    public TypeParameter[] TypeParameters { get; }

    public string GetFileName()
    {
        var typeSymbol = TypeDeclaration.TypeSymbol;

        var containingTypeDeclaration = ContainingTypeDeclaration;
        if (containingTypeDeclaration is not null)
        {
            return typeSymbol.IsGenericType
                ? $"{containingTypeDeclaration.TypeSymbol.Name}.{typeSymbol.Name}`{TypeParameters.Length}.g.cs"
                : $"{containingTypeDeclaration.TypeSymbol.Name}.{typeSymbol.Name}.g.cs";
        }

        return typeSymbol.IsGenericType
            ? $"{typeSymbol.Name}`{TypeParameters.Length}.g.cs"
            : $"{typeSymbol.Name}.g.cs";
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowOnInvalidTransformationResult(
        TransformationResult transformationResult,
        [CallerArgumentExpression(nameof(transformationResult))] string exp = "")
    {
        throw new ArgumentException("Transformation result is invalid.", exp);
    }
}

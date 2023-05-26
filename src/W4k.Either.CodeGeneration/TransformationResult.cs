using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.TypeDeclaration;
using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration;

internal class TransformationResult
{
    private TransformationResult(TypeKind typeKind, Diagnostic diagnostic)
    {
        IsValid = false;

        TypeKind = typeKind;
        TypeDeclaration = null;
        ContainingTypeDeclaration = null;
        ParametrizationKind = ParametrizationKind.Unknown;
        TypeParameters = Array.Empty<TypeParameter>();
        
        Diagnostics.Add(diagnostic);
    }    
    
    private TransformationResult(
        TypeKind typeKind,
        Declaration? typeDeclaration,
        Declaration? containingTypeDeclaration,
        ParametrizationKind parametrizationKind,
        TypeParameter[] typeParameters)
    {
        IsValid = true;

        TypeKind = typeKind;
        TypeDeclaration = typeDeclaration;
        ContainingTypeDeclaration = containingTypeDeclaration;
        ParametrizationKind = parametrizationKind;
        TypeParameters = typeParameters;
    }    

    [MemberNotNullWhen(true, nameof(TypeDeclaration))]
    public bool IsValid { get; }
    public TypeKind TypeKind { get; }
    public Declaration? TypeDeclaration { get; }
    public Declaration? ContainingTypeDeclaration { get; }
    public ParametrizationKind ParametrizationKind { get; }
    public TypeParameter[] TypeParameters { get; }
    public List<Diagnostic> Diagnostics { get; } = new();

    public static TransformationResult Valid(
        TypeKind typeKind,
        Declaration typeDeclaration,
        Declaration? containingTypeDeclaration,
        ParametrizationKind parametrizationKind,
        TypeParameter[] typeParameters) =>
        new(typeKind, typeDeclaration, containingTypeDeclaration, parametrizationKind, typeParameters);
    
    public static TransformationResult Invalid(Diagnostic diagnostic) =>
        new(TypeKind.Unknown, diagnostic);
}
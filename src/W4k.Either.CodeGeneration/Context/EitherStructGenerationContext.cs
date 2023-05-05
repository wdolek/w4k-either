using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.Context;

[DebuggerDisplay("{TargetNamespace}.{TargetTypeName}")]
internal sealed class EitherStructGenerationContext
{
    private readonly List<Diagnostic> _diagnostics = new();
    
    private EitherStructGenerationContext(
        bool isGeneric,
        string targetNamespace,
        ContainingTypeDeclaration? containingTypeDeclaration,
        string targetTypeName,
        TypeParameter[] typeParameters,
        TypeConstructor[] declaredConstructors,
        Diagnostic? diagnostic)
    {
        IsGenericType = isGeneric;
        TargetNamespace = targetNamespace;
        ContainingTypeDeclaration = containingTypeDeclaration;
        TargetTypeName = targetTypeName;
        TypeParameters = typeParameters;
        DeclaredConstructors = declaredConstructors;

        if (diagnostic is not null)
        {
            _diagnostics.Add(diagnostic);
        }
        
        // computed props (after all other props set!)
        FullTypeName = CreateFullTypeName();
        ReferringTypeName = CreateReferringTypeName(FullTypeName);
    }

    public bool IsGenericType { get; }
    public string TargetNamespace { get; }
    public string TargetTypeName { get; }
    public string FullTypeName { get; }
    public string ReferringTypeName { get; }
    public ContainingTypeDeclaration? ContainingTypeDeclaration { get; }
    public TypeParameter[] TypeParameters { get; }
    public TypeConstructor[] DeclaredConstructors { get; }
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;

    public string FileName => IsGenericType
        ? $"{TargetTypeName}`{TypeParameters.Length}.g.cs"
        : $"{TargetTypeName}.g.cs";

    public static EitherStructGenerationContext Generic(
        string @namespace,
        ContainingTypeDeclaration? parentTypeDeclaration,
        string typeName,
        TypeParameter[] typeParameters,
        TypeConstructor[] declaredConstructors) =>
        new(isGeneric: true, @namespace, parentTypeDeclaration, typeName, typeParameters, declaredConstructors, diagnostic: null);

    public static EitherStructGenerationContext NonGeneric(
        string @namespace,
        ContainingTypeDeclaration? parentTypeDeclaration,
        string typeName,
        TypeParameter[] typeParameters,
        TypeConstructor[] declaredConstructors) =>
        new(isGeneric: false, @namespace, parentTypeDeclaration, typeName, typeParameters, declaredConstructors, diagnostic: null);

    public static EitherStructGenerationContext Invalid(Diagnostic diagnostic) =>
        new(
            isGeneric: false,
            targetNamespace: "",
            containingTypeDeclaration: null,
            targetTypeName: "",
            Array.Empty<TypeParameter>(),
            Array.Empty<TypeConstructor>(),
            diagnostic);

    public bool IsDefaultCtorDeclared()
    {
        var declaredConstructors = DeclaredConstructors;
        foreach (var ctorTypeParam in declaredConstructors)
        {
            if (ctorTypeParam.IsParameterless)
            {
                return true;
            }
        }

        return false;
    }
    
    public bool IsCtorDeclared(TypeParameter typeParameter)
    {
        var declaredConstructors = DeclaredConstructors;
        foreach (var ctorTypeParam in declaredConstructors)
        {
            if (ctorTypeParam.ParamTypeName == typeParameter.Name)
            {
                return true;
            }
        }

        return false;
    }
    
    private string CreateFullTypeName()
    {
        if (!IsGenericType)
        {
            return TargetTypeName;
        }

        var typeParams = TypeParameters;
        var sb = new StringBuilder(64);

        sb.Append(TargetTypeName);
        sb.Append("<");

        for (var i = 0; i < typeParams.Length; i++)
        {
            sb.Append(typeParams[i].Name);
            if (i < typeParams.Length - 1)
            {
                sb.Append(", ");
            }
        }
        
        sb.Append(">");

        return sb.ToString();
    }

    private string CreateReferringTypeName(string typeName) =>
        ContainingTypeDeclaration is null
            ? typeName
            : $"{ContainingTypeDeclaration.TypeName}.{typeName}";
}

internal sealed class ContainingTypeDeclaration
{
    public ContainingTypeDeclaration(string typeName, string fullDeclaration)
    {
        TypeName = typeName;
        FullDeclaration = fullDeclaration;
    }

    public string TypeName { get; }
    public string FullDeclaration { get; }
}

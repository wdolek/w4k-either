using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.Context;

[DebuggerDisplay("{TargetNamespace}.{TargetTypeName}")]
internal sealed class EitherStructGenerationContext
{
    private readonly List<Diagnostic> _diagnostics = new();
    
    private EitherStructGenerationContext(
        bool isGeneric,
        string targetNamespace,
        string targetTypeName,
        TypeParameter[] typeParameters)
    {
        IsGenericType = isGeneric;
        TargetNamespace = targetNamespace;
        TargetTypeName = targetTypeName;
        TypeParameters = typeParameters;
    }

    public bool IsGenericType { get; }
    public string TargetNamespace { get; }
    public string TargetTypeName { get; }
    public TypeParameter[] TypeParameters { get; }
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;
    public string FileName => IsGenericType
        ? $"{TargetTypeName}`{TypeParameters.Length}.g.cs"
        : $"{TargetTypeName}.g.cs";

    public static EitherStructGenerationContext Generic(
        string @namespace,
        string typeName,
        TypeParameter[] typeParameters) =>
        new(true, @namespace, typeName, typeParameters);

    public static EitherStructGenerationContext NonGeneric(
        string @namespace,
        string typeName,
        TypeParameter[] typeParameters) =>
        new(false, @namespace, typeName, typeParameters);

    public static EitherStructGenerationContext Invalid(string @namespace, string typeName, Diagnostic diagnostics) =>
        new EitherStructGenerationContext(false, @namespace, typeName, Array.Empty<TypeParameter>())
            .AddDiagnostic(diagnostics);

    private EitherStructGenerationContext AddDiagnostic(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
        return this;
    }
}

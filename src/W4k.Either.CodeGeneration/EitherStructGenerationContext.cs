using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration;

internal sealed class EitherStructGenerationContext
{
    private readonly List<Diagnostic> _diagnostics = new();
    
    private EitherStructGenerationContext(
        bool isGeneric,
        string targetNamespace,
        string targetTypeName,
        IReadOnlyList<string> typeParameters)
    {
        IsGenericType = isGeneric;
        TargetNamespace = targetNamespace;
        TargetTypeName = targetTypeName;
        TypeParameters = typeParameters;
    }

    public bool IsGenericType { get; }
    public string TargetNamespace { get; }
    public string TargetTypeName { get; }
    public IReadOnlyList<string> TypeParameters { get; }
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();

    public static EitherStructGenerationContext Generic(string @namespace, string typeName, IReadOnlyList<string> typeParameters) =>
        new(true, @namespace, typeName, typeParameters);

    public static EitherStructGenerationContext NonGeneric(string @namespace, string typeName, IReadOnlyList<string> typeArguments) =>
        new(false, @namespace, typeName, typeArguments);

    public static EitherStructGenerationContext Invalid(string @namespace, string typeName, Diagnostic diagnostics) =>
        new EitherStructGenerationContext(false, @namespace, typeName, Array.Empty<string>())
            .AddDiagnostic(diagnostics);

    public EitherStructGenerationContext AddDiagnostic(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
        return this;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration;

[DebuggerDisplay("{TargetNamespace}.{TargetTypeName}")]
internal sealed class EitherStructGenerationContext
{
    private readonly List<Diagnostic> _diagnostics = new();
    
    private EitherStructGenerationContext(
        bool isGeneric,
        string targetNamespace,
        string targetTypeName,
        IReadOnlyList<TypeParameter> typeParameters)
    {
        IsGenericType = isGeneric;
        TargetNamespace = targetNamespace;
        TargetTypeName = targetTypeName;
        TypeParameters = typeParameters;
    }

    public bool IsGenericType { get; }
    public string TargetNamespace { get; }
    public string TargetTypeName { get; }
    public IReadOnlyList<TypeParameter> TypeParameters { get; }
    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;

    public static EitherStructGenerationContext Generic(
        string @namespace,
        string typeName,
        IReadOnlyList<TypeParameter> typeParameters) =>
        new(true, @namespace, typeName, typeParameters);

    public static EitherStructGenerationContext NonGeneric(
        string @namespace,
        string typeName,
        IReadOnlyList<TypeParameter> typeParameters) =>
        new(false, @namespace, typeName, typeParameters);

    public static EitherStructGenerationContext Invalid(string @namespace, string typeName, Diagnostic diagnostics) =>
        new EitherStructGenerationContext(false, @namespace, typeName, Array.Empty<TypeParameter>())
            .AddDiagnostic(diagnostics);

    private EitherStructGenerationContext AddDiagnostic(Diagnostic diagnostic)
    {
        _diagnostics.Add(diagnostic);
        return this;
    }

    [DebuggerDisplay("{Index} -> {ParameterName} ({ArgumentName})")]
    public class TypeParameter
    {
        private readonly string _name;

        public TypeParameter(int index, string name, bool isValueType, bool isNullable)
        {
            _name = name;

            Index = index;
            IsValueType = isValueType;
            IsNullable = isNullable;
        }

        public int Index { get; }
        public bool IsValueType { get; }
        public bool IsNullable { get; }

        public string ParameterName => _name;

        public string ArgumentName => IsNullable
            ? _name + "?"
            : _name;
    }
}

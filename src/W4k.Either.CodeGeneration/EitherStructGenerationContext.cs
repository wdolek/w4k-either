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
    public string FileName => IsGenericType
        ? $"{TargetTypeName}`{TypeParameters.Count}.g.cs"
        : $"{TargetTypeName}.g.cs";

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

    [DebuggerDisplay("{Index} -> {Name} ({ArgumentName})")]
    public class TypeParameter
    {
        private readonly bool _isNullable;

        public TypeParameter(int index, string name, bool isReferenceType, bool isValueType, bool isNullable)
        {
            Index = index;

            Name = name;
            ArgumentName = isNullable
                ? name + "?"
                : name;

            // for unconstrained type parameters, both `isReferenceType` and `isValueType` are false -> treat as reference type
            IsReferenceType = isReferenceType || (!isReferenceType && !isValueType);
            IsValueType = isValueType;

            _isNullable = isNullable;
        }

        public int Index { get; }
        public string Name { get; }
        public string ArgumentName { get; }
        public bool IsReferenceType { get; }
        public bool IsValueType { get; }
        public bool IsNullableReferenceType => IsReferenceType && _isNullable;
        public bool IsNonNullableReferenceType => IsReferenceType && !_isNullable;
    }
}

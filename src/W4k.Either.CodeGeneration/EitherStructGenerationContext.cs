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

    [DebuggerDisplay("{Index} -> {Name}")]
    public class TypeParameter
    {
        public TypeParameter(int index, string name, bool isReferenceType, bool isValueType, bool isNullable)
        {
            Index = index;
            Name = name;
            FieldName = "_v" + index;
            IsNullable = isNullable;
            IsValueType = isValueType;

            // type is unconstrained whenever it has no `class` or `struct` constraint
            // in such case, both `isReferenceType` and `isValueType` are false
            var isUnconstrained = !isReferenceType && !isValueType;
            
            // treat unconstrained type as reference type
            IsReferenceType = isReferenceType || isUnconstrained;

            // there are two rules:
            // - reference type is always stored as nullable
            // - value type is stored as nullable only if explicitly nullable
            // and additional exception when specifying types using attribute - in such case, nullable value type already contains `?`
            AsFieldType = (isNullable || IsReferenceType) && !name.EndsWith("?")
                ? name + "?"
                : name;

            AsArgument = isNullable && IsReferenceType
                ? name + "?"
                : name;

            AsFieldInvoker = IsReferenceType switch
            {
                // non-nullable reference type -> use null-forgiving operator as we know value is not null
                true when !isNullable => FieldName + "!",
                
                // non-nullable value type -> value type cannot be null
                false when !isNullable => FieldName,
                
                // nullable (undetermined)
                _ => FieldName + "?",
            };

            AsFieldReceiver = !isNullable && IsReferenceType
                ? FieldName + "!"
                : FieldName;

            AsDefault = IsNonNullableReferenceType
                ? "default!"
                : "default";
        }

        public int Index { get; }
        public string Name { get; }
        public string FieldName { get; set; }
        public bool IsNullable { get; }
        public bool IsValueType { get; }
        public bool IsReferenceType { get; }
        public bool IsNullableReferenceType => IsReferenceType && IsNullable;
        public bool IsNonNullableReferenceType => IsReferenceType && !IsNullable;
        public string AsFieldType { get; }
        public string AsFieldInvoker { get; }
        public string AsFieldReceiver { get; }
        public string AsArgument { get; }
        public string AsDefault { get; }
    }
}

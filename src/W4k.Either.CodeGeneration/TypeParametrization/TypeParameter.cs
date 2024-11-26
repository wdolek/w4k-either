using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace W4k.Either.TypeParametrization;

[DebuggerDisplay("{Index} -> {Name}")]
internal sealed class TypeParameter
{
    public TypeParameter(ITypeSymbol typeSymbol, int index, string name, bool isReferenceType, bool isValueType, bool isNullable)
    {
        TypeSymbol = typeSymbol;
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
        AsFieldType = (isNullable || IsReferenceType) && !name.EndsWith("?", StringComparison.Ordinal)
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

    public ITypeSymbol TypeSymbol { get; }

    public int Index { get; }

    public bool IsNullable { get; }
    public bool IsValueType { get; }
    public bool IsReferenceType { get; }

    public string Name { get; }
    public string FieldName { get; }
    public string AsFieldType { get; }
    public string AsFieldInvoker { get; }
    public string AsFieldReceiver { get; }
    public string AsArgument { get; }
    public string AsDefault { get; }

    public bool IsNullableReferenceType => IsReferenceType && IsNullable;
    public bool IsNonNullableReferenceType => IsReferenceType && !IsNullable;
}
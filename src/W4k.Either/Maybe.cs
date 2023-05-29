using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace W4k.Either;

public static class Maybe
{
    public static Maybe<T> Some<T>(T value)
        where T : notnull
        => new(value);

    public static Maybe<T> None<T>() => new();
}

[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Maybe<T> : IEquatable<Maybe<T>>, ISerializable
    where T : notnull
{
    public Maybe()
    {
        _idx = 1;
        _v1 = default;

        HasValue = false;
    }

    internal Maybe(T? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _idx = 1;
        _v1 = value;

        HasValue = true;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    public T? Value => _v1;
}

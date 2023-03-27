using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace W4k.Either;

public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Default = new();

    [Pure]
    public static bool operator ==(Unit left, Unit right) => true;

    [Pure]
    public static bool operator !=(Unit left, Unit right) => false;

    [Pure]
    public override int GetHashCode() => 0;

    [Pure]
    public override string ToString() => "()";

    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unit;

    [Pure]
    public bool Equals(Unit other) => true;
}

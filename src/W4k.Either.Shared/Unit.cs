using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace W4k.Either;

/// <summary>
/// Unit type representing <c>void</c>.
/// </summary>
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "`Unit` has no values to compare, yet left/right params are required for operators.")]
[SuppressMessage("Performance", "CA1805:Do not initialize unnecessarily", Justification = "Being explicit with `Default` to not let Rider annoy me about unassigned readonly field.")]
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// The default and only instance of the <see cref="Unit"/> type.
    /// </summary>
    public static readonly Unit Default = new();

    /// <summary>
    /// Determines whether two <see cref="Unit"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Unit"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Unit"/> instance to compare.</param>
    /// <returns>Always <c>true</c>, as there is only one instance of the <see cref="Unit"/> type.</returns>
    [Pure]
    public static bool operator ==(Unit left, Unit right) => true;

    /// <summary>
    /// Determines whether two <see cref="Unit"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Unit"/> instance to compare.</param>
    /// <param name="right">The second <see cref="Unit"/> instance to compare.</param>
    /// <returns>Always <c>false</c>, as there is only one instance of the <see cref="Unit"/> type.</returns>
    [Pure]
    public static bool operator !=(Unit left, Unit right) => false;

    /// <summary>
    /// Retrieves the hash code for the <see cref="Unit"/> instance.
    /// </summary>
    /// <returns>Always <c>0</c>, as there is only one instance of the <see cref="Unit"/> type.</returns>
    [Pure]
    public override int GetHashCode() => 0;

    /// <summary>
    /// Converts the <see cref="Unit"/> instance to its string representation.
    /// </summary>
    /// <returns>A string representing the <see cref="Unit"/> instance, which is <c>"()"</c>.</returns>
    [Pure]
    public override string ToString() => "()";

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Unit"/> instance.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Unit"/> instance; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Unit;

    /// <summary>
    /// Determines whether the specified <see cref="Unit"/> instance is equal to the current <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Unit"/> instance to compare with the current <see cref="Unit"/> instance.</param>
    /// <returns>Always <c>true</c>, as there is only one instance of the <see cref="Unit"/> type.</returns>
    [Pure]
    public bool Equals(Unit other) => true;
}
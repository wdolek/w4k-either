using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace W4k.Either;

/// <summary>
/// Provides static methods for creating instances of the <see cref="Maybe{T}"/> struct.
/// </summary>
public static class Maybe
{
    /// <summary>
    /// Creates a new instance of the <see cref="Maybe{T}"/> struct with a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to be wrapped in the Maybe struct.</param>
    /// <returns>A Maybe struct instance containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified value is null.</exception>
    public static Maybe<T> Some<T>(T value)
        where T : notnull
        => new(value);

    /// <summary>
    /// Creates a new instance of the <see cref="Maybe{T}"/> struct with no value.
    /// </summary>
    /// <typeparam name="T">The type of the value (which is not present).</typeparam>
    /// <returns>An empty Maybe struct.</returns>
    public static Maybe<T> None<T>() 
        where T : notnull
        => new();
}

/// <summary>
/// Represents a value that may or may not be present.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Maybe<T> : IEquatable<Maybe<T>>, ISerializable
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Maybe{T}"/> struct with no value.
    /// </summary>
    public Maybe()
    {
        _idx = 1;
        _v1 = default;

        HasValue = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Maybe{T}"/> struct with a specified value.
    /// </summary>
    /// <param name="value">The value to be wrapped in the Maybe struct.</param>
    public Maybe(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _idx = 1;
        _v1 = value;

        HasValue = true;
    }

    /// <summary>
    /// Gets a value indicating whether this <see cref="Maybe{T}"/> struct has a value.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    /// <summary>
    /// Gets the value contained in this Maybe struct.
    /// </summary>
    /// <remarks>
    /// This property returns <c>null</c> (or <c>default</c>) if the <see cref="Maybe{T}"/> struct has no value.
    /// </remarks>
    public T? Value => _v1;
}

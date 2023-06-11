using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace W4k.Either;

/// <summary>
/// Provides static methods for creating instances of the <see cref="OptionalResult{TSuccess, TError}"/> struct.
/// </summary>
public static class OptionalResult
{
    /// <summary>
    /// Creates a new instance of the <see cref="OptionalResult{TSuccess, TError}"/> struct representing a successful result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TError">The type of the error value.</typeparam>
    /// <param name="success">The success value.</param>
    /// <returns>An <see cref="OptionalResult{TSuccess, TError}"/> struct representing a successful result with the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified success value is null.</exception>
    public static OptionalResult<TSuccess, TError> Success<TSuccess, TError>(TSuccess success)
        where TSuccess : notnull
        where TError : notnull
        => new(success);

    /// <summary>
    /// Creates a new instance of the <see cref="OptionalResult{TSuccess, TError}"/> struct representing an empty result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value (which is not present).</typeparam>
    /// <typeparam name="TError">The type of the error value (which is not present).</typeparam>
    /// <returns>An empty <see cref="OptionalResult{TSuccess, TError}"/> struct.</returns>
    public static OptionalResult<TSuccess, TError> Empty<TSuccess, TError>()
        where TSuccess : notnull
        where TError : notnull
        => new();

    /// <summary>
    /// Creates a new instance of the <see cref="OptionalResult{TSuccess, TError}"/> struct representing a failed result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value (which is not present).</typeparam>
    /// <typeparam name="TError">The type of the error value.</typeparam>
    /// <param name="error">The error value.</param>
    /// <returns>An <see cref="OptionalResult{TSuccess, TError}"/> struct representing a failed result with the specified error value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified error value is null.</exception>
    public static OptionalResult<TSuccess, TError> Failed<TSuccess, TError>(TError error)
        where TSuccess : notnull
        where TError : notnull
        => new(error);
}

/// <summary>
/// Represents a result that may or may not contain a success value or an error value.
/// </summary>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
/// <typeparam name="TError">The type of the error value.</typeparam>
[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct OptionalResult<TSuccess, TError> : IEquatable<OptionalResult<TSuccess, TError>>, ISerializable
    where TSuccess : notnull
    where TError : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionalResult{TSuccess, TError}"/> struct representing an empty but successful result.
    /// </summary>
    public OptionalResult()
    {
        _idx = 1;
        _v1 = default;
        _v2 = default;

        HasValue = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionalResult{TSuccess, TError}"/> struct representing a successful result.
    /// </summary>
    /// <param name="value">The success value.</param>
    public OptionalResult(TSuccess value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _idx = 1;
        _v1 = value;
        _v2 = default;

        HasValue = true;
    } 
    
    /// <summary>
    /// Gets a value indicating whether this <see cref="OptionalResult{TSuccess, TError}"/> represents a successful result.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _idx == 1;
    
    /// <summary>
    /// Gets a value indicating whether this <see cref="OptionalResult{TSuccess, TError}"/> represents a failed result.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed => _idx == 2;

    /// <summary>
    /// Gets a value indicating whether this <see cref="OptionalResult{TSuccess, TError}"/> has a value, implies <see cref="IsSuccess"/>.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; } = false;
    
    /// <summary>
    /// Deconstructs the <see cref="OptionalResult{TSuccess, TError}"/> into its constituent values.
    /// </summary>
    /// <param name="value">When this method returns, contains the success value if the <see cref="OptionalResult{TSuccess, TError}"/> represents a successful result; otherwise, the default value.</param>
    /// <param name="error">When this method returns, contains the error value if the <see cref="OptionalResult{TSuccess, TError}"/> represents a failed result; otherwise, the default value.</param>
    public void Deconstruct(out TSuccess? value, out TError? error)
    {
        switch (_idx)
        {
            case 1:
                value = _v1;
                error = default;
                break;

            case 2:
                value = default;
                error = _v2;
                break;

            default:
                ThrowHelper.ThrowOnInvalidState();
                value = default;
                error = default;
                break;
        }
    }

    /// <summary>
    /// Gets the success value if the <see cref="OptionalResult{TSuccess, TError}"/> represents a successful result; otherwise, throws an exception.
    /// </summary>
    /// <returns>The success value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="OptionalResult{TSuccess, TError}"/> represents a failed result.</exception>
    public TSuccess? Value =>
        _idx == 1
            ? _v1
            : ThrowHelper.ThrowOnInvalidState<TSuccess>();

    /// <summary>
    /// Gets the error value if the <see cref="OptionalResult{TSuccess, TError}"/> represents a failed result; otherwise, throws an exception.
    /// </summary>
    /// <returns>The error value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="OptionalResult{TSuccess, TError}"/> represents a successful result.</exception>
    public TError? Error =>
        _idx == 2
            ? _v2
            : ThrowHelper.ThrowOnInvalidState<TError>();
}

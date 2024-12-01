using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Either;

/// <summary>
/// Provides static methods for creating instances of the <see cref="Result{TError}"/> and <see cref="Result{TSuccess, TError}"/> structs.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a new instance of the <see cref="Result{TError}"/> struct representing a successful result.
    /// </summary>
    /// <typeparam name="TError">The type of the error value (which is not present).</typeparam>
    /// <returns>A Result struct representing a successful result.</returns>
    public static Result<TError> Success<TError>()
        where TError : notnull
        => new();

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TError}"/> struct representing a failed result.
    /// </summary>
    /// <typeparam name="TError">The type of the error value.</typeparam>
    /// <param name="error">The error value.</param>
    /// <returns>A Result struct representing a failed result with the specified error value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified error value is null.</exception>
    public static Result<TError> Failed<TError>(TError error)
        where TError : notnull
        => new(error);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TSuccess, TError}"/> struct representing a successful result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value.</typeparam>
    /// <typeparam name="TError">The type of the error value (which is not present).</typeparam>
    /// <param name="value">The success value.</param>
    /// <returns>A Result struct representing a successful result with the specified success value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified success value is null.</exception>
    public static Result<TSuccess, TError> Success<TSuccess, TError>(TSuccess value)
        where TSuccess : notnull
        where TError : notnull
        => new(value);

    /// <summary>
    /// Creates a new instance of the <see cref="Result{TSuccess, TError}"/> struct representing a failed result.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success value (which is not present).</typeparam>
    /// <typeparam name="TError">The type of the error value.</typeparam>
    /// <param name="error">The error value.</param>
    /// <returns>A Result struct representing a failed result with the specified error value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified error value is null.</exception>
    public static Result<TSuccess, TError> Failed<TSuccess, TError>(TError error)
        where TSuccess : notnull
        where TError : notnull
        => new(error);
}

/// <summary>
/// Represents a result that may or may not contain an error value.
/// </summary>
/// <typeparam name="TError">The type of the error value.</typeparam>
[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Result<TError> : IEquatable<Result<TError>>, ISerializable
    where TError : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> struct representing a successful result.
    /// </summary>
    public Result()
    {
        _idx = 1;
        _v1 = default;

        IsSuccess = true;
        IsFailed = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> struct representing a failed result.
    /// </summary>
    /// <param name="error">The error value.</param>
    /// <exception cref="ArgumentNullException">Thrown if the specified error value is null.</exception>
    public Result(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        _idx = 1;
        _v1 = error;

        IsSuccess = false;
        IsFailed = true;
    }

    /// <summary>
    /// Gets a value indicating whether this Result struct represents a successful result.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether this Result struct represents a failed result.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed { get; }

    /// <summary>
    /// Gets the error value if the Result struct represents a failed result; otherwise, throws an exception.
    /// </summary>
    /// <returns>The error value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result struct represents a successful result.</exception>
    public TError? Error =>
        IsFailed
            ? _v1
            : ThrowHelper.ThrowOnInvalidState<TError>();
}

/// <summary>
/// Represents a result that may or may not contain a success value or an error value.
/// </summary>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
/// <typeparam name="TError">The type of the error value.</typeparam>
[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Result<TSuccess, TError> : IEquatable<Result<TSuccess, TError>>, ISerializable
    where TSuccess : notnull
    where TError : notnull
{
    /// <summary>
    /// Gets a value indicating whether this Result struct represents a successful result.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _idx == 1;

    /// <summary>
    /// Gets a value indicating whether this Result struct represents a failed result.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed => _idx == 2;

    /// <summary>
    /// Deconstructs the Result into its constituent values.
    /// </summary>
    /// <param name="value">When this method returns, contains the success value if the Result represents a successful result; otherwise, the default value.</param>
    /// <param name="error">When this method returns, contains the error value if the Result represents a failed result; otherwise, the default value.</param>
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
    /// Gets the success value if the Result represents a successful result; otherwise, throws an exception.
    /// </summary>
    /// <returns>The success value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result represents a failed result.</exception>
    public TSuccess? Value =>
        _idx == 1
            ? _v1
            : ThrowHelper.ThrowOnInvalidState<TSuccess>();

    /// <summary>
    /// Gets the error value if the Result represents a failed result; otherwise, throws an exception.
    /// </summary>
    /// <returns>The error value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the Result represents a successful result.</exception>
    public TError? Error =>
        _idx == 2
            ? _v2
            : ThrowHelper.ThrowOnInvalidState<TError>();
}
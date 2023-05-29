using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace W4k.Either;

public static class Result
{
    public static Result<TError> Success<TError>()
        where TError : notnull
        => new();
    
    public static Result<TError> Failed<TError>(TError error)
        where TError : notnull
        => new(error);
    
    public static Result<TSuccess, TError> Success<TSuccess, TError>(TSuccess value)
        where TSuccess : notnull
        where TError : notnull
        => new(value);

    public static Result<TSuccess, TError> Failed<TSuccess, TError>(TError error)
        where TSuccess : notnull
        where TError : notnull
        => new(error);
}


[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Result<TError> : IEquatable<Result<TError>>, ISerializable
    where TError : notnull
{
    public Result()
    {
        _idx = 1;
        _v1 = default;

        IsSuccess = true;
        IsFailed = false;
    }

    internal Result(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);

        _idx = 1;
        _v1 = error;

        IsSuccess = false;
        IsFailed = true;
    }
    
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed { get; }

    public TError? Error =>
        IsFailed
            ? _v1
            : throw new InvalidOperationException();
}

[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Result<TSuccess, TError> : IEquatable<Result<TSuccess, TError>>, ISerializable
    where TSuccess : notnull
    where TError : notnull
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _idx == 1;
    
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed => _idx == 2;

    public TSuccess? Value =>
        _idx == 1
            ? _v1
            : throw new InvalidOperationException();
    
    public TError? Error =>
        _idx == 2
            ? _v2
            : throw new InvalidOperationException();
}

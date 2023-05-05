using System;
using System.Diagnostics.CodeAnalysis;

namespace W4k.Either;

public static class Result
{
    public static Result<TSuccess, TError> Success<TSuccess, TError>(TSuccess value)
        where TSuccess : notnull
        where TError : notnull
        => new(value);

    public static Result<TSuccess, TError> Failure<TSuccess, TError>(TError error)
        where TSuccess : notnull
        where TError : notnull
        => new(error);
}

[Either]
public readonly partial struct Result<TSuccess, TError>
    where TSuccess : notnull
    where TError : notnull
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _idx == 1;
    
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError => _idx == 2;

    public TSuccess? Value =>
        _idx == 1
            ? _v1
            : throw new InvalidOperationException();
    
    public TError? Error =>
        _idx == 2
            ? _v2
            : throw new InvalidOperationException();
}

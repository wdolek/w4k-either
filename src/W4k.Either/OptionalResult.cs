﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace W4k.Either;

public static class OptionalResult
{
    public static OptionalResult<TSuccess, TError> Success<TSuccess, TError>(TSuccess success)
        where TSuccess : notnull
        where TError : notnull
        => new(success);

    public static OptionalResult<TSuccess, TError> Empty<TSuccess, TError>()
        where TSuccess : notnull
        where TError : notnull
        => new();
    
    public static OptionalResult<TSuccess, TError> Failed<TSuccess, TError>(TError error)
        where TSuccess : notnull
        where TError : notnull
        => new(error);
}

[Either]
[Serializable]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct OptionalResult<TSuccess, TError> : IEquatable<OptionalResult<TSuccess, TError>>, ISerializable
    where TSuccess : notnull
    where TError : notnull
{
    public OptionalResult()
    {
        _idx = 1;
        _v1 = default;
        _v2 = default;

        HasValue = false;
    }

    internal OptionalResult(TSuccess value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _idx = 1;
        _v1 = value;
        _v2 = default;

        HasValue = true;
    } 
    
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _idx == 1;
    public bool IsError => _idx == 2;

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; } = false;

    public TSuccess? Value =>
        _idx == 1
            ? _v1
            : throw new InvalidOperationException();

    public TError? Error =>
        _idx == 2
            ? _v2
            : throw new InvalidOperationException();
}

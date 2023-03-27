using System;
using System.Diagnostics.CodeAnalysis;

namespace W4k.Either.Abstractions;

public static class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowOnInvalidState() => throw new InvalidOperationException();

    [DoesNotReturn]
    public static T ThrowOnInvalidState<T>() => throw new InvalidOperationException();
}

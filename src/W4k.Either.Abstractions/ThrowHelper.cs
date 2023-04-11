using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace W4k.Either.Abstractions;

public static class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowOnInvalidState() => throw new InvalidOperationException();

    [DoesNotReturn]
    public static T ThrowOnInvalidState<T>() => throw new InvalidOperationException();

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace W4k.Either.Abstractions;

/// <summary>
/// Provides helper methods for throwing exceptions related to invalid states.
/// </summary>
public static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> indicating an invalid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when this method is called, indicating an invalid state.</exception>
    [DoesNotReturn]
    public static void ThrowOnInvalidState() => throw new InvalidOperationException();

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> indicating an invalid state and returns a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to return. This method will never return a value, as it always throws an exception.</typeparam>
    /// <returns>This method does not return a value; it always throws an exception.</returns>
    /// <exception cref="InvalidOperationException">Thrown when this method is called, indicating an invalid state.</exception>
    [DoesNotReturn]
    public static T ThrowOnInvalidState<T>() => throw new InvalidOperationException();

    internal static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
}

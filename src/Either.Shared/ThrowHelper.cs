using System;
using System.Diagnostics.CodeAnalysis;

namespace Either;

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
}
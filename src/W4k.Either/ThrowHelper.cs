using System.Diagnostics.CodeAnalysis;

namespace W4k.Either;

internal static class ThrowHelper
{
    [DoesNotReturn]
    public static T ThrowOnInvalidState<T>() => throw new InvalidOperationException();
}

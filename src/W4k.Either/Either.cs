using System.Diagnostics.Contracts;

namespace W4k.Either;

public static class Either
{
    [Pure]
    public static Either<T0, T1> From<T0, T1>(T0 v0)
        where T0 : notnull
        where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(v0);
        return new(0, v0, default);
    }

    [Pure]
    public static Either<T0, T1> From<T0, T1>(T1 v1)
        where T0 : notnull
        where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(v1);
        return new(1, default, v1);
    }
}

using System;

namespace W4k.Either.Abstractions;

/// <summary>
/// Using source generator, <c>partial struct</c> decorated with this attribute will become choice monad.
/// </summary>
/// <remarks>
/// Minimum number of items <c>Either</c> can contain is 2 and maximum 8.
/// </remarks>
/// <example>
/// Generic choice monad:
/// <code>
/// [Either]
/// partial struct ThisOrThat&gt;TLeft, TRight&lt; { }
/// </code>
/// Or with explicit types:
/// <code>
/// [Either(typeof(int), typeof(string))]
/// partial struct IntOrString { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class EitherAttribute : Attribute
{
    public EitherAttribute()
    {
    }

    public EitherAttribute(Type t0, Type t1)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
    }

    public EitherAttribute(Type t0, Type t1, Type t2)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5, Type t6)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
        ThrowHelper.ThrowIfNull(t6);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5, Type t6, Type t7)
    {
        ThrowHelper.ThrowIfNull(t0);
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
        ThrowHelper.ThrowIfNull(t6);
        ThrowHelper.ThrowIfNull(t7);
    }
}

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
/// Or with C# 11.0 generic attributes:
/// <code>
/// [Either&gt;int, string&lt;]
/// partial struct IntOrString { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class EitherAttribute : Attribute
{
    public EitherAttribute()
        : this(0, null, null, null, null, null, null, null, null)
    {
    }

    public EitherAttribute(Type t0, Type t1)
        : this(2, t0, t1, null, null, null, null, null, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
    }

    public EitherAttribute(Type t0, Type t1, Type t2)
        : this(3, t0, t1, t2, null, null, null, null, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3)
        : this(4, t0, t1, t2, t3, null, null, null, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4)
        : this(5, t0, t1, t2, t3, t4, null, null, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5)
        : this(6, t0, t1, t2, t3, t4, t5, null, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5, Type t6)
        : this(7, t0, t1, t2, t3, t4, t5, t6, null)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
        ArgumentNullException.ThrowIfNull(t6);
    }

    public EitherAttribute(Type t0, Type t1, Type t2, Type t3, Type t4, Type t5, Type t6, Type t7)
        : this(8, t0, t1, t2, t3, t4, t5, t6, t7)
    {
        ArgumentNullException.ThrowIfNull(t0);
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
        ArgumentNullException.ThrowIfNull(t6);
        ArgumentNullException.ThrowIfNull(t7);
    }

    private EitherAttribute(
        byte numOfGenericTypes,
        Type? t0,
        Type? t1,
        Type? t2,
        Type? t3,
        Type? t4,
        Type? t5,
        Type? t6,
        Type? t7)
    {
        NumOfGenericTypes = numOfGenericTypes;
        T0 = t0;
        T1 = t1;
        T2 = t2;
        T3 = t3;
        T4 = t4;
        T5 = t5;
        T6 = t6;
        T7 = t7;
    }

    public byte NumOfGenericTypes { get; }
    public Type? T0 { get; }
    public Type? T1 { get; }
    public Type? T2 { get; }
    public Type? T3 { get; }
    public Type? T4 { get; }
    public Type? T5 { get; }
    public Type? T6 { get; }
    public Type? T7 { get; }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2, T3> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2), typeof(T3))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2, T3, T4> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2, T3, T4, T5> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2, T3, T4, T5, T6> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))
    {
    }
}

[AttributeUsage(AttributeTargets.Struct)]
public class EitherAttribute<T0, T1, T2, T3, T4, T5, T6, T7> : EitherAttribute
{
    public EitherAttribute()
        : base(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))
    {
    }
}

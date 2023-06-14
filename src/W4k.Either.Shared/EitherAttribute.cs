using System;

namespace W4k.Either;

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
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    public EitherAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    public EitherAttribute(Type t1, Type t2)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    /// <param name="t4">Type of fourth value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3, Type t4)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    /// <param name="t4">Type of fourth value.</param>
    /// <param name="t5">Type of fifth value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3, Type t4, Type t5)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    /// <param name="t4">Type of fourth value.</param>
    /// <param name="t5">Type of fifth value.</param>
    /// <param name="t6">Type of sixth value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3, Type t4, Type t5, Type t6)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
        ThrowHelper.ThrowIfNull(t6);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    /// <param name="t4">Type of fourth value.</param>
    /// <param name="t5">Type of fifth value.</param>
    /// <param name="t6">Type of sixth value.</param>
    /// <param name="t7">Type of seventh value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3, Type t4, Type t5, Type t6, Type t7)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
        ThrowHelper.ThrowIfNull(t6);
        ThrowHelper.ThrowIfNull(t7);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EitherAttribute"/> class.
    /// </summary>
    /// <remarks>
    /// Depending on nullable reference type scope, reference type may represent either nullable or non-nullable type.
    /// When <c>nullable</c> is enabled, reference type is not nullable, otherwise it is considered nullable (<c>?</c>).
    /// </remarks>
    /// <param name="t1">Type of first value.</param>
    /// <param name="t2">Type of second value.</param>
    /// <param name="t3">Type of third value.</param>
    /// <param name="t4">Type of fourth value.</param>
    /// <param name="t5">Type of fifth value.</param>
    /// <param name="t6">Type of sixth value.</param>
    /// <param name="t7">Type of seventh value.</param>
    /// <param name="t8">Type of eighth value.</param>
    public EitherAttribute(Type t1, Type t2, Type t3, Type t4, Type t5, Type t6, Type t7, Type t8)
    {
        ThrowHelper.ThrowIfNull(t1);
        ThrowHelper.ThrowIfNull(t2);
        ThrowHelper.ThrowIfNull(t3);
        ThrowHelper.ThrowIfNull(t4);
        ThrowHelper.ThrowIfNull(t5);
        ThrowHelper.ThrowIfNull(t6);
        ThrowHelper.ThrowIfNull(t7);
        ThrowHelper.ThrowIfNull(t8);
    }
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3, T4> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3, T4, T5> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3, T4, T5, T6> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3, T4, T5, T6, T7> : Attribute
{
}

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EitherAttribute<T1, T2, T3, T4, T5, T6, T7, T8> : Attribute
{
}

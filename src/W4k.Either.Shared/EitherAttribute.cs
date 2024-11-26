using System;

namespace W4k.Either;

/// <summary>
/// Using source generator, <c>partial struct</c> decorated with this attribute will become choice monad.
/// </summary>
/// <remarks>
/// Minimum number of types <c>Either</c> can use is 1 and maximum 8.
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
public sealed class EitherAttribute : EitherBaseAttribute
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
        ArgumentNullException.ThrowIfNull(t6);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
        ArgumentNullException.ThrowIfNull(t6);
        ArgumentNullException.ThrowIfNull(t7);
#endif
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
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(t1);
        ArgumentNullException.ThrowIfNull(t2);
        ArgumentNullException.ThrowIfNull(t3);
        ArgumentNullException.ThrowIfNull(t4);
        ArgumentNullException.ThrowIfNull(t5);
        ArgumentNullException.ThrowIfNull(t6);
        ArgumentNullException.ThrowIfNull(t7);
        ArgumentNullException.ThrowIfNull(t8);
#endif
    }
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of two predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string&gt;]
/// partial struct IntOrString { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of three predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool&gt;]
/// partial struct IntStringOrBool { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of four predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <typeparam name="T4">Type of the fourth value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool, double&gt;]
/// partial struct IntStringBoolOrDouble { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3, T4> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of five predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <typeparam name="T4">Type of the fourth value.</typeparam>
/// <typeparam name="T5">Type of the fifth value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool, double, char&gt;]
/// partial struct IntStringBoolDoubleOrChar { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3, T4, T5> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of six predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <typeparam name="T4">Type of the fourth value.</typeparam>
/// <typeparam name="T5">Type of the fifth value.</typeparam>
/// <typeparam name="T6">Type of the sixth value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool, double, char, float&gt;]
/// partial struct IntStringBoolDoubleCharOrFloat { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3, T4, T5, T6> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of seven predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <typeparam name="T4">Type of the fourth value.</typeparam>
/// <typeparam name="T5">Type of the fifth value.</typeparam>
/// <typeparam name="T6">Type of the sixth value.</typeparam>
/// <typeparam name="T7">Type of the seventh value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool, double, char, float, long&gt;]
/// partial struct IntStringBoolDoubleCharFloatOrLong { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3, T4, T5, T6, T7> : EitherBaseAttribute
{
}

/// <summary>
/// Using source generator, a <c>partial struct</c> decorated with this attribute will become a choice monad of eight predefined types.
/// </summary>
/// <typeparam name="T1">Type of the first value.</typeparam>
/// <typeparam name="T2">Type of the second value.</typeparam>
/// <typeparam name="T3">Type of the third value.</typeparam>
/// <typeparam name="T4">Type of the fourth value.</typeparam>
/// <typeparam name="T5">Type of the fifth value.</typeparam>
/// <typeparam name="T6">Type of the sixth value.</typeparam>
/// <typeparam name="T7">Type of the seventh value.</typeparam>
/// <typeparam name="T8">Type of the eighth value.</typeparam>
/// <example>
/// <code>
/// [Either&lt;int, string, bool, double, char, float, long, byte&gt;]
/// partial struct IntStringBoolDoubleCharFloatLongOrByte { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EitherAttribute<T1, T2, T3, T4, T5, T6, T7, T8> : EitherBaseAttribute
{
}

/// <summary>
/// Base Either attribute class.
/// </summary>
public abstract class EitherBaseAttribute : Attribute
{
    /// <summary>
    /// Gets or sets member names to skip by the source generator.
    /// </summary>
    public string[] Skip { get; set; } = [];
}
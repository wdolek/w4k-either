using System;
using System.Runtime.Serialization;

namespace W4k.Either;

/// <summary>
/// Represents an either type with two possible values: Left or Right.
/// </summary>
/// <typeparam name="TLeft">The type of the Left value.</typeparam>
/// <typeparam name="TRight">The type of the Right value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, ISerializable
    where TLeft : notnull
    where TRight : notnull
{
    /// <summary>
    /// Gets a value indicating whether this instance is a Left value.
    /// </summary>
    public bool IsLeft => _idx == 1;
    
    /// <summary>
    /// Gets a value indicating whether this instance is a Right value.
    /// </summary>
    public bool IsRight => _idx == 2;
}

/// <summary>
/// Represents an either type with three possible values: Left, Middle, or Right.
/// </summary>
/// <typeparam name="TLeft">The type of the Left value.</typeparam>
/// <typeparam name="TMiddle">The type of the Middle value.</typeparam>
/// <typeparam name="TRight">The type of the Right value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<TLeft, TMiddle, TRight> : IEquatable<Either<TLeft, TMiddle, TRight>>, ISerializable
    where TLeft : notnull
    where TMiddle : notnull
    where TRight : notnull
{
}

/// <summary>
/// Represents an either type with four possible values.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
/// <typeparam name="T3">The type of the third value.</typeparam>
/// <typeparam name="T4">The type of the fourth value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4> : IEquatable<Either<T1, T2, T3, T4>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
{
}

/// <summary>
/// Represents an either type with five possible values.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
/// <typeparam name="T3">The type of the third value.</typeparam>
/// <typeparam name="T4">The type of the fourth value.</typeparam>
/// <typeparam name="T5">The type of the fifth value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4, T5> : IEquatable<Either<T1, T2, T3, T4, T5>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
{
}

/// <summary>
/// Represents an either type with six possible values.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
/// <typeparam name="T3">The type of the third value.</typeparam>
/// <typeparam name="T4">The type of the fourth value.</typeparam>
/// <typeparam name="T5">The type of the fifth value.</typeparam>
/// <typeparam name="T6">The type of the sixth value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6> : IEquatable<Either<T1, T2, T3, T4, T5, T6>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
{
}

/// <summary>
/// Represents an either type with seven possible values.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
/// <typeparam name="T3">The type of the third value.</typeparam>
/// <typeparam name="T4">The type of the fourth value.</typeparam>
/// <typeparam name="T5">The type of the fifth value.</typeparam>
/// <typeparam name="T6">The type of the sixth value.</typeparam>
/// <typeparam name="T7">The type of the seventh value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7> : IEquatable<Either<T1, T2, T3, T4, T5, T6, T7>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
    where T7 : notnull
{
}

/// <summary>
/// Represents an either type with eight possible values.
/// </summary>
/// <typeparam name="T1">The type of the first value.</typeparam>
/// <typeparam name="T2">The type of the second value.</typeparam>
/// <typeparam name="T3">The type of the third value.</typeparam>
/// <typeparam name="T4">The type of the fourth value.</typeparam>
/// <typeparam name="T5">The type of the fifth value.</typeparam>
/// <typeparam name="T6">The type of the sixth value.</typeparam>
/// <typeparam name="T7">The type of the seventh value.</typeparam>
/// <typeparam name="T8">The type of the eight value.</typeparam>
[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7, T8> : IEquatable<Either<T1, T2, T3, T4, T5, T6, T7, T8>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
    where T7 : notnull
    where T8 : notnull
{
}

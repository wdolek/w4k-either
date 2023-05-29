using System;
using System.Runtime.Serialization;

namespace W4k.Either;

[Either]
[Serializable]
public readonly partial struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, ISerializable
    where TLeft : notnull
    where TRight : notnull
{
    public bool IsLeft => _idx == 1;
    public bool IsRight => _idx == 2;
}

[Either]
[Serializable]
public readonly partial struct Either<TLeft, TMiddle, TRight> : IEquatable<Either<TLeft, TMiddle, TRight>>, ISerializable
    where TLeft : notnull
    where TMiddle : notnull
    where TRight : notnull
{
}

[Either]
[Serializable]
public readonly partial struct Either<T1, T2, T3, T4> : IEquatable<Either<T1, T2, T3, T4>>, ISerializable
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
{
}

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

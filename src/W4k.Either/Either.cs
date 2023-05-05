namespace W4k.Either;

[Either]
public readonly partial struct Either<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull
{
    public bool IsLeft => _idx == 1;
    public bool IsRight => _idx == 2;
}

[Either]
public readonly partial struct Either<TLeft, TMiddle, TRight>
    where TLeft : notnull
    where TMiddle : notnull
    where TRight : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
    where T6 : notnull
{
}

[Either]
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7>
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
public readonly partial struct Either<T1, T2, T3, T4, T5, T6, T7, T8>
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

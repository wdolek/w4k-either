namespace Either.CodeGeneration.GenericUsage;

[Either]
public readonly partial struct UnconstrainedEither<TLeft, TRight>
{
    public byte State => _idx;
}

[Either]
public readonly partial struct NotNullEither<TLeft, TRight>
    where TLeft : notnull
{
    public byte State => _idx;
}

[Either]
public readonly partial struct NotNullRefEither<TLeft, TRight>
    where TLeft : class
{
    public byte State => _idx;
}

[Either]
public readonly partial struct NullableRefEither<TLeft, TRight>
    where TLeft : class?
{
    public byte State => _idx;
}

[Either]
public readonly partial struct ValueEither<TLeft, TRight>
    where TLeft : struct
{
    public byte State => _idx;
}

#nullable disable

[Either]
public readonly partial struct NullableDisabledRefEither<TLeft, TRight>
    where TLeft : class
{
    public byte State => _idx;
}

#nullable restore
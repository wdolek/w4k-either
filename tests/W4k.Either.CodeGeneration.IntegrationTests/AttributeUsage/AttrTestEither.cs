namespace W4k.Either.CodeGeneration.AttributeUsage;

[Either(typeof(Scrooge), typeof(Duckula), typeof(Nanny?))]
public readonly partial struct AttrTestEither
{
    public byte State => _idx;
}

#nullable disable

[Either(typeof(Scrooge), typeof(Duckula), typeof(Nanny?))]
public readonly partial struct NullableDisabledAttrTestEither
{
    public byte State => _idx;
}

#nullable restore
namespace W4k.Either.CodeGeneration.IntegrationTests.AttributeUsage;

public class InstantiationTests
{
    [Fact]
    public void DisallowNullWhenNullableEnabledForReferenceType()
    {
        Assert.Throws<ArgumentNullException>(() => new AttrTestEither((Scrooge)null!));
    }
    
    [Fact]
    public void AllowNullWhenNullableDisabledForReferenceType()
    {
        var either = new NullableDisabledAttrTestEither((Scrooge?)null);
        Assert.Equal(1, either.State);
    }

    [Fact]
    public void AllowNullForNullableStruct()
    {
        var either = new AttrTestEither((Nanny?)null);
        Assert.Equal(3, either.State);
    }
}

namespace Either.CodeGeneration.GenericUsage;

public class InstantiationTests
{
    [Fact]
    public void AllowNullForUnconstrained()
    {
        var either = new UnconstrainedEither<Scrooge, ValueTuple>(null);
        Assert.Equal(1, either.State);
    }

    [Fact]
    public void AllowNullForNullableRefConstraint()
    {
        var either = new NullableRefEither<Scrooge, ValueTuple>(null);
        Assert.Equal(1, either.State);
    }

    [Fact]
    public void DisallowNullForNotNullConstraint()
    {
        Assert.Throws<ArgumentNullException>(() => new NotNullEither<Scrooge, ValueTuple>(null!));
    }

    [Fact]
    public void DisallowNullForNotNullRefConstraint()
    {
        Assert.Throws<ArgumentNullException>(() => new NotNullRefEither<Scrooge, ValueTuple>(null!));
    }

    [Fact]
    public void AllowNullWhenNullableDisabled()
    {
        var either = new NullableDisabledRefEither<Scrooge, ValueTuple>(null);
        Assert.Equal(1, either.State);
    }
}
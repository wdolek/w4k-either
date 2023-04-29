using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.GenericUsage;

public class InstantiationTests
{
    [Fact]
    public void AllowNullForUnconstrained()
    {
        var either = new UnconstrainedEither<Scrooge, Unit>(null);
        Assert.Equal(1, either.State);
    }
    
    [Fact]
    public void AllowNullForNullableRefConstraint()
    {
        var either = new NullableRefEither<Scrooge, Unit>(null);
        Assert.Equal(1, either.State);
    }

    [Fact]
    public void DisallowNullForNotNullConstraint()
    {
        Assert.Throws<ArgumentNullException>(() => new NotNullEither<Scrooge, Unit>(null!));
    }

    [Fact]
    public void DisallowNullForNotNullRefConstraint()
    {
        Assert.Throws<ArgumentNullException>(() => new NotNullRefEither<Scrooge, Unit>(null!));
    }

    [Fact]
    public void AllowNullWhenNullableDisabled()
    {
        var either = new NullableDisabledRefEither<Scrooge, Unit>(null);
        Assert.Equal(1, either.State);
    }
}

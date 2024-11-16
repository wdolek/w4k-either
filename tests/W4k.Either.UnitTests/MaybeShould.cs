namespace W4k.Either;

public class MaybeShould
{
    [Fact]
    public void BeCreatedWithSome()
    {
        var maybe = Maybe.Some(42);

        Assert.True(maybe.HasValue);
        Assert.Equal(42, maybe.Value);
    }

    [Fact]
    public void BeCreatedWithNone()
    {
        var maybe = Maybe.None<int>();

        Assert.False(maybe.HasValue);
        Assert.Equal(default, maybe.Value);
    }
}
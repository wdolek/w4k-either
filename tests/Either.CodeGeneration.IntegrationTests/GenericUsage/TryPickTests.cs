namespace Either.CodeGeneration.GenericUsage;

public class TryPickTests
{
    [Fact]
    public void ShouldPickCurrentState()
    {
        var scrooge = new UnconstrainedEither<Scrooge?, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));
        var @null = new UnconstrainedEither<Scrooge?, Duckula>(null);
        var duckula = new UnconstrainedEither<Scrooge?, Duckula>(new Duckula(IsKetchupLover: true));

        // current state, value is set
        Assert.True(scrooge.TryPick(out Scrooge? scroogeValue));
        Assert.NotNull(scroogeValue);

        // current state, value is null
        Assert.True(@null.TryPick(out Scrooge? nullValue));
        Assert.Null(nullValue);

        // current state, value is present
        Assert.True(duckula.TryPick(out Duckula duckulaValue));
        Assert.True(duckulaValue.IsKetchupLover);
    }

    [Fact]
    public void ShouldPickWithCorrectRemainder()
    {
        var scrooge = new UnconstrainedEither<Scrooge, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));
        var duckula = new UnconstrainedEither<Scrooge, Duckula>(new Duckula(IsKetchupLover: true));

        // Scrooge, trying to pick Scrooge
        Assert.True(scrooge.TryPick(out Scrooge? scroogeValue, out var duckulaValue));
        Assert.NotNull(scroogeValue);
        Assert.False(duckulaValue.IsKetchupLover);

        // Scrooge, trying to pick Duckula
        Assert.False(scrooge.TryPick(out duckulaValue, out scroogeValue));
        Assert.NotNull(scroogeValue);
        Assert.False(duckulaValue.IsKetchupLover);

        // Duckula, trying to pick Duckula
        Assert.True(duckula.TryPick(out duckulaValue, out scroogeValue));
        Assert.Null(scroogeValue);
        Assert.True(duckulaValue.IsKetchupLover);

        // Duckula, trying to pick Scrooge
        Assert.False(duckula.TryPick(out scroogeValue, out duckulaValue));
        Assert.Null(scroogeValue);
        Assert.True(duckulaValue.IsKetchupLover);
    }

    [Fact]
    public void ShouldNotPickDifferentState()
    {
        var scrooge = new UnconstrainedEither<Scrooge?, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));
        Assert.False(scrooge.TryPick(out Duckula _));
    }
}
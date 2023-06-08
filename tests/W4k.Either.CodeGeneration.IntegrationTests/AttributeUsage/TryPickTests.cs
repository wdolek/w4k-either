namespace W4k.Either.CodeGeneration.AttributeUsage;

public class TryPickTests
{
    [Fact]
    public void ShouldPickCurrentState()
    {
        var scrooge = new AttrTestEither(new Scrooge(Money: 315_360_000_000_000_000));
        var duckula = new AttrTestEither(new Duckula(IsKetchupLover: true));
        var nanny = new AttrTestEither((Nanny?)null);

        // current state, value is set
        Assert.True((bool)scrooge.TryPick(out Scrooge? scroogeValue));
        Assert.NotNull(scroogeValue);

        // current state, value is present
        Assert.True((bool)duckula.TryPick(out Duckula duckulaValue));
        Assert.True(duckulaValue.IsKetchupLover);

        // current state, value is null
        Assert.True((bool)nanny.TryPick(out Nanny? nannyValue));
        Assert.False(nannyValue.HasValue);
    }
    
    [Fact]
    public void ShouldPickCurrentStateWhenNullableDisabled()
    {
        var scrooge = new NullableDisabledAttrTestEither((Scrooge)null!);

        // current state, value is not set
        Assert.True((bool)scrooge.TryPick(out Scrooge? scroogeValue));
        Assert.Null(scroogeValue);
    }    
}

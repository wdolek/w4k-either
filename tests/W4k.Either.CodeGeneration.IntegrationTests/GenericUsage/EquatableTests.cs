namespace W4k.Either.CodeGeneration.GenericUsage;

public class EquatableTests
{
    [Fact]
    public void ShouldEqualForSameInnerState()
    {
        var first = new NotNullEither<Scrooge, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));
        var second = new NotNullEither<Scrooge, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));
        var third = new NotNullEither<Scrooge, Duckula>(new Duckula(IsKetchupLover: true));
        
        Assert.True(first == second);
        Assert.True(first.Equals((object?)second));

        Assert.False(first == third);
        Assert.False(first.Equals((object?)third));        
    }
    
    [Fact]
    public void ShouldEqualForInnerNull()
    {
        var first = new UnconstrainedEither<Scrooge, Duckula>(null);
        var second = new UnconstrainedEither<Scrooge, Duckula>(null);
        var third = new UnconstrainedEither<Scrooge, Duckula>(new Duckula(IsKetchupLover: true));
        
        Assert.True(first == second);
        Assert.True(first.Equals((object?)second));

        Assert.False(first == third);
        Assert.False(first.Equals((object?)third));        
    }    
}

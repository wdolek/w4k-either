namespace W4k.Either.CodeGeneration.GenericUsage;

public class PatternMatchingTests
{
    [Fact]
    public void CaseShouldReturnCorrectState()
    {
        var either = new NotNullEither<Scrooge, Duckula>(new Scrooge(Money: 315_360_000_000_000_000));

        var scrooge = either.Case switch
        {
            Scrooge v => v,
            _ => null,
        };
        
        Assert.NotNull(scrooge);
    }
    
    [Fact]
    public void CaseShouldReturnCorrectStateForValueType()
    {
        var either = new NotNullEither<Scrooge, Duckula>(new Duckula(IsKetchupLover: true));

        var duckula = either.Case switch
        {
            Duckula d => d,
            _ => default,
        };
        
        Assert.True(duckula.IsKetchupLover);
    }    
}

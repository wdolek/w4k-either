namespace W4k.Either.CodeGeneration.GenericUsage;

public class MatchTests
{
    [Fact]
    public void MatchCurrentState()
    {
        UnconstrainedEither<Scrooge, Unit> scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        // simple match
        var isHuey = scrooge.Match(
            _ => true,
            _ => false);

        Assert.True((bool)isHuey);

        // match with state
        var hueyMatch = scrooge.Match(
            state: 41,
            (state, _) => state + 1,
            (state, _) => state + 0);

        Assert.Equal(42, hueyMatch);
    }

    [Fact]
    public async Task MatchCurrentStateAsync()
    {
        UnconstrainedEither<Scrooge, Unit> scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        // async match
        var isHueyAsync = await scrooge.MatchAsync(
            (_, _) => Task.FromResult(true),
            (_, _) => Task.FromResult(false));

        Assert.True((bool)isHueyAsync);

        // async match with state
        var hueyMatchAsync = await scrooge.MatchAsync(
            state: 41,
            (state, _, _) => Task.FromResult(state + 1),
            (state, _, _) => Task.FromResult(state + 0));

        Assert.Equal(42, hueyMatchAsync);
    }
}

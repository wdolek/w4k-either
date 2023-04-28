using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.Generics;

public class InvalidStateTests
{
    [Fact]
    public void AllowInstantiateDefault()
    {
        var eitherCtor = new UnconstrainedEither<Scrooge, Unit>();
        Assert.Equal(0, eitherCtor.State);
        
        UnconstrainedEither<Scrooge, Unit> eitherDefault = default;
        Assert.Equal(0, eitherDefault.State);
    }

    [Fact]
    public void ShouldBeEquatable()
    {
        var valid = new UnconstrainedEither<Scrooge, Unit>(new Scrooge(Money: 315_360_000_000_000_000));

        var invalid1 = new UnconstrainedEither<Scrooge, Unit>();
        var invalid2 = new UnconstrainedEither<Scrooge, Unit>();
        
        Assert.False((bool)(valid == invalid1));
        Assert.False(valid.Equals((object?)invalid1));

        Assert.False((bool)(invalid1 == valid));
        Assert.False(invalid1.Equals((object?)valid));
        
        Assert.Throws<InvalidOperationException>(() => invalid1 == invalid2);
    }

    [Fact]
    public void DisallowPatternMatching()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();
        Assert.Throws<InvalidOperationException>(() => invalid.Case);
    }

    [Fact]
    public void DisallowGettingHashCode()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();
        Assert.Throws<InvalidOperationException>(() => invalid.GetHashCode());
    }
    
    [Fact]
    public void DisallowToString()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();
        Assert.Throws<InvalidOperationException>(() => invalid.ToString());
    }
    
    [Fact]
    public void DisAllowTryPick()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();
        Assert.False((bool)invalid.TryPick(out Scrooge _));
    }

    [Fact]
    public async Task DisallowMatch()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();

        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                _ => Unit.Default,
                _ => Unit.Default));

        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                -1,
                (_, _) => Unit.Default,
                (_, _) => Unit.Default));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                (_, _) => Task.FromResult(Unit.Default),
                (_, _) => Task.FromResult(Unit.Default)));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                -1,
                (_, _, _) => Task.FromResult(Unit.Default),
                (_, _, _) => Task.FromResult(Unit.Default)));
    }
    
    [Fact]
    public async Task DisallowSwitch()
    {
        var invalid = new UnconstrainedEither<Scrooge, Unit>();

        Assert.Throws<InvalidOperationException>(
            () => invalid.Switch(
                _ => { },
                _ => { }));

        Assert.Throws<InvalidOperationException>(
            () => invalid.Switch(
                -1,
                (_, _) => { },
                (_, _) => { }));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.SwitchAsync(
                (_, _) => Task.CompletedTask,
                (_, _) => Task.CompletedTask));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.SwitchAsync(
                -1,
                (_, _, _) => Task.CompletedTask,
                (_, _, _) => Task.CompletedTask));
    }    
}

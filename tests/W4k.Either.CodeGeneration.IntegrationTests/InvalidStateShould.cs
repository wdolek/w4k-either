using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests;

[Either]
public readonly partial struct InvalidEither<TLeft, TRight>
{
    public byte State => _idx;
}

public class InvalidStateShould
{
    [Fact]
    public void BePossibleToCreate()
    {
        var eitherCtor = new InvalidEither<Scrooge, Duckula>();
        Assert.Equal(0, eitherCtor.State);
        
        InvalidEither<Scrooge, Duckula> eitherDefault = default;
        Assert.Equal(0, eitherDefault.State);
    }

    [Fact]
    public void BeEquatable()
    {
        var valid = new InvalidEither<Scrooge, Duckula>(new Duckula(IsKetchupLover: true));

        var invalid1 = new InvalidEither<Scrooge, Duckula>();
        var invalid2 = new InvalidEither<Scrooge, Duckula>();
        
        Assert.False(valid == invalid1);
        Assert.False(valid.Equals((object?)invalid1));

        Assert.False(invalid1 == valid);
        Assert.False(invalid1.Equals((object?)valid));
        
        Assert.Throws<InvalidOperationException>(() => invalid1 == invalid2);
    }

    [Fact]
    public void NotBePossibleToUseForPatternMatching()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();
        Assert.Throws<InvalidOperationException>(() => invalid.Case);
    }

    [Fact]
    public void ForbidGetHashCode()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();
        Assert.Throws<InvalidOperationException>(() => invalid.GetHashCode());
    }
    
    [Fact]
    public void ForbidToString()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();
        Assert.Throws<InvalidOperationException>(() => invalid.ToString());
    }
    
    [Fact]
    public void NotThrowOnTryPick()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();
        Assert.False(invalid.TryPick(out Scrooge _));
    }

    [Fact]
    public async Task ForbidMatch()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();

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
    public async Task ForbidSwitch()
    {
        var invalid = new InvalidEither<Scrooge, Duckula>();

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

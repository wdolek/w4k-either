namespace W4k.Either.CodeGeneration.GenericUsage;

public class InvalidStateTests
{
    [Fact]
    public void AllowInstantiateDefault()
    {
        var eitherCtor = new UnconstrainedEither<Scrooge, ValueTuple>();
        Assert.Equal(0, eitherCtor.State);

        UnconstrainedEither<Scrooge, ValueTuple> eitherDefault = default;
        Assert.Equal(0, eitherDefault.State);
    }

    [Fact]
    public void ShouldBeEquatable()
    {
        var valid = new UnconstrainedEither<Scrooge, ValueTuple>(new Scrooge(Money: 315_360_000_000_000_000));

        var invalid1 = new UnconstrainedEither<Scrooge, ValueTuple>();
        var invalid2 = new UnconstrainedEither<Scrooge, ValueTuple>();

        Assert.False(valid == invalid1);
        Assert.False(valid.Equals((object?)invalid1));

        Assert.False(invalid1 == valid);
        Assert.False(invalid1.Equals((object?)valid));

        Assert.Throws<InvalidOperationException>(() => invalid1 == invalid2);
    }

    [Fact]
    public void DisallowPatternMatching()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();
        Assert.Throws<InvalidOperationException>(() => invalid.Case);
    }

    [Fact]
    public void DisallowGettingHashCode()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();
        Assert.Throws<InvalidOperationException>(() => invalid.GetHashCode());
    }

    [Fact]
    public void DisallowToString()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();
        Assert.Throws<InvalidOperationException>(() => invalid.ToString());
    }

    [Fact]
    public void DisAllowTryPick()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();
        Assert.False(invalid.TryPick(out Scrooge? _));
    }

    [Fact]
    public async Task DisallowMatch()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();

        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                _ => ValueTuple.Create(),
                _ => ValueTuple.Create()));

        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                -1,
                (_, _) => ValueTuple.Create(),
                (_, _) => ValueTuple.Create()));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                (_, _) => Task.FromResult(ValueTuple.Create()),
                (_, _) => Task.FromResult(ValueTuple.Create())));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                -1,
                (_, _, _) => Task.FromResult(ValueTuple.Create()),
                (_, _, _) => Task.FromResult(ValueTuple.Create())));
    }

    [Fact]
    public async Task DisallowSwitch()
    {
        var invalid = new UnconstrainedEither<Scrooge, ValueTuple>();

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
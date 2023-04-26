#nullable disable

using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.NullableDisabled;

[Either]
public readonly partial struct NullRefDisabledTestEither<TRef, TNotNull, TValue, TAny>
    where TRef : class
    where TNotNull : notnull
    where TValue : struct
{
    public byte State => _idx;
    public TRef T1 => _v1;
    public TNotNull T2 => _v2;
    public TValue T3 => _v3;
    public TAny T4 => _v4;
}

public class NullRefDisabledEitherShould
{
        [Fact]
    public void InstantiateInvalidStateUsingDefaultCtor()
    {
        var either = new NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge>();
        Assert.Equal(0, either.State);
        Assert.Throws<InvalidOperationException>(() => either.Case);
    }
    
    [Fact]
    public void PreventNullForNonNullableArgs()
    {
        // non-nullable ref type
        Assert.Throws<ArgumentNullException>(() => new NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge>((Dewey)null));
    }

    [Fact]
    public void AllowNullForNullableArgs()
    {
        // class? constraint
        _ = new NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge>((Huey)null);

        // unconstrained
        _ = new NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge>((Scrooge)null);
    }

    [Fact]
    public void ReturnCorrectValueUsingPatternMatching()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> either = new Dewey(HasBlueHat: true);

        var dewey = either.Case switch
        {
            Dewey v => v,
            _ => null,
        };
        
        Assert.NotNull(dewey);
    }

    [Fact]
    public void CompareInnerState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> first = new Huey(HasRedHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> second = new Huey(HasRedHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> third = new Dewey(HasBlueHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> invalid = default;

        Assert.True(first == second);
        Assert.True(first.Equals((object)second));

        Assert.False(first == third);
        Assert.False(first.Equals((object)third));
        
        Assert.False(first == invalid);
        Assert.False(first.Equals((object)invalid));
    }

    [Fact]
    public void GetHashCodeOfInnerState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> huey = (Huey)null;
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> dewey = new Dewey(HasBlueHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> invalid = default;

        Assert.Equal(0, huey.GetHashCode());
        Assert.Equal(dewey.T2!.GetHashCode(), dewey.GetHashCode());
        Assert.Equal(scrooge.T4!.GetHashCode(), scrooge.GetHashCode());
        Assert.Throws<InvalidOperationException>(() => invalid.GetHashCode());
    }

    [Fact]
    public void GetStringFromInnerState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> huey = (Huey)null;
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> dewey = new Dewey(HasBlueHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> invalid = default;

        // `huey` is null
        Assert.Equal("", huey.ToString());

        Assert.Equal("Dewey { HasBlueHat = True }", dewey.ToString());

        // NB! `Scrooge` type is record and thus all that fluff around
        Assert.Equal("Scrooge { Money = 315360000000000000 }", scrooge.ToString());
        
        Assert.Throws<InvalidOperationException>(() => invalid.ToString());
    }

    [Fact]
    public void TryPickAccordingState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> huey = (Huey)null;
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> dewey = new Dewey(HasBlueHat: true);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> invalid = default;

        Assert.False(huey.TryPick(out Dewey _));

        Assert.False(dewey.TryPick(out Scrooge _));
        Assert.True(dewey.TryPick(out Dewey deweyValue));
        Assert.NotNull(deweyValue);
        Assert.True(deweyValue.HasBlueHat);

        Assert.True(scrooge.TryPick(out Scrooge scroogeValue));
        Assert.NotNull(scroogeValue);

        Assert.False(invalid.TryPick(out Scrooge _));
    }

    [Fact]
    public async Task MatchAccordingState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> huey = new Huey(HasRedHat: true);

        // simple match
        var isHuey = huey.Match(
            _ => true,
            _ => false,
            _ => false,
            _ => false);

        Assert.True(isHuey);

        // match with state
        var hueyMatch = huey.Match(
            state: 41,
            (state, _) => state + 1,
            (state, _) => state + 0,
            (state, _) => state + 0,
            (state, _) => state + 0);

        Assert.Equal(42, hueyMatch);

        // async match
        var isHueyAsync = await huey.MatchAsync(
            (_, _) => Task.FromResult(true),
            (_, _) => Task.FromResult(false),
            (_, _) => Task.FromResult(false),
            (_, _) => Task.FromResult(false));

        Assert.True(isHueyAsync);

        // async match with state
        var hueyMatchAsync = await huey.MatchAsync(
            state: 41,
            (state, _, _) => Task.FromResult(state + 1),
            (state, _, _) => Task.FromResult(state + 0),
            (state, _, _) => Task.FromResult(state + 0),
            (state, _, _) => Task.FromResult(state + 0));

        Assert.Equal(42, hueyMatchAsync);
    }
    
    [Fact]
    public async Task NotMatchInvalidState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> invalid = default;

        // match on invalid state
        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                _ => false,
                _ => false,
                _ => false,
                _ => false));

        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                state: 41,
                (state, _) => state + 0,
                (state, _) => state + 0,
                (state, _) => state + 0,
                (state, _) => state + 0));
        
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                (_, _) => Task.FromResult(false),
                (_, _) => Task.FromResult(false),
                (_, _) => Task.FromResult(false),
                (_, _) => Task.FromResult(false)));
        
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                state: 41,
                (state, _, _) => Task.FromResult(state + 0),
                (state, _, _) => Task.FromResult(state + 0),
                (state, _, _) => Task.FromResult(state + 0),
                (state, _, _) => Task.FromResult(state + 0)));
    }
    
    [Fact]
    public async Task SwitchAccordingState()
    {
        NullRefDisabledTestEither<Huey, Dewey, Duckula, Scrooge> huey = new Huey(HasRedHat: true);

        // simple switch
        huey.Switch(
            _ => Assert.True(true),
            _ => Assert.Fail("Should not be called"),
            _ => Assert.Fail("Should not be called"),
            _ => Assert.Fail("Should not be called"));

        // switch with state
        huey.Switch(
            state: 42,
            (state, _) => Assert.Equal(42, state),
            (_, _) => Assert.Fail("Should not be called"),
            (_, _) => Assert.Fail("Should not be called"),
            (_, _) => Assert.Fail("Should not be called"));

        // async switch
        await huey.SwitchAsync(
            (_, _) => Task.CompletedTask,
            (_, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            },
            (_, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            },
            (_, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            });

        // async switch with state
        await huey.SwitchAsync(
            state: 42,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            (state, _, _) =>
            {
                Assert.Equal(42, state);
                return Task.CompletedTask;
            },
            (_, _, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            },
            (_, _, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            },
            (_, _, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            });
    }
}

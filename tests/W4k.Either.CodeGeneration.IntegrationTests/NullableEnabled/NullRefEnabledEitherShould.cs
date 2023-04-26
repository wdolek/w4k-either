using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.NullableEnabled;

[Either]
public readonly partial struct NullRefEnabledTestEither<TNonNullableRef, TNullableRef, TNotNull, TValue, TAny>
    where TNonNullableRef : class
    where TNullableRef : class?
    where TNotNull : notnull
    where TValue : struct
{
    public byte State => _idx;
    public TNonNullableRef? T1 => _v1;
    public TNullableRef? T2 => _v2;
    public TNotNull? T3 => _v3;
    public TValue T4 => _v4;
    public TAny? T5 => _v5;
}

public class NullRefEnabledEitherShould
{
    [Fact]
    public void InstantiateInvalidStateUsingDefaultCtor()
    {
        var either = new NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?>();
        Assert.Equal(0, either.State);
        Assert.Throws<InvalidOperationException>(() => either.Case);
    }
    
    [Fact]
    public void PreventNullForNonNullableArgs()
    {
        // non-nullable ref type
        Assert.Throws<ArgumentNullException>(() => new NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?>((Huey)null!));

        // notnull constraint
        Assert.Throws<ArgumentNullException>(() => new NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?>((Louie)null!));
    }

    [Fact]
    public void AllowNullForNullableArgs()
    {
        // class? constraint
        _ = new NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?>((Dewey?)null);

        // unconstrained
        _ = new NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?>((Scrooge?)null);
    }

    [Fact]
    public void ReturnCorrectValueUsingPatternMatching()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> either = new Louie(HasGreenHat: true);

        var louie = either.Case switch
        {
            Louie v => v,
            _ => null,
        };
        
        Assert.NotNull(louie);
    }

    [Fact]
    public void CompareInnerState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> first = new Louie(HasGreenHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> second = new Louie(HasGreenHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> third = new Huey(HasRedHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> invalid = default;

        Assert.True(first == second);
        Assert.True(first.Equals((object?)second));

        Assert.False(first == third);
        Assert.False(first.Equals((object?)third));
        
        Assert.False(first == invalid);
        Assert.False(first.Equals((object?)invalid));
    }

    [Fact]
    public void GetHashCodeOfInnerState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> huey = new Huey(HasRedHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> dewey = (Dewey?)null;
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> invalid = default;

        Assert.Equal(huey.T1!.GetHashCode(), huey.GetHashCode());
        Assert.Equal(0, dewey.GetHashCode());
        Assert.Equal(scrooge.T5!.GetHashCode(), scrooge.GetHashCode());
        Assert.Throws<InvalidOperationException>(() => invalid.GetHashCode());
    }

    [Fact]
    public void GetStringFromInnerState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> huey = new Huey(HasRedHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> dewey = (Dewey?)null;
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> invalid = default;

        Assert.Equal("Huey { HasRedHat = True }", huey.ToString());

        // `dewey` is null
        Assert.Equal("", dewey.ToString());
        
        // NB! `Scrooge` type is record and thus all that fluff around
        Assert.Equal("Scrooge { Money = 315360000000000000 }", scrooge.ToString());
        
        Assert.Throws<InvalidOperationException>(() => invalid.ToString());
    }

    [Fact]
    public void TryPickAccordingState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> huey = new Huey(HasRedHat: true);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> dewey = (Dewey?)null;
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> invalid = default;

        Assert.False(huey.TryPick(out Scrooge? _));
        Assert.True(huey.TryPick(out Huey? hueyValue));
        Assert.NotNull(hueyValue);
        Assert.True(hueyValue.HasRedHat);
        
        Assert.True(dewey.TryPick(out Dewey? deweyValue));
        Assert.Null(deweyValue);
        
        Assert.True(scrooge.TryPick(out Scrooge? scroogeValue));
        Assert.NotNull(scroogeValue);

        Assert.False(invalid.TryPick(out Scrooge? _));
    }

    [Fact]
    public async Task MatchAccordingState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> huey = new Huey(HasRedHat: true);

        // simple match
        var isHuey = huey.Match(
            _ => true,
            _ => false,
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
            (state, _) => state + 0,
            (state, _) => state + 0);

        Assert.Equal(42, hueyMatch);

        // async match
        var isHueyAsync = await huey.MatchAsync(
            (_, _) => Task.FromResult(true),
            (_, _) => Task.FromResult(false),
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
            (state, _, _) => Task.FromResult(state + 0),
            (state, _, _) => Task.FromResult(state + 0));

        Assert.Equal(42, hueyMatchAsync);
    }
    
    [Fact]
    public async Task NotMatchInvalidState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> invalid = default;

        // match on invalid state
        Assert.Throws<InvalidOperationException>(
            () => invalid.Match(
                _ => false,
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
                (state, _) => state + 0,
                (state, _) => state + 0));
        
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => invalid.MatchAsync(
                (_, _) => Task.FromResult(false),
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
                (state, _, _) => Task.FromResult(state + 0),
                (state, _, _) => Task.FromResult(state + 0)));
    }
    
    [Fact]
    public async Task SwitchAccordingState()
    {
        NullRefEnabledTestEither<Huey, Dewey?, Louie, Duckula, Scrooge?> huey = new Huey(HasRedHat: true);

        // simple switch
        huey.Switch(
            _ => Assert.True(true),
            _ => Assert.Fail("Should not be called"),
            _ => Assert.Fail("Should not be called"),
            _ => Assert.Fail("Should not be called"),
            _ => Assert.Fail("Should not be called"));

        // switch with state
        huey.Switch(
            state: 42,
            (state, _) => Assert.Equal(42, state),
            (_, _) => Assert.Fail("Should not be called"),
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
            },
            (_, _, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            });
    }    
}

using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.GenericUsage;

public class SwitchTests
{
    [Fact]
    public void SwitchAccordingState()
    {
        NotNullEither<Scrooge, Unit> huey = new Scrooge(Money: 315_360_000_000_000_000);

        // simple switch
        huey.Switch(
            _ => Assert.True(true),
            _ => Assert.Fail("Should not be called"));

        // switch with state
        huey.Switch(
            state: 42,
            (state, _) => Assert.Equal(42, state),
            (_, _) => Assert.Fail("Should not be called"));
    }
    
    [Fact]
    public async Task SwitchAccordingStateAsync()
    {
        NotNullEither<Scrooge, Unit> scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        // async switch
        await scrooge.SwitchAsync(
            (_, _) => Task.CompletedTask,
            (_, _) =>
            {
                Assert.Fail("Should not be called");
                return Task.CompletedTask;
            });

        // async switch with state
        await scrooge.SwitchAsync(
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
            });
    }
}

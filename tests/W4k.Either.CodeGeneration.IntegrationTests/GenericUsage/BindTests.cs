namespace W4k.Either.CodeGeneration.GenericUsage;

public class BindTests
{
    [Fact]
    public void ShouldComputeCorrectResult()
    {
        // arrange
        UnconstrainedEither<string, int> either = 10;

        // act
        var result = either.Bind(i => Computation(i));

        // assert
        Assert.Equal(2, result.State);
        Assert.Equal(3.1622776601683795, (double)result.Case!, 10);
    }

    [Fact]
    public void Bind_WithNonPositiveValue_ReturnsErrorMessage()
    {
        // arrange
        UnconstrainedEither<string, int> either = -5;

        // act
        var result = either.Bind(i => Computation(i));

        // assert
        Assert.Equal(1, result.State);
        Assert.Equal("Invalid input", result.Case);
    }

    private static UnconstrainedEither<string?, double> Computation(int value)
    {
        if (value > 0)
        {
            var result = Math.Sqrt(value);
            return result;
        }

        return "Invalid input";
    }
}

using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration.IntegrationTests.GenericUsage;

public class ObjectOverridesTests
{
    [Theory]
    [MemberData(nameof(GenerateForHashCode))]
    public void ShouldReturnHashCodeOfInnerValueOrZero(UnconstrainedEither<Scrooge?, Unit> either, int expected)
    {
        Assert.Equal(expected, either.GetHashCode());
    }

    [Theory]
    [MemberData(nameof(GenerateForToString))]
    public void ShouldReturnStringRepresentationOfInnerValueOrEmptyString(UnconstrainedEither<Scrooge?, Unit> either, string expected)
    {
        Assert.Equal(expected, either.ToString());
    }

    public static TheoryData<UnconstrainedEither<Scrooge?, Unit>, int> GenerateForHashCode()
    {
        var scrooge = new Scrooge(Money: 315_360_000_000_000_000);
        
        return new()
        {
            { (Scrooge?)null, 0 },
            { new Scrooge(Money: 315_360_000_000_000_000), scrooge.GetHashCode() },
        };
    }

    public static TheoryData<UnconstrainedEither<Scrooge?, Unit>, string> GenerateForToString()
    {
        var scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        return new()
        {
            { (Scrooge?)null, "" },
            { new Scrooge(Money: 315_360_000_000_000_000), scrooge.ToString() },
        };
    }
}

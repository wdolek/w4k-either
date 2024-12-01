namespace Either.CodeGeneration.GenericUsage;

public class MapTests
{
    [Fact]
    public void MapToAnotherType()
    {
        NotNullEither<Scrooge, ValueTuple> scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        // Scrooge -> Huey
        var huey = scrooge.Map<Huey>(s => new Huey(s));

        Assert.IsType<NotNullEither<Huey, ValueTuple>>(huey);
        Assert.IsType<Huey>(huey.Case);
    }

    private record Huey(Scrooge Uncle);
}
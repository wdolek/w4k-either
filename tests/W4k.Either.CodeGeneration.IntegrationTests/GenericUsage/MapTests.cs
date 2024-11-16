namespace W4k.Either.CodeGeneration.GenericUsage;

public class MapTests
{
    [Fact]
    public void MapToAnotherType()
    {
        NotNullEither<Scrooge, Unit> scrooge = new Scrooge(Money: 315_360_000_000_000_000);

        // Scrooge -> Huey
        var huey = scrooge.Map<Huey>(s => new Huey(s));

        Assert.IsType<NotNullEither<Huey, Unit>>(huey);
        Assert.IsType<Huey>(huey.Case);
    }

    private record Huey(Scrooge Uncle);
}
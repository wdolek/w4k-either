namespace Either;

public class EitherShould
{
    [Fact]
    public void BeLeft()
    {
        Either<int, string> either = 42;
        Assert.True(either.IsLeft);
    }

    [Fact]
    public void BeRight()
    {
        Either<int, string> either = "foo";
        Assert.True(either.IsRight);
    }
}
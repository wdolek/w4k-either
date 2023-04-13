namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class EitherSourceGeneratorShould
{
    [Fact]
    public Task GenerateEither()
    {
        var source = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TLeft, TRight>
        where TLeft : notnull
        where TRight : notnull
    {
    }
}";

        return TestHelper.Verify(source);
    }
}

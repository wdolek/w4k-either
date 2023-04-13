namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class EitherSourceGeneratorShould
{
    [Fact]
    public Task GenerateEither2()
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
    
    [Fact]
    public Task GenerateEither3()
    {
        var source = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TLeft, TMiddle, TRight>
        where TLeft : notnull
        where TMiddle: notnull
        where TRight : notnull
    {
    }
}";

        return TestHelper.Verify(source);
    }    
}

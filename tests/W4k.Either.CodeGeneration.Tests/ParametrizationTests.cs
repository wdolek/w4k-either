namespace W4k.Either.CodeGeneration;

[UsesVerify]
public class ParametrizationTests
{
    [Fact]
    public Task GenerateUsingAttribute()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(string), typeof(int))]
    public partial struct MyStringOrInt
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    public Task GenerateUsingGenericAttribute()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either<string, int>]
    public partial struct MyStringOrInt
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    public Task GenerateUsingGenericTypeParams()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyThisOrThat<TLeft, TRight>
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}
namespace W4k.Either.CodeGeneration;

[UsesVerify]
public class SkipMemberTests
{
    [Fact]
    public Task SkipOnGenericType()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(Skip = new [] { ""Case"", ""TryPick"", ""Switch*"" })]
    public partial struct MyEither<TLeft, TRight>
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task SkipWithAttrTypeParams()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(string), typeof(int), Skip = new [] { ""SwitchAsync"", ""SwitchAsync<TState>"" })]
    public partial struct StringOrInt
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }    
    
    [Fact]
    public Task SkipWithGenericAttr()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either<string, int>(Skip = new [] { ""Bind"", ""Map"", ""MatchAsync"", ""MatchAsync<TState>"" })]
    public partial struct StringOrInt
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}

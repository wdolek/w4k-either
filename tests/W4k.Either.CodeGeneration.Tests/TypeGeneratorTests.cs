namespace W4k.Either.CodeGeneration;

[UsesVerify]
public class TypeGeneratorTests
{
    [Fact]
    public Task GenerateStruct()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyValuableEither<TLeft, TRight>
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task GenerateClass()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial class MyClassyEither<TLeft, TRight>
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    public Task GenerateNestedType()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    public partial class MyContainingType
    {
        [Either]
        private partial struct MyNestedEither<TLeft, TRight>
        {
        }
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}

namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class StructGeneratorTests
{

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

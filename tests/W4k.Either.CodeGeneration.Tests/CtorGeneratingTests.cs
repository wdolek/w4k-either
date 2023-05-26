namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class CtorGeneratingTests
{
    [Fact]
    public Task GenerateOnlyCtorNotDeclaredByUser()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TLeft, TRight>
    {
        public MyEither()
        {
            _idx = 1;
            _v1 = default;
            _v2 = default;
        }

        public MyEither(TRight value)
        {
            _idx = 2;
            _v1 = default;
            _v2 = value;
        }
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task GenerateOnlyCtorNotDeclaredByUser2()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(int?))]
    public partial struct MyEither
    {
        public MyEither(int? value)
        {
            _idx = 2;
            _v1 = default;
            _v2 = value;
        }
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }    
}

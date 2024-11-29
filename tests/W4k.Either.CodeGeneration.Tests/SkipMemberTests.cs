namespace W4k.Either.CodeGeneration;

public class SkipMemberTests
{
    [Fact]
    public Task SkipOnGenericType()
    {
        var source = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(Generate = Members.BindAll | Members.MapAll | Members.MatchAll)]
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
    [Either(typeof(string), typeof(int), Generate = Members.Case | Members.TryPick | Members.BindAll | Members.MapAll | Members.MatchAll | Members.Switch | Members.SwitchWithState)]
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
    [Either<string, int>(Generate = Members.Case | Members.TryPick | Members.BindWithState | Members.Switch | Members.SwitchWithState)]
    public partial struct StringOrInt
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}
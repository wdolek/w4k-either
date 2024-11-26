namespace W4k.Either.CodeGeneration;

public class NullabilityTests
{
    [Fact]
    [Trait("Usage", "Attribute")]
    [Trait("Nullable", "Enabled")]
    public Task GenerateUsingAttributeAndNullRefEnabled()
    {
        var source = @"
#nullable enable
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(string), typeof(int), typeof(int?))]
    public partial struct MyEither
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    [Trait("Usage", "Attribute")]
    [Trait("Nullable", "Disabled")]
    public Task GenerateUsingAttributeAndNullRefDisabled()
    {
        var source = @"
#nullable disable
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(string), typeof(int), typeof(int?))]
    public partial struct MyEither
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    [Trait("Usage", "Generics")]
    [Trait("Nullable", "Enabled")]
    public Task GenerateUsingGenericsAndNullRefEnabled()
    {
        // intentionally not setting any constraint for `TAny` -> it can be anything
        var source = @"
#nullable enable
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TAny, TNonNullRef, TNullRef, TStruct, TNotNull, TObj, TNullObj, TIFace, TNullIFace, TUnmanaged>
        where TNonNullRef : class
        where TNullRef : class?
        where TStruct : struct
        where TNotNull : notnull
        where TObj : LeObject
        where TNullObj : LeObject?
        where TIFace : IAmInterface
        where TNullIFace : IAmInterface?
        where TUnmanaged : unmanaged
    {
    }

    public class LeObject
    {
    }

    public interface IAmInterface
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    [Trait("Usage", "Generics")]
    [Trait("Nullable", "Disabled")]
    public Task GenerateUsingGenericsAndNullRefDisabled()
    {
        // intentionally not setting any constraint for `TAny` -> it can be anything
        var source = @"
#nullable disable
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TAny, TNullRef, TStruct, TNotNull, TObj, TIFace, TUnmanaged>
        where TNullRef : class
        where TStruct : struct
        where TNotNull : notnull
        where TObj : LeObject
        where TIFace : IAmInterface
        where TUnmanaged : unmanaged
    {
    }

    public class LeObject
    {
    }

    public interface IAmInterface
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}
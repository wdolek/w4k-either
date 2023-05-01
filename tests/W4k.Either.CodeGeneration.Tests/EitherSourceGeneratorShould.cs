namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class EitherSourceGeneratorShould
{
    [Fact]
    public Task GenerateUsingAttributeAndNullRefEnabled()
    {
        var source = @"
#nullable enable
using W4k.Either.Abstractions;

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
    public Task GenerateUsingAttributeAndNullRefDisabled()
    {
        var source = @"
#nullable disable
using W4k.Either.Abstractions;

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
    public Task GenerateUsingGenericsAndNullRefEnabled()
    {
        // intentionally not setting any constraint for `TAny` -> it can be anything
        var source = @"
#nullable enable
using W4k.Either.Abstractions;

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
    public Task GenerateUsingGenericsAndNullRefDisabled()
    {
        // intentionally not setting any constraint for `TAny` -> it can be anything
        var source = @"
#nullable disable
using W4k.Either.Abstractions;

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

    [Fact]
    public Task GenerateNestedType()
    {
        var source = @"
using W4k.Either.Abstractions;

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

    [Theory]
    [MemberData(nameof(CreateDiagnosticErrorProducingSourceCode))]
    public void ReportErrorForInvalidCode(string source, string expectedDiagnosticId)
    {
        var (diagnostics, _) = TestHelper.GenerateSourceCode(source);

        Assert.NotEmpty(diagnostics);
        Assert.Equal(expectedDiagnosticId, diagnostics[0].Id);
    }

    public static TheoryData<string, string> CreateDiagnosticErrorProducingSourceCode()
    {
        const string notPartial = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public struct MyEither<T0, T1>
    {
    }
}";
        
        const string containingNotPartial = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    public class MyContainingType
    {
        [Either]
        public partial struct MyEither<T0, T1>
        {
        }
    }
}";
        
        const string tooFewTypeParams = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<T0>
    {
    }
}";

        const string ambiguousTypeParams = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(string))]
    public partial struct MyEither<T0, T1>
    {
    }
}";
        
        const string noTypeParam = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither
    {
    }
}";
        
        const string typesNotUnique = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(int))]
    public partial struct MyEither
    {
    }
}";
        
        const string attrWithOpenGenerics = @"
using W4k.Either.Abstractions;
using System.Collections.Generic;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(List<>))]
    public partial struct MyEither
    {
    }
}";

        return new TheoryData<string, string>
        {
            {
                notPartial,
                "W4KE001"
            },
            {
                containingNotPartial,
                "W4KE001"
            },
            {
                tooFewTypeParams,
                "W4KE002"
            },
            {
                ambiguousTypeParams,
                "W4KE003"
            },
            {
                noTypeParam,
                "W4KE004"
            },
            {
                typesNotUnique,
                "W4KE005"
            },
            {
                attrWithOpenGenerics,
                "W4KE006"
            }
        };
    }
}

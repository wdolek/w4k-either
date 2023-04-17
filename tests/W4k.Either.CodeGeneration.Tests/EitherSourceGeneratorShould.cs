namespace W4k.Either.CodeGeneration.Tests;

[UsesVerify]
public class EitherSourceGeneratorShould
{
    [Fact]
    public Task GenerateIntOrStr()
    {
        var source = @"
using System;
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(string))]
    public partial struct IntOrStr
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }    

    [Fact]
    public Task GenerateGenericEither2()
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

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task GenerateGenericEither3()
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

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    public Task GenerateGenericWithOneValueAndNullableRefType()
    {
        var source = @"
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither<TLeft, TRight>
        where TLeft : struct
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task GenerateWithGenericType()
    {
        var source = @"
using System.Collections.Generic;
using W4k.Either.Abstractions;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(string), typeof(List<int>))]
    public partial struct MyEither
    {
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

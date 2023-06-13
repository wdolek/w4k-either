namespace W4k.Either.CodeGeneration;

public class DiagnosticsTests
{
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
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public struct MyEither<T0, T1>
    {
    }
}";
        
        const string containingNotPartial = @"
using W4k.Either;

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
        
        const string ambiguousTypeParams = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(string))]
    public partial struct MyEither<T0, T1>
    {
    }
}";
        
        const string ambiguousTypeParamsWithGenericAttr = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either<int, string>]
    public partial struct MyEither<T0, T1>
    {
    }
}";
        
        const string ambiguousTypeParamsWithGenericAttrAndOtherAttr = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(string))]
    [Either<int, string>]
    public partial struct MyEither
    {
    }
}";
        
        const string noTypeParam = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither
    {
    }
}";
        
        const string typesNotUnique = @"
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(int))]
    public partial struct MyEither
    {
    }
}";
        
        const string attrWithOpenGenerics = @"
using W4k.Either;
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
                ambiguousTypeParams,
                "W4KE003"
            },
            {
                ambiguousTypeParamsWithGenericAttr,
                "W4KE003"
            },
            {
                ambiguousTypeParamsWithGenericAttrAndOtherAttr,
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

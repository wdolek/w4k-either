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
// struct not partial
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public struct MyEither<T0, T1>
    {
    }
}";
        
        const string containingNotPartial = @"
// class not partial
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
// ambiguous type params #1
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(string))]
    public partial struct MyEither<T0, T1>
    {
    }
}";
        
        const string ambiguousTypeParamsWithGenericAttr = @"
// ambiguous type params #2
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either<int, string>]
    public partial struct MyEither<T0, T1>
    {
    }
}";
        
        const string ambiguousTypeParamsWithGenericAttrAndOtherAttr = @"
// ambiguous type params #3
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
// no type parametrization
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either]
    public partial struct MyEither
    {
    }
}";
        
        const string typesNotUnique = @"
// types not unique
using W4k.Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Either(typeof(int), typeof(int))]
    public partial struct MyEither
    {
    }
}";
        
        const string attrWithOpenGenerics = @"
// attr with open generics
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

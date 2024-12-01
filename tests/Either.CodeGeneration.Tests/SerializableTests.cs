namespace Either.CodeGeneration;

public class SerializableTests
{
    [Fact]
    public Task GenerateSerializableMembers()
    {
        var source = @"
using System;
using System.Runtime.Serialization;
using Either;

namespace MyLittleEither.MyLittleEitherMonad
{
    [Serializable, Either]
    public partial struct MyEither<TLeft, TRight> : ISerializable
    {
    }
}";

        var (diagnostics, output) = TestHelper.GenerateSourceCode(source);
        Assert.Empty(diagnostics);

        return Verify(output).UseDirectory("Snapshots");
    }
}
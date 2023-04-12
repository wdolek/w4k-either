using System.Runtime.CompilerServices;

namespace W4k.Either.CodeGeneration.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init() => VerifySourceGenerators.Initialize();
}

using System.Runtime.CompilerServices;

namespace W4k.Either.CodeGeneration;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init() => VerifySourceGenerators.Initialize();
}

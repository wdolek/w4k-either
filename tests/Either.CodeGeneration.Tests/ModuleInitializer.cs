using System.Runtime.CompilerServices;

namespace Either.CodeGeneration;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init() => VerifySourceGenerators.Initialize();
}
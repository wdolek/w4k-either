namespace W4k.Either.CodeGeneration.Generator;

internal interface IMemberCodeGenerator
{
    bool CanGenerate();
    void Generate(IndentedWriter writer);
}
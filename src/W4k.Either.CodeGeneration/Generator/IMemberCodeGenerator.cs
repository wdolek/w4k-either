namespace W4k.Either.Generator;

internal interface IMemberCodeGenerator
{
    bool CanGenerate();
    void Generate(IndentedWriter writer);
}
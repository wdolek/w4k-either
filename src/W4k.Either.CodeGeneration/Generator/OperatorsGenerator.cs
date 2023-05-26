namespace W4k.Either.CodeGeneration.Generator;

internal class OperatorsGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public OperatorsGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter writer)
    {
        var referringTypeName = _context.TypeDeclaration.TypeSymbol.ToDisplayString();
        
        writer.AppendIndentedLine("[Pure]");    
        writer.AppendIndentedLine($"public static bool operator ==({referringTypeName} left, {referringTypeName} right) => left.Equals(right);");
        writer.AppendLineBreak();

        writer.AppendIndentedLine("[Pure]");
        writer.AppendIndentedLine($"public static bool operator !=({referringTypeName} left, {referringTypeName} right) => !left.Equals(right);");
        writer.AppendLineBreak();

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine("[Pure]");
            writer.AppendIndentedLine($"public static implicit operator {referringTypeName}({typeParam.AsArgument} value) => new(value);");
            writer.AppendLineBreak();
        }
    }
}

namespace W4k.Either.Generator;

internal sealed class FieldsGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public FieldsGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter sb)
    {
        sb.AppendIndentedLine("private readonly byte _idx;");

        foreach (var typeParam in _context.TypeParameters)
        {
            sb.AppendIndentedLine($"private readonly {typeParam.AsFieldType} {typeParam.FieldName};");
        }

        sb.AppendLineBreak();
    }
}

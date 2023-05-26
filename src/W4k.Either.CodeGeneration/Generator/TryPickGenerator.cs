namespace W4k.Either.CodeGeneration.Generator;

internal class TryPickGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public TryPickGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter writer)
    {
        foreach (var typeParam in _context.TypeParameters)
        {
            var notNullWhenTrue = typeParam.IsNonNullableReferenceType
                ? "[NotNullWhen(true)] "
                : string.Empty;

            writer.AppendIndentedLine("[Pure]");
            writer.AppendIndentedLine($"public bool TryPick({notNullWhenTrue}out {typeParam.AsFieldType} value)");
            writer.AppendIndentedLine("{");
            writer.AppendIndentedLine($"    if (_idx == {typeParam.Index})");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine($"        value = {typeParam.AsFieldReceiver};");
            writer.AppendIndentedLine("        return true;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
            writer.AppendIndentedLine($"    value = {typeParam.AsDefault};");
            writer.AppendIndentedLine("    return false;");
            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }
}

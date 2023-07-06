namespace W4k.Either.Generator;

internal sealed class TryPickGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public TryPickGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => !_context.Skip.Contains("TryPick");

    public void Generate(IndentedWriter writer)
    {
        foreach (var typeParam in _context.TypeParameters)
        {
            var notNullWhenTrue = typeParam.IsNonNullableReferenceType
                ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] "
                : string.Empty;

            writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
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

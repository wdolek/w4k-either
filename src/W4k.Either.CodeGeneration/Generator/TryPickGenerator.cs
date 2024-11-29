namespace W4k.Either.Generator;

internal sealed class TryPickGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public TryPickGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => _context.Generate.ShouldGenerate(Members.TryPick)
        || (_context.Generate.ShouldGenerate(Members.TryPickWithRemainder) && _context.TypeParameters.Length == 2);

    public void Generate(IndentedWriter writer)
    {
        if (_context.Generate.ShouldGenerate(Members.TryPick))
        {
            GenerateSingleOutParam(writer);
        }

        if (_context.Generate.ShouldGenerate(Members.TryPickWithRemainder) && _context.TypeParameters.Length == 2)
        {
            GenerateWithSimpleRemainder(writer);
        }
    }

    private void GenerateSingleOutParam(IndentedWriter writer)
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

    private void GenerateWithSimpleRemainder(IndentedWriter writer)
    {
        var typeParameters = _context.TypeParameters;
        foreach (var typeParam in typeParameters)
        {
            // determine other type parameter (out of two values in array)
            // NB! .Index property uses one-based indexing
            //     array [0] -> Index(1)
            //     array [1] -> Index(2)
            var otherTypeParam = typeParam.Index == 1
                ? typeParameters[1]
                : typeParameters[0];

            var notNullWhenTrue = typeParam.IsNonNullableReferenceType
                ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] "
                : string.Empty;

            var otherNotNullWhenFalse = otherTypeParam.IsNonNullableReferenceType
                ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] "
                : string.Empty;

            writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
            writer.AppendIndentedLine($"public bool TryPick({notNullWhenTrue}out {typeParam.AsFieldType} value, {otherNotNullWhenFalse}out {otherTypeParam.AsFieldType} remainder)");
            writer.AppendIndentedLine("{");
            writer.AppendIndentedLine($"    if (_idx == {typeParam.Index})");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine($"        value = {typeParam.AsFieldReceiver};");
            writer.AppendIndentedLine($"        remainder = {otherTypeParam.AsDefault};");
            writer.AppendIndentedLine("        return true;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
            writer.AppendIndentedLine($"    value = {typeParam.AsDefault};");
            writer.AppendIndentedLine($"    remainder = {otherTypeParam.AsFieldReceiver};");
            writer.AppendIndentedLine("    return false;");
            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }
}
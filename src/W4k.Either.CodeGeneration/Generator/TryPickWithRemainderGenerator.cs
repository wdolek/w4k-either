namespace W4k.Either.Generator;

internal sealed class TryPickWithRemainderGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public TryPickWithRemainderGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => !_context.Skip.Contains("TryPick")
        && _context.TypeParameters.Length >= 2;

    public void Generate(IndentedWriter writer)
    {
        if (_context.TypeParameters.Length == 2)
        {
            GenerateWithOther(writer);
        }
        else
        {
            GenerateWithRemainder(writer);
        }
    }

    private void GenerateWithOther(IndentedWriter writer)
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
                ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] "
                : string.Empty;

            writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
            writer.AppendIndentedLine($"public bool TryPick({notNullWhenTrue}out {typeParam.AsFieldType} value, {otherNotNullWhenFalse}out {otherTypeParam.AsFieldType} remainder)");
            writer.AppendIndentedLine("{");
            writer.AppendIndentedLine($"    if (_idx == {typeParam.Index})");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine($"        value = {typeParam.AsFieldReceiver};");
            writer.AppendIndentedLine("        return true;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
            writer.AppendIndentedLine($"    value = {otherTypeParam.AsFieldReceiver};");
            writer.AppendIndentedLine("    return false;");
            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }

    private void GenerateWithRemainder(IndentedWriter writer)
    {
        var typeSymbolName = _context.TypeDeclaration.TypeSymbol.Name;
        var typeParameters = _context.TypeParameters;

        var remainderNotNullWhenFalse = _context.TypeDeclaration.TypeSymbol.IsReferenceType
            ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(false)] "
            : string.Empty;

        foreach (var typeParam in typeParameters)
        {
            var remainderTypeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index);

            var notNullWhenTrue = typeParam.IsNonNullableReferenceType
                ? "[global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] "
                : string.Empty;

            writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
            writer.AppendIndentedLine($"public bool TryPick({notNullWhenTrue}out {typeParam.AsFieldType} value, {remainderNotNullWhenFalse}out {remainderTypeName} remainder)");
            writer.AppendIndentedLine("{");
            writer.AppendIndentedLine($"    if (_idx == {typeParam.Index})");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine($"        value = {typeParam.AsFieldReceiver};");
            writer.AppendIndentedLine("        return true;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();

            // instantiate remainder type - using appropriate constructor according to current state (_idx)
            writer.AppendIndentedLine("    switch(_idx)");
            writer.AppendIndentedLine("    {");

            foreach (var otherTypeParam in typeParameters)
            {
                if (typeParam.Index == otherTypeParam.Index)
                {
                    continue;
                }

                writer.AppendIndentedLine($"        case {otherTypeParam.Index}:");
                writer.AppendIndentedLine($"            value = new {remainderTypeName}({otherTypeParam.AsFieldReceiver});");
                writer.AppendIndentedLine("            break;");
            }

            writer.AppendIndentedLine("        default:");
            writer.AppendIndentedLine("            global::W4k.Either.ThrowHelper.ThrowOnInvalidState();");
            writer.AppendIndentedLine("            break;");
            writer.AppendIndentedLine("    }");

            writer.AppendLineBreak();
            writer.AppendIndentedLine("    return false;");
            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }
}

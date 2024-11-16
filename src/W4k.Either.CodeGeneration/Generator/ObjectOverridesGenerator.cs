namespace W4k.Either.Generator;

internal sealed class ObjectOverridesGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public ObjectOverridesGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter writer)
    {
        WriteGetHashCode(writer);
        WriteToString(writer);
        WriteObjectEquals(writer);
    }

    private void WriteGetHashCode(IndentedWriter writer)
    {
        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        writer.AppendIndentedLine("public override int GetHashCode()");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    switch (_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine(
                typeParam.IsNullable
                    ? $"            return {typeParam.AsFieldInvoker}.GetHashCode() ?? 0;"
                    : $"            return {typeParam.AsFieldInvoker}.GetHashCode();");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<int>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }

    private void WriteToString(IndentedWriter writer)
    {
        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        writer.AppendIndentedLine("public override string ToString()");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    switch (_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            return {typeParam.AsFieldInvoker}.ToString() ?? string.Empty;");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<string>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }

    private void WriteObjectEquals(IndentedWriter writer)
    {
        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        writer.AppendIndentedLine("public override bool Equals(object? obj)");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine($"    if (obj is not {_context.TypeDeclaration.TypeSymbol.ToDisplayString()} other)");
        writer.AppendIndentedLine("    {");
        writer.AppendIndentedLine("        return false;");
        writer.AppendIndentedLine("    }");
        writer.AppendLineBreak();
        writer.AppendIndentedLine("    return Equals(other);");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}
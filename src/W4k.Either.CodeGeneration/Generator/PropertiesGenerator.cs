namespace W4k.Either.CodeGeneration.Generator;

internal sealed class PropertiesGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public PropertiesGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => !_context.Skip.Contains("Case");

    public void Generate(IndentedWriter sb)
    {
        sb.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        sb.AppendIndentedLine("public object? Case");
        sb.AppendIndentedLine("{");
        sb.AppendIndentedLine("    get");
        sb.AppendIndentedLine("    {");
        sb.AppendIndentedLine("        switch (_idx)");
        sb.AppendIndentedLine("        {");

        foreach (var typeParam in _context.TypeParameters)
        {
            sb.AppendIndentedLine($"            case {typeParam.Index}:");
            sb.AppendIndentedLine($"                return {typeParam.FieldName};");            
        }

        sb.AppendIndentedLine("            default:");
        sb.AppendIndentedLine("                return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<object?>();");
        sb.AppendIndentedLine("        }");
        sb.AppendIndentedLine("    }");
        sb.AppendIndentedLine("}");
        sb.AppendLineBreak();
    }
}

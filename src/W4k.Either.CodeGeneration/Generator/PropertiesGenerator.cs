namespace W4k.Either.CodeGeneration.Generator;

internal class PropertiesGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public PropertiesGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter sb)
    {
        sb.AppendIndentedLine("[Pure]");
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
        sb.AppendIndentedLine("                return ThrowHelper.ThrowOnInvalidState<object?>();");
        sb.AppendIndentedLine("        }");
        sb.AppendIndentedLine("    }");
        sb.AppendIndentedLine("}");
        sb.AppendLineBreak();
    }
}

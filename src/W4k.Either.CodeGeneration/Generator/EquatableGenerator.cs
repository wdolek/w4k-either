namespace W4k.Either.CodeGeneration.Generator;

internal class EquatableGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public EquatableGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate()
    {
        // TODO: check if type implements `IEquatable<T>`
        return true;
    }

    public void Generate(IndentedWriter writer)
    {
        // TODO: handle class (may be null)
        writer.AppendIndentedLine("[Pure]");
        writer.AppendIndentedLine($"public bool Equals({_context.TypeDeclaration.TypeSymbol.ToDisplayString()} other)");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    if (_idx != other._idx)");
        writer.AppendIndentedLine("    {");
        writer.AppendIndentedLine("        return false;");
        writer.AppendIndentedLine("    }");
        writer.AppendLineBreak();
        writer.AppendIndentedLine("    switch (_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");

            if (!typeParam.IsNullable)
            {
                // -> non-nullable
                writer.AppendIndentedLine($"            return {typeParam.AsFieldInvoker}.Equals(other.{typeParam.FieldName});");
            } 
            else if (typeParam.IsReferenceType)
            {
                // -> possibly nullable reference type
                writer.AppendIndentedLine($"            return ({typeParam.FieldName} is null && other.{typeParam.FieldName} is null) || ({typeParam.AsFieldInvoker}.Equals(other.{typeParam.FieldName}) ?? false);");
            }
            else
            {
                // -> value type
                writer.AppendIndentedLine($"            return {typeParam.AsFieldInvoker}.Equals(other.{typeParam.FieldName}) ?? false;");
            }
        }
        
        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return ThrowHelper.ThrowOnInvalidState<bool>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}

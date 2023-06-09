using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.Generator;

internal sealed class OperatorsGenerator : IMemberCodeGenerator
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

        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");

        if (_context.TypeKind == TypeKind.Struct)
        {
            writer.AppendIndentedLine($"public static bool operator ==({referringTypeName} left, {referringTypeName} right) => left.Equals(right);");
        }
        else
        {
            writer.AppendIndentedLine($"public static bool operator ==({referringTypeName} left, {referringTypeName} right)");
            writer.AppendIndentedLine("{");
            writer.AppendIndentedLine("    if (ReferenceEquals(left, null))");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine("        return ReferenceEquals(right, null);");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
            writer.AppendIndentedLine("    return left.Equals(right);");
            writer.AppendIndentedLine("}");
        }

        writer.AppendLineBreak();

        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        writer.AppendIndentedLine($"public static bool operator !=({referringTypeName} left, {referringTypeName} right) => !(left == right);");
        writer.AppendLineBreak();

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
            writer.AppendIndentedLine($"public static implicit operator {referringTypeName}({typeParam.AsArgument} value) => new(value);");
            writer.AppendLineBreak();
        }
    }
}

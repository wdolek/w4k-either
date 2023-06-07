using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration.Generator;

internal class MapGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public MapGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => _context.ParametrizationKind == ParametrizationKind.Generic;

    public void Generate(IndentedWriter writer)
    {
        var typeSymbolName = _context.TypeDeclaration.TypeSymbol.Name;
        var typeParameters = _context.TypeParameters;
        var newTypeParamName = TypeNameBuilder.GetUniqueTypeParamName(typeParameters);

        foreach (var typeParam in typeParameters)
        {
            var typeName = TypeNameBuilder.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);

            writer.AppendIndentedLine($"public {typeName} Map<{newTypeParamName}>(global::System.Func<{typeParam.AsArgument}, {newTypeParamName}> mapper)");
            writer.AppendIndentedLine("{");

            writer.AppendIndentedLine("    switch (_idx)");
            writer.AppendIndentedLine("    {");

            foreach (var mappedTypeParam in typeParameters)
            {
                writer.AppendIndentedLine($"        case {mappedTypeParam.Index}:");
                writer.AppendIndentedLine(
                    mappedTypeParam.Index == typeParam.Index
                        ? $"            return new {typeName}(mapper({mappedTypeParam.AsFieldReceiver}));"
                        : $"            return new {typeName}({mappedTypeParam.AsFieldReceiver});");
            }
            
            writer.AppendIndentedLine("        default:");
            writer.AppendIndentedLine($"            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<{typeName}>();");            
            writer.AppendIndentedLine("    }");

            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }
}

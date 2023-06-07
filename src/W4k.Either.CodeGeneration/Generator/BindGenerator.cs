using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration.Generator;

internal class BindGenerator : IMemberCodeGenerator
{
    private static readonly string[] CandidateNames = { "TNew", "TBound" };
    
    private readonly GeneratorContext _context;

    public BindGenerator(GeneratorContext context)
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

            writer.AppendIndentedLine($"public {typeName} Bind<{newTypeParamName}>(global::System.Func<{typeParam.AsArgument}, {typeName}> binder)");
            writer.AppendIndentedLine("{");

            writer.AppendIndentedLine("    switch (_idx)");
            writer.AppendIndentedLine("    {");

            foreach (var boundTypeParam in typeParameters)
            {
                writer.AppendIndentedLine($"        case {boundTypeParam.Index}:");
                writer.AppendIndentedLine(
                    boundTypeParam.Index == typeParam.Index
                        ? $"            return binder({boundTypeParam.AsFieldReceiver});"
                        : $"            return new {typeName}({boundTypeParam.AsFieldReceiver});");
            }
            
            writer.AppendIndentedLine("        default:");
            writer.AppendIndentedLine($"            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<{typeName}>();");            
            writer.AppendIndentedLine("    }");

            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }        
    }
}

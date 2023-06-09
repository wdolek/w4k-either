using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration.Generator;

internal sealed class BindGenerator : IMemberCodeGenerator
{
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
        var newTypeParamName = TypeGeneratorHelper.GetTypeParamName(typeParameters);
        
        foreach (var typeParam in typeParameters)
        {
            var typeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);
            var constraints = TypeGeneratorHelper.GetTypeParamConstraints((ITypeParameterSymbol)typeParam.TypeSymbol);

            writer.AppendIndentedLine($"public {typeName} Bind<{newTypeParamName}>(global::System.Func<{typeParam.AsArgument}, {typeName}> binder)");
            if (constraints.Count > 0)
            {
                writer.AppendIndentedLine($"    where {newTypeParamName} : {string.Join(", ", constraints)}");
            }

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

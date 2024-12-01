using Microsoft.CodeAnalysis;
using Either.TypeParametrization;

namespace Either.Generator;

internal sealed class MapGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public MapGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() =>
        _context.ParametrizationKind == ParametrizationKind.Generic
        && (_context.Generate.ShouldGenerate(Members.Map) || _context.Generate.ShouldGenerate(Members.MapWithState));

    public void Generate(IndentedWriter writer)
    {
        var typeSymbolName = _context.TypeDeclaration.TypeSymbol.Name;
        var typeParameters = _context.TypeParameters;
        var newTypeParamName = TypeGeneratorHelper.GetTypeParamName(typeParameters);

        foreach (var typeParam in typeParameters)
        {
            if (_context.Generate.ShouldGenerate(Members.Map))
            {
                WriteMap(writer, typeParam, typeSymbolName, typeParameters, newTypeParamName);
            }

            if (_context.Generate.ShouldGenerate(Members.MapWithState))
            {
                WriteMapWithState(writer, typeParam, typeSymbolName, typeParameters, newTypeParamName);
            }
        }
    }

    private static void WriteMap(
        IndentedWriter writer,
        TypeParameter typeParam,
        string typeSymbolName,
        TypeParameter[] typeParameters,
        string newTypeParamName)
    {
        var typeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);
        var constraints = TypeGeneratorHelper.GetTypeParamConstraints((ITypeParameterSymbol)typeParam.TypeSymbol);

        writer.AppendIndentedLine(
            $"public {typeName} Map<{newTypeParamName}>(global::System.Func<{typeParam.AsArgument}, {newTypeParamName}> mapper)");

        if (constraints.Count > 0)
        {
            writer.AppendIndentedLine($"    where {newTypeParamName} : {string.Join(", ", constraints)}");
        }

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
        writer.AppendIndentedLine($"            return global::Either.ThrowHelper.ThrowOnInvalidState<{typeName}>();");
        writer.AppendIndentedLine("    }");

        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }

    private static void WriteMapWithState(
        IndentedWriter writer,
        TypeParameter typeParam,
        string typeSymbolName,
        TypeParameter[] typeParameters,
        string newTypeParamName)
    {
        var typeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);
        var constraints = TypeGeneratorHelper.GetTypeParamConstraints((ITypeParameterSymbol)typeParam.TypeSymbol);

        writer.AppendIndentedLine(
            $"public {typeName} Map<TState, {newTypeParamName}>(TState state, global::System.Func<TState, {typeParam.AsArgument}, {newTypeParamName}> mapper)");

        if (constraints.Count > 0)
        {
            writer.AppendIndentedLine($"    where {newTypeParamName} : {string.Join(", ", constraints)}");
        }

        writer.AppendIndentedLine("{");

        writer.AppendIndentedLine("    switch (_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var mappedTypeParam in typeParameters)
        {
            writer.AppendIndentedLine($"        case {mappedTypeParam.Index}:");
            writer.AppendIndentedLine(
                mappedTypeParam.Index == typeParam.Index
                    ? $"            return new {typeName}(mapper(state, {mappedTypeParam.AsFieldReceiver}));"
                    : $"            return new {typeName}({mappedTypeParam.AsFieldReceiver});");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine($"            return global::Either.ThrowHelper.ThrowOnInvalidState<{typeName}>();");
        writer.AppendIndentedLine("    }");

        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}
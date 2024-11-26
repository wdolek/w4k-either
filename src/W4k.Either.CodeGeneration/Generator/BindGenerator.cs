using Microsoft.CodeAnalysis;
using W4k.Either.TypeParametrization;

namespace W4k.Either.Generator;

internal sealed class BindGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public BindGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() =>
        _context.ParametrizationKind == ParametrizationKind.Generic
        && !_context.Skip.Contains("Bind*");

    public void Generate(IndentedWriter writer)
    {
        var typeSymbolName = _context.TypeDeclaration.TypeSymbol.Name;
        var typeParameters = _context.TypeParameters;
        var newTypeParamName = TypeGeneratorHelper.GetTypeParamName(typeParameters);

        foreach (var typeParam in typeParameters)
        {
            if (!_context.Skip.Contains("Bind"))
            {
                WriteBind(writer, typeParam, typeSymbolName, typeParameters, newTypeParamName);
            }

            if (!_context.Skip.Contains("Bind<TState>"))
            {
                WriteBindWithState(writer, typeParam, typeSymbolName, typeParameters, newTypeParamName);
            }
        }
    }

    private static void WriteBind(
        IndentedWriter writer,
        TypeParameter typeParam,
        string typeSymbolName,
        TypeParameter[] typeParameters,
        string newTypeParamName)
    {
        var typeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);
        var constraints = TypeGeneratorHelper.GetTypeParamConstraints((ITypeParameterSymbol)typeParam.TypeSymbol);

        writer.AppendIndentedLine(
            $"public {typeName} Bind<{newTypeParamName}>(global::System.Func<{typeParam.AsArgument}, {typeName}> binder)");

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

    private static void WriteBindWithState(
        IndentedWriter writer,
        TypeParameter typeParam,
        string typeSymbolName,
        TypeParameter[] typeParameters,
        string newTypeParamName)
    {
        var typeName = TypeGeneratorHelper.GetTypeName(typeSymbolName, typeParameters, typeParam.Index, newTypeParamName);
        var constraints = TypeGeneratorHelper.GetTypeParamConstraints((ITypeParameterSymbol)typeParam.TypeSymbol);

        writer.AppendIndentedLine(
            $"public {typeName} Bind<TState, {newTypeParamName}>(TState state, global::System.Func<TState, {typeParam.AsArgument}, {typeName}> binder)");

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
                    ? $"            return binder(state, {boundTypeParam.AsFieldReceiver});"
                    : $"            return new {typeName}({boundTypeParam.AsFieldReceiver});");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine($"            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<{typeName}>();");
        writer.AppendIndentedLine("    }");

        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}
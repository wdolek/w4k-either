using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration.Generator;

internal class SerializableGenerator : IMemberCodeGenerator
{
    private const string SerializableInterfaceName = "System.Runtime.Serialization.ISerializable";
    private const string SerializationInfoTypeName = "System.Runtime.Serialization.SerializationInfo";
    private const string StreamingContextTypeName = "System.Runtime.Serialization.StreamingContext";
    private const string GetObjectDataMethodName = "GetObjectData";

    private readonly GeneratorContext _context;

    private bool _generateCtor;
    private bool _generateGetObjectData;

    public SerializableGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate()
    {
        var type = _context.TypeDeclaration.TypeSymbol;

        if (!ImplementsISerializable(type))
        {
            return false;
        }

        _generateCtor = !HasSerializableCtorDeclared(type);
        _generateGetObjectData = !HasGetObjectDataDeclared(type);

        return _generateCtor && _generateGetObjectData;
    }

    public void Generate(IndentedWriter writer)
    {
        if (_generateCtor)
        {
            GenerateSerializableConstructor(writer);
        }
        
        if (_generateGetObjectData)
        {
            GenerateGetObjectData(writer);
        }
    }

    private static bool ImplementsISerializable(INamedTypeSymbol typeSymbol)
    {
        var isSerializable = false;
        foreach (var implementedInterface in typeSymbol.AllInterfaces)
        {
            if (implementedInterface.ToDisplayString() == SerializableInterfaceName)
            {
                isSerializable = true;
                break;
            }
        }

        return isSerializable;
    }

    private static bool HasSerializableParameters(ImmutableArray<IParameterSymbol> parameters) =>
        parameters.Length == 2
        && parameters[0].Type.ToDisplayString() == SerializationInfoTypeName
        && parameters[1].Type.ToDisplayString() == StreamingContextTypeName;

    private static bool HasSerializableCtorDeclared(INamedTypeSymbol typeSymbol)
    {
        var isCtorDeclared = false;
        foreach (var ctor in typeSymbol.Constructors)
        {
            if (ctor.Parameters.Length != 2)
            {
                continue;
            }

            isCtorDeclared = HasSerializableParameters(ctor.Parameters);
            break;
        }

        return isCtorDeclared;
    }

    private static bool HasGetObjectDataDeclared(INamedTypeSymbol typeSymbol)
    {
        var hasGetObjectDataDeclared = false;
        foreach (var member in typeSymbol.GetMembers())
        {
            if (member is not IMethodSymbol method)
            {
                continue;
            }

            // implicit implementation
            if (IsGetObjectDataMethod(method))
            {
                hasGetObjectDataDeclared = true;
                break;
            }

            // explicit implementation
            if (IsExplicitlyImplemented(method))
            {
                hasGetObjectDataDeclared = true;
                break;
            }
        }

        return hasGetObjectDataDeclared;

        static bool IsGetObjectDataMethod(IMethodSymbol method) =>
            method.Name == GetObjectDataMethodName
            && HasSerializableParameters(method.Parameters);

        static bool IsExplicitlyImplemented(IMethodSymbol method)
        {
            if (method.ExplicitInterfaceImplementations.Length == 0)
            {
                return false;
            }

            foreach (var interfaceMethod in method.ExplicitInterfaceImplementations)
            {
                if (IsGetObjectDataMethod(interfaceMethod))
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void GenerateSerializableConstructor(IndentedWriter sb)
    {
        var accessModifier = _context.TypeKind == TypeKind.Struct || _context.TypeDeclaration.TypeSymbol.IsSealed
            ? "private"
            : "protected";

        sb.AppendIndentedLine($"{accessModifier} {_context.TypeDeclaration.TypeSymbol.Name}(SerializationInfo info, StreamingContext context)");
        sb.AppendIndentedLine("{");
        sb.AppendIndentedLine("    _idx = info.GetByte(nameof(_idx));");
        sb.AppendIndentedLine("    switch (_idx)");
        sb.AppendIndentedLine("    {");

        foreach (var typeParam in _context.TypeParameters)
        {
            // specifying type using attribute: for value types we want to avoid "Unboxing a possibly null value" warning
            var nullForgiving = typeParam.IsValueType && !typeParam.IsNullable
                ? "!"
                : string.Empty;
            
            sb.AppendIndentedLine($"        case {typeParam.Index}:");

            foreach (var otherTypeParam in _context.TypeParameters)
            {
                sb.AppendIndentedLine(
                    otherTypeParam.Index == typeParam.Index
                        ? $"            {otherTypeParam.FieldName} = ({typeParam.AsFieldType})info.GetValue(\"{typeParam.FieldName}\", typeof({typeParam.Name})){nullForgiving};"
                        : $"            {otherTypeParam.FieldName} = {otherTypeParam.AsDefault};");
            }

            sb.AppendIndentedLine("            break;");
        }
        
        sb.AppendIndentedLine("        default:");
        
        foreach (var otherTypeParam in _context.TypeParameters)
        {
            sb.AppendIndentedLine($"            {otherTypeParam.FieldName} = {otherTypeParam.AsDefault};");
        }          
        
        sb.AppendIndentedLine("            ThrowHelper.ThrowOnInvalidState();");
        sb.AppendIndentedLine("        break;");
        sb.AppendIndentedLine("    }");
        sb.AppendIndentedLine("}");
        sb.AppendLineBreak();
    }

    private void GenerateGetObjectData(IndentedWriter writer)
    {
        writer.AppendIndentedLine("void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    info.AddValue(\"_idx\", _idx);");
        writer.AppendIndentedLine("    switch (_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var typeParam in _context.TypeParameters)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            info.AddValue(\"{typeParam.FieldName}\", {typeParam.FieldName});");
            writer.AppendIndentedLine("             break;");
        }
        
        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            ThrowHelper.ThrowOnInvalidState();");
        writer.AppendIndentedLine("            break;");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}

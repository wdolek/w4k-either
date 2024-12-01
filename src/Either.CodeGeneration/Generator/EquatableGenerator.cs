using Microsoft.CodeAnalysis;

namespace Either.Generator;

internal sealed class EquatableGenerator : IMemberCodeGenerator
{
    private const string GetObjectDataMethodName = "Equals";

    private readonly GeneratorContext _context;

    public EquatableGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate()
    {
        var type = _context.TypeDeclaration.TypeSymbol;

        // NB! we don't check whether type implements `IEquatable<T>` as it's not relevant - we generate `Equals(<declared type>)`
        //     in any case as it's used by operators. Generation is omitted only if method is already declared by the user.
        return !HasEqualsDeclared(type);
    }

    public void Generate(IndentedWriter writer)
    {
        writer.AppendIndentedLine("[global::System.Diagnostics.Contracts.Pure]");
        writer.AppendIndentedLine($"public bool Equals({_context.TypeDeclaration.TypeSymbol.ToDisplayString()} other)");
        writer.AppendIndentedLine("{");

        if (_context.TypeKind == TypeKind.Class)
        {
            writer.AppendIndentedLine("    if (ReferenceEquals(null, other))");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine("        return false;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
            writer.AppendIndentedLine("    if (ReferenceEquals(this, other))");
            writer.AppendIndentedLine("    {");
            writer.AppendIndentedLine("        return true;");
            writer.AppendIndentedLine("    }");
            writer.AppendLineBreak();
        }

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
        writer.AppendIndentedLine("            return global::Either.ThrowHelper.ThrowOnInvalidState<bool>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }

    private static bool HasEqualsDeclared(INamedTypeSymbol typeSymbol)
    {
        var hasEqualsDeclared = false;
        foreach (var member in typeSymbol.GetMembers())
        {
            if (member is not IMethodSymbol method)
            {
                continue;
            }

            // implicit implementation
            if (IsEqualsMethod(method, typeSymbol))
            {
                hasEqualsDeclared = true;
                break;
            }

            // explicit implementation
            if (IsExplicitlyImplemented(method, typeSymbol))
            {
                hasEqualsDeclared = true;
                break;
            }
        }

        return hasEqualsDeclared;

        static bool IsEqualsMethod(IMethodSymbol method, INamedTypeSymbol type)
        {
            return method.Name == GetObjectDataMethodName
                   && method.Parameters.Length == 1
                   && method.Parameters[0].Equals(type, SymbolEqualityComparer.Default);
        }

        static bool IsExplicitlyImplemented(IMethodSymbol method, INamedTypeSymbol type)
        {
            if (method.ExplicitInterfaceImplementations.Length == 0)
            {
                return false;
            }

            foreach (var interfaceMethod in method.ExplicitInterfaceImplementations)
            {
                if (IsEqualsMethod(interfaceMethod, type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
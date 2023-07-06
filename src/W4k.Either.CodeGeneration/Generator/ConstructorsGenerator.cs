using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using W4k.Either.TypeParametrization;

namespace W4k.Either.Generator;

internal sealed class ConstructorsGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    private TypeParameter[] _generateCtorForTypes = Array.Empty<TypeParameter>();

    public ConstructorsGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate()
    {
        // no constructor declared, generate for all type parameters
        var declaredConstructors = _context.TypeDeclaration.TypeSymbol.InstanceConstructors;
        if (declaredConstructors.Length == 0)
        {
            _generateCtorForTypes = _context.TypeParameters;
            return true;
        }

        var typeParameters = _context.TypeParameters;
        var declaredConstructorParams = new List<TypeParameter>();

        // find declared constructors with relevant type parameter
        foreach (var ctor in declaredConstructors)
        {
            if (ctor.Parameters.Length != 1)
            {
                continue;
            }

            // check whether declared constructor has type parameter relevant to generation context,
            // e.g. user declared constructor for one of types themselves
            var ctorParam = ctor.Parameters[0];
            if (TryLookupTypeParameter(typeParameters, ctorParam.Type, out var typeParam))
            {
                declaredConstructorParams.Add(typeParam);
            }
        }

        // get difference between all type parameters and parameters of declared constructors
        _generateCtorForTypes = Except(typeParameters, declaredConstructorParams);
        
        return _generateCtorForTypes.Length > 0;
    }

    public void Generate(IndentedWriter writer)
    {
        var typeName = _context.TypeDeclaration.TypeSymbol.Name;

        var local = _generateCtorForTypes;
        foreach (var typeParam in local)
        {
            writer.AppendIndentedLine($"public {typeName}({typeParam.AsArgument} value)");
            writer.AppendIndentedLine("{");

            if (typeParam.IsNonNullableReferenceType)
            {
                writer.AppendIndentedLine("    global::System.ArgumentNullException.ThrowIfNull(value);");                
            }

            writer.AppendIndentedLine($"    _idx = {typeParam.Index};");
                
            foreach (var otherTypeParam in _context.TypeParameters)
            {
                writer.AppendIndentedLine(
                    otherTypeParam.Index == typeParam.Index
                        ? $"    {otherTypeParam.FieldName} = value;"
                        : $"    {otherTypeParam.FieldName} = {otherTypeParam.AsDefault};");
            }
                
            writer.AppendIndentedLine("}");
            writer.AppendLineBreak();
        }
    }

    private static bool TryLookupTypeParameter(
        TypeParameter[] typeParams,
        ITypeSymbol ctorParamType,
        [NotNullWhen(true)] out TypeParameter? foundTypeParameter)
    {
        foundTypeParameter = default;

        foreach (var typeParam in typeParams)
        {
            if (typeParam.TypeSymbol.Equals(ctorParamType, SymbolEqualityComparer.Default))
            {
                foundTypeParameter = typeParam;
                return true;
            }
        }

        return false;
    }
    
    private static TypeParameter[] Except(TypeParameter[] source, List<TypeParameter> subtract)
    {
        // NB! we expect that type parameters are unique, so we can use simple array comparison
        if (source.Length == subtract.Count)
        {
            return Array.Empty<TypeParameter>();
        }
        
        var result = new List<TypeParameter>(source.Length - subtract.Count);

        // - intentionally not using HashSet<>: we expect lower number of elements so looping should be still faster
        // - intentionally not using LINQ: to avoid additional (enumerator) allocations
        foreach (var sourceItem in source)
        {
            var found = false;
            foreach (var subtractItem in subtract)
            {
                if (subtractItem.TypeSymbol.Equals(sourceItem.TypeSymbol, SymbolEqualityComparer.Default))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                result.Add(sourceItem);
            }
        }

        return result.ToArray();
    }
}
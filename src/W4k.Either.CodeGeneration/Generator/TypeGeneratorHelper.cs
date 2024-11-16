using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using W4k.Either.TypeParametrization;

namespace W4k.Either.Generator;

internal static class TypeGeneratorHelper
{
    private static readonly string[] CandidateNames = { "TNew", "TBound" };

    public static string GetTypeName(string typeName, TypeParameter[] typeParameters, int replaceIndex, string replaceTypeParamName)
    {
        var sb = new StringBuilder();

        sb.Append(typeName);
        sb.Append('<');

        for (var i = 0; i < typeParameters.Length; i++)
        {
            var typeParameter = typeParameters[i];

            sb.Append(
                typeParameter.Index == replaceIndex
                    ? replaceTypeParamName
                    : typeParameter.AsArgument);

            if (i < typeParameters.Length - 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append('>');

        return sb.ToString();
    }

    public static string GetTypeParamName(TypeParameter[] typeParameters)
    {
        var candidateNames = CandidateNames;
        foreach (var candidate in candidateNames)
        {
            if (!IsTypeParamNamePresent(typeParameters, candidate))
            {
                return candidate;
            }
        }

        var baseName = candidateNames[0];
        var suffix = 1;
        while (true)
        {
            var generatedName = $"{baseName}{suffix:D}";
            if (!IsTypeParamNamePresent(typeParameters, generatedName))
            {
                return generatedName;
            }

            ++suffix;
        }
    }

    public static List<string> GetTypeParamConstraints(ITypeParameterSymbol typeParamSymbol)
    {
        var constraints = new List<string>();

        // 'notnull' constraint
        // (branching since it's not possible to combine `notnull` with `class`/`struct` constraints)
        if (typeParamSymbol.HasNotNullConstraint)
        {
            constraints.Add("notnull");
        }
        else
        {
            // 'class' / 'struct' constraints
            if (typeParamSymbol.HasReferenceTypeConstraint)
            {
                constraints.Add("class");
            }
            else if (typeParamSymbol.HasValueTypeConstraint)
            {
                constraints.Add("struct");
            }
            else if (typeParamSymbol.HasUnmanagedTypeConstraint)
            {
                constraints.Add("unmanaged");
            }
        }

        // constructor constraint 'new()'
        if (typeParamSymbol.HasConstructorConstraint)
        {
            constraints.Add("new()");
        }

        // type constraints
        foreach (var constraint in typeParamSymbol.ConstraintTypes)
        {
            constraints.Add(constraint.ToDisplayString());
        }

        return constraints;
    }

    private static bool IsTypeParamNamePresent(TypeParameter[] existingTypeParameters, string name)
    {
        for (var i = 0; i < existingTypeParameters.Length; i++)
        {
            if (existingTypeParameters[i].Name == name)
            {
                return true;
            }
        }

        return false;
    }
}
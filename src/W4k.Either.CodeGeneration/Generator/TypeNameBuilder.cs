using System.Text;
using W4k.Either.CodeGeneration.TypeParametrization;

namespace W4k.Either.CodeGeneration.Generator;

internal static class TypeNameBuilder
{
    private static readonly string[] CandidateNames = { "TNew", "TBound" };

    public static string GetTypeName(string typeName, TypeParameter[] typeParameters, int replaceIndex, string replaceTypeParamName)
    {
        var sb = new StringBuilder();

        sb.Append(typeName);
        sb.Append("<");
        
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
        
        sb.Append(">");
        
        return sb.ToString();
    }
    
    public static string GetUniqueTypeParamName(TypeParameter[] typeParameters)
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

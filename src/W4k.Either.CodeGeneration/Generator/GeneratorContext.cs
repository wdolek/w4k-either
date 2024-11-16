using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using W4k.Either.TypeDeclaration;
using W4k.Either.TypeParametrization;

namespace W4k.Either.Generator;

internal sealed class GeneratorContext
{
    private const string SkipMembersPropertyName = "Skip";

    public GeneratorContext(TransformationResult transformationResult)
    {
        if (!transformationResult.IsValid)
        {
            ThrowOnInvalidTransformationResult(nameof(transformationResult));
        }

        TypeKind = transformationResult.TypeKind;
        TypeDeclaration = transformationResult.TypeDeclaration;
        ContainingTypeDeclaration = transformationResult.ContainingTypeDeclaration;
        ParametrizationKind = transformationResult.ParametrizationKind;
        TypeParameters = transformationResult.TypeParameters;
        Skip = GetSkippedMembers(transformationResult.Attribute);
    }

    public TypeKind TypeKind { get; }
    public Declaration TypeDeclaration { get; }
    public Declaration? ContainingTypeDeclaration { get; }
    public ParametrizationKind ParametrizationKind { get; }
    public TypeParameter[] TypeParameters { get; }
    public HashSet<string> Skip { get; set; }

    public string GetFileName()
    {
        var typeSymbol = TypeDeclaration.TypeSymbol;

        var containingTypeDeclaration = ContainingTypeDeclaration;
        if (containingTypeDeclaration is not null)
        {
            return typeSymbol.IsGenericType
                ? $"{containingTypeDeclaration.TypeSymbol.Name}.{typeSymbol.Name}`{TypeParameters.Length}.g.cs"
                : $"{containingTypeDeclaration.TypeSymbol.Name}.{typeSymbol.Name}.g.cs";
        }

        return typeSymbol.IsGenericType
            ? $"{typeSymbol.Name}`{TypeParameters.Length}.g.cs"
            : $"{typeSymbol.Name}.g.cs";
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowOnInvalidTransformationResult(string argName) =>
        throw new ArgumentException("Transformation result is invalid.", argName);

    private static HashSet<string> GetSkippedMembers(AttributeData attr)
    {
        var skip = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var arg in attr.NamedArguments)
        {
            if (!string.Equals(arg.Key, SkipMembersPropertyName, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (var v in arg.Value.Values)
            {
                skip.Add((string)v.Value!);
            }

            break;
        }

        return skip;
    }
}
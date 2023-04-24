using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration.Processors;

internal static class AttributeTypeParamsProcessor
{
    public static (TypeParameter[] TypeParameters, Diagnostic? Diagnostic) GetAttributeTypeParameters(
        AttributeData attribute,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ctor = attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return (Array.Empty<TypeParameter>(), null);
        }

        var displayFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var attributeLocation = attribute.ApplicationSyntaxReference!.SyntaxTree.GetLocation(attribute.ApplicationSyntaxReference.Span);
        
        var ctorArguments = attribute.ConstructorArguments;

        var typeParams = new TypeParameter[ctorArguments.Length];
        var typeParamsSpan = typeParams.AsSpan();
        
        var isNullableEnabled = IsNullableReferenceTypesEnabled(semanticModel, attributeLocation);

        for (var i = 0; i < typeParams.Length; i++)
        {
            var arg = ctorArguments[i];
            if (arg.Value is not INamedTypeSymbol typeSymbol)
            {
                continue;
            }

            var typeParamName = typeSymbol.ToDisplayString(displayFormat);

            // use of `[Either(typeof(MyGenericType<>))]` is forbidden, type using open generics couldn't be generated
            if (IsUnboundGenericType(typeSymbol, attributeLocation, out var diagnostic))
            {
                return (Array.Empty<TypeParameter>(), diagnostic);
            }

            // check that types are unique, `[Either(typeof(int), typeof(int))]` is forbidden,
            // it wouldn't be possible to determine which field to use based on its type
            if (IsTypeUsed(typeParamsSpan.Slice(0, i), typeSymbol, attributeLocation, typeParamName, out diagnostic))
            {
                return (Array.Empty<TypeParameter>(), diagnostic);
            }

            typeParams[i] = new TypeParameter(
                index: i + 1,
                name: typeParamName,
                isReferenceType: typeSymbol.IsReferenceType,
                isValueType: typeSymbol.IsValueType,
                isNullable: IsTypeNullable(typeSymbol, isNullableEnabled));
        }

        return (typeParams, null);
    }

    private static bool IsNullableReferenceTypesEnabled(SemanticModel semanticModel, Location location)
    {
        var nullableContext = semanticModel.GetNullableContext(location.SourceSpan.Start);
        return nullableContext.AnnotationsEnabled();
    }    
    
    private static bool IsUnboundGenericType(INamedTypeSymbol typeSymbol, Location location, out Diagnostic? diagnostic)
    {
        if (typeSymbol.IsUnboundGenericType)
        {
            diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeParameterMustBeBound,
                location: location,
                messageArgs: typeSymbol.Name);

            return true;
        }

        diagnostic = null;
        return false;
    }

    private static bool IsTypeUsed(
        Span<TypeParameter> collectedParams,
        INamedTypeSymbol typeSymbol,
        Location location,
        string typeParamName,
        out Diagnostic? diagnostic)
    {
        foreach (var typeParameter in collectedParams)
        {
            if (typeParameter.Name != typeParamName)
            {
                continue;
            }
                      
            diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeMustBeUnique,
                location: location,
                messageArgs: typeSymbol.Name);
                        
            return true;
        }

        diagnostic = null;
        return false;
    }
    
    private static bool IsTypeNullable(INamedTypeSymbol typeSymbol, bool isNullableEnabled)
    {
        var isNullable = false;
        if (typeSymbol.IsReferenceType)
        {
            isNullable = !isNullableEnabled;
        }
        else if (typeSymbol.IsValueType)
        {
            isNullable = typeSymbol.IsGenericType && typeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T;
        }

        return isNullable;
    }
}

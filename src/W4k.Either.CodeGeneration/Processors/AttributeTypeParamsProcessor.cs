using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration.Processors;

internal static class AttributeTypeParamsProcessor
{
    private static readonly SymbolDisplayFormat DisplayFormat = new(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    public static ProcessorResult GetAttributeTypeParameters(ProcessorContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ctor = context.Attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return ProcessorResult.Empty();
        }

        var ctorArguments = context.Attribute.ConstructorArguments;

        var typeParams = new TypeParameter[ctorArguments.Length];
        var typeParamsSpan = typeParams.AsSpan();

        for (var i = 0; i < typeParams.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var arg = ctorArguments[i];
            if (arg.Value is not INamedTypeSymbol typeSymbol)
            {
                continue;
            }

            var typeParamName = typeSymbol.ToDisplayString(DisplayFormat);

            // use of `[Either(typeof(MyGenericType<>))]` is forbidden, type using open generics couldn't be generated
            if (typeSymbol.IsUnboundGenericType)
            {
                var unboundGenericTypeDiagnostic = Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.TypeParameterMustBeBound,
                    location: context.AttributeLocation,
                    messageArgs: typeSymbol.Name);

                return ProcessorResult.Failure(unboundGenericTypeDiagnostic);
            }

            // check that types are unique, `[Either(typeof(int), typeof(int))]` is forbidden,
            // it wouldn't be possible to determine which field to use based on its type
            var (isTypeUsed, typeUsedDiagnostic) = IsTypeUsed(typeParamsSpan.Slice(0, i), typeSymbol, context.AttributeLocation, typeParamName);
            if (isTypeUsed)
            {
                return ProcessorResult.Failure(typeUsedDiagnostic!);
            }

            typeParams[i] = new TypeParameter(
                index: i + 1,
                name: typeParamName,
                isReferenceType: typeSymbol.IsReferenceType,
                isValueType: typeSymbol.IsValueType,
                isNullable: IsTypeNullable(typeSymbol, context.IsNullRefTypeScopeEnabled));
        }

        return ProcessorResult.Success(typeParams);
    }

    private static (bool IsTypeUsed, Diagnostic? diagnostic) IsTypeUsed(
        Span<TypeParameter> collectedParams,
        INamedTypeSymbol typeSymbol,
        Location location,
        string typeParamName)
    {
        foreach (var typeParameter in collectedParams)
        {
            if (typeParameter.Name != typeParamName)
            {
                continue;
            }
                      
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TypeMustBeUnique,
                location: location,
                messageArgs: typeSymbol.Name);
                        
            return (true, diagnostic);
        }

        return (false, null);
    }
    
    private static bool IsTypeNullable(INamedTypeSymbol typeSymbol, bool isNullableEnabled)
    {
        // #nullable enable -> treat all reference types as nullable
        if (typeSymbol.IsReferenceType)
        {
            return !isNullableEnabled;
        }

        // check whether value type is nullable, using `Nullable<T>`
        if (typeSymbol.IsValueType)
        {
            return typeSymbol.IsGenericType && typeSymbol.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T;
        }

        return false;
    }
}

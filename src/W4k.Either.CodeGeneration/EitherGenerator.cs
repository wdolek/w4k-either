using System;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace W4k.Either.CodeGeneration;

[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    private const string EitherAttributeFullyQualifiedName = "W4k.Either.Abstractions.EitherAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<EitherStructGenerationContext> structsToGenerate = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                EitherAttributeFullyQualifiedName,
                static (node, _) => IsStructDeclarationSyntax(node),
                static (ctx, ct) => GetTypeToGenerate(ctx, ct))
            .Where(c => c is not null)!;
        
        context.RegisterSourceOutput(
            structsToGenerate,
            static (ctx, structToGenerate) => Execute(ctx, structToGenerate));
    }

    private static bool IsStructDeclarationSyntax(SyntaxNode node) =>
        node is StructDeclarationSyntax;

    private static EitherStructGenerationContext? GetTypeToGenerate(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        // type not available in current compilation
        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var targetName = typeSymbol.Name;
        var targetNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : typeSymbol.ContainingNamespace.ToDisplayString();

        // must be `partial struct`
        if (!IsPartial(typeSymbol, cancellationToken))
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.TypeMustBePartial,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // get type parameters
        var (attrTypeParams, attrDiagnostic) = GetAttributeTypeParameters(context.Attributes[0], context.SemanticModel, cancellationToken);
        if (attrDiagnostic is not null)
        {
            return EitherStructGenerationContext.Invalid(targetNamespace, targetName, attrDiagnostic);
        }
        
        var (targetTypeParams, genericsDiagnostic) = GetTargetTypeParameters(typeSymbol, cancellationToken);
        if (genericsDiagnostic is not null)
        {
            return EitherStructGenerationContext.Invalid(targetNamespace, targetName, genericsDiagnostic);
        }

        // user has not specified any type parameter
        if (attrTypeParams.Length == 0 && targetTypeParams.Length == 0)
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.NoTypeParameter,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        // user specified types both using attribute and making type generic
        if (attrTypeParams.Length > 0 && targetTypeParams.Length > 0)
        {
            return EitherStructGenerationContext.Invalid(
                targetNamespace,
                targetName,
                Diagnostic.Create(
                    descriptor: DiagnosticDescriptors.AmbiguousTypeParameters,
                    location: typeSymbol.Locations[0],
                    messageArgs: typeSymbol.Name));
        }

        return targetTypeParams.Length > 0
            ? EitherStructGenerationContext.Generic(targetNamespace, targetName, targetTypeParams)
            : EitherStructGenerationContext.NonGeneric(targetNamespace, targetName, attrTypeParams);
    }

    private static void Execute(SourceProductionContext context, EitherStructGenerationContext structToGenerate)
    {
        var diagnostics = structToGenerate.Diagnostics;
        if (diagnostics.Count > 0)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            return;
        }

        var sb = new StringBuilder(4096);
        EitherStructWriter.Write(structToGenerate, sb);

        context.AddSource(
            structToGenerate.FileName,
            SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private static bool IsPartial(INamedTypeSymbol namedTypeSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var syntaxRef in namedTypeSymbol.DeclaringSyntaxReferences)
        {
            var syntaxNode = syntaxRef.GetSyntax();
            if (syntaxNode is StructDeclarationSyntax structDeclaration)
            {
                if (structDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static (EitherStructGenerationContext.TypeParameter[] TypeParameters, Diagnostic? Diagnostic) GetAttributeTypeParameters(
        AttributeData attribute,
        SemanticModel semanticModel,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ctor = attribute.AttributeConstructor;
        if (ctor is null || ctor.Parameters.Length == 0)
        {
            return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), null);
        }

        var displayFormat = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
            miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        var attributeLocation = attribute.ApplicationSyntaxReference!.SyntaxTree.GetLocation(attribute.ApplicationSyntaxReference.Span);
        
        var ctorArguments = attribute.ConstructorArguments;

        var typeParams = new EitherStructGenerationContext.TypeParameter[ctorArguments.Length];
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
                return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), diagnostic);
            }

            // check that types are unique, `[Either(typeof(int), typeof(int))]` is forbidden,
            // it wouldn't be possible to determine which field to use based on its type
            if (IsTypeUsed(typeParamsSpan.Slice(0, i), typeSymbol, attributeLocation, typeParamName, out diagnostic))
            {
                return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), diagnostic);
            }

            typeParams[i] = new EitherStructGenerationContext.TypeParameter(
                index: i + 1,
                name: typeParamName,
                isReferenceType: typeSymbol.IsReferenceType,
                isValueType: typeSymbol.IsValueType,
                isNullable: IsTypeNullable(typeSymbol, isNullableEnabled));
        }

        return (typeParams, null);
    }

    private static (EitherStructGenerationContext.TypeParameter[] TypeParameters, Diagnostic? Diagnostic) GetTargetTypeParameters(
        INamedTypeSymbol typeSymbol,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (typeSymbol.Arity == 0)
        {
            return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), null);
        }

        if (typeSymbol.Arity < 2)
        {
            var diagnostic = Diagnostic.Create(
                descriptor: DiagnosticDescriptors.TooFewTypeParameters,
                location: typeSymbol.Locations[0],
                messageArgs: typeSymbol.Name);

            return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), diagnostic);
        }

        var typeParams = new EitherStructGenerationContext.TypeParameter[typeSymbol.TypeParameters.Length];

        for (var i = 0; i < typeParams.Length; i++)
        {
            var typeParam = typeSymbol.TypeParameters[i];
            var typeParamName = typeParam.Name;

            var isReferenceType = typeParam.HasReferenceTypeConstraint || typeParam.IsReferenceType;
            var isValueType = typeParam.HasValueTypeConstraint || typeParam.IsValueType;
            var isNullable = !typeParam.HasNotNullConstraint;

            typeParams[i] = new EitherStructGenerationContext.TypeParameter(
                index: i + 1,
                name: typeParamName,
                isReferenceType: isReferenceType,
                isValueType: isValueType,
                isNullable: isNullable);
        }

        return (typeParams, null);
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
        Span<EitherStructGenerationContext.TypeParameter> collectedParams,
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

    private static bool IsNullableReferenceTypesEnabled(SemanticModel semanticModel, Location location)
    {
        var nullableContext = semanticModel.GetNullableContext(location.SourceSpan.Start);
        return nullableContext.AnnotationsEnabled();
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

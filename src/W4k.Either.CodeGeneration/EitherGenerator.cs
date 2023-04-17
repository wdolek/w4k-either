using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using W4k.Either.Abstractions;

namespace W4k.Either.CodeGeneration;

[Generator]
public class EitherGenerator : IIncrementalGenerator
{
    private static readonly string EitherAttributeFullyQualifiedName = typeof(EitherAttribute).FullName!;

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
        var (attrTypeParams, attrDiagnostic) = GetAttributeTypeParameters(context.Attributes[0], cancellationToken);
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
        
        context.AddSource(CreateGeneratedFileName(structToGenerate), sb.ToString());
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

        var ctorArguments = attribute.ConstructorArguments;
        
        var typeParams = new EitherStructGenerationContext.TypeParameter[ctorArguments.Length];
        var typeParamsSpan = typeParams.AsSpan();

        for (var i = 0; i < typeParams.Length; i++)
        {
            var arg = ctorArguments[i];
            if (arg.Value is INamedTypeSymbol typeSymbol)
            {
                var typeParamName = typeSymbol.ToDisplayString(displayFormat);
                var attributeLocation =
                    attribute.ApplicationSyntaxReference?.SyntaxTree.GetLocation(attribute.ApplicationSyntaxReference.Span)
                    ?? typeSymbol.Locations[0];

                if (IsTypeUsed(typeParamsSpan.Slice(0, i), typeSymbol, attributeLocation, typeParamName, out var diagnostic))
                {
                    return (Array.Empty<EitherStructGenerationContext.TypeParameter>(), diagnostic);
                }

                typeParams[i] = new EitherStructGenerationContext.TypeParameter(
                    index: i + 1,
                    name: typeParamName,
                    isValueType: typeSymbol.IsValueType,
                    isNullable: false);
            }
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

            var isValueType = typeParam.HasValueTypeConstraint || typeParam.IsValueType;
            var isNullable = !isValueType && !typeParam.HasNotNullConstraint;

            typeParams[i] = new EitherStructGenerationContext.TypeParameter(
                index: i + 1,
                name: typeParamName,
                isValueType: isValueType,
                isNullable: isNullable);
        }

        return (typeParams, null);
    }

    private static bool IsTypeUsed(
        Span<EitherStructGenerationContext.TypeParameter> collectedParams,
        INamedTypeSymbol typeSymbol,
        Location location,
        string typeParamName,
        [NotNullWhen(true)] out Diagnostic? diagnostic)
    {
        foreach (var typeParameter in collectedParams)
        {
            if (typeParameter.ParameterName != typeParamName)
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

    private static string CreateGeneratedFileName(EitherStructGenerationContext structToGenerate) =>
        structToGenerate.IsGenericType
            ? $"{structToGenerate.TargetTypeName}`{structToGenerate.TypeParameters.Count}.g.cs"
            : $"{structToGenerate.TargetTypeName}.g.cs";
}

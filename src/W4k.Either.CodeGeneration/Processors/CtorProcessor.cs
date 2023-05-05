using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration.Processors;

internal static class CtorProcessor
{
    public static TypeConstructor[] CollectDeclaredConstructors(
        INamedTypeSymbol typeSymbol,
        TypeParameter[] typeParameters,
        CancellationToken cancellationToken)
    {
        if (typeSymbol.Constructors.Length == 0)
        {
            return Array.Empty<TypeConstructor>();
        }

        var declaredConstructors = new List<TypeConstructor>();
        foreach (var ctor in typeSymbol.Constructors)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // we generate either default constructor `ctor()` or constructor with single parameter `ctor(T)`; more constructor parameters
            // indicate that such constructor is declared by user - we can ignore it as it won't interfere with code generation
            if (ctor.Parameters.Length > 1)
            {
                continue;
            }

            if (ctor.Parameters.Length == 0)
            {
                AddDefaultCtorIfExplicitlyDeclared(declaredConstructors, ctor);
            }
            else
            {
                AddCtorIfTypeRecognized(declaredConstructors, ctor, typeParameters);
            }
        }

        return declaredConstructors.ToArray();
    }

    private static void AddDefaultCtorIfExplicitlyDeclared(List<TypeConstructor> declaredConstructors, IMethodSymbol ctor)
    {
        if (!ctor.IsImplicitlyDeclared)
        {
            declaredConstructors.Add(TypeConstructor.Parameterless);
        }
    }

    private static void AddCtorIfTypeRecognized(
        List<TypeConstructor> declaredConstructors,
        IMethodSymbol ctor,
        TypeParameter[] typeParameters)
    {
        var ctorParamType = ctor.Parameters[0].Type;
        if (IsParamTypeRecognized(typeParameters, ctorParamType))
        {
            declaredConstructors.Add(TypeConstructor.WithParameter(ctorParamType.Name));
        }
    }

    private static bool IsParamTypeRecognized(TypeParameter[] typeParameters, ITypeSymbol paramType)
    {
        foreach (var typeParam in typeParameters)
        {
            if (typeParam.Name == paramType.Name)
            {
                return true;
            }
        }

        return false;
    }
}

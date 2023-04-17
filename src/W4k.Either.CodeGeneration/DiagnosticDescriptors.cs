﻿using Microsoft.CodeAnalysis;

namespace W4k.Either.CodeGeneration;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor TypeMustBePartial = new(
        id: "W4KE001",
        title: "Type must be partial",
        messageFormat: "The type '{0}' must be partial",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor TooFewTypeParameters = new(
        id: "W4KE002",
        title: "Too few type parameters",
        messageFormat: "The type {0} must have at least two type parameters",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor AmbiguousTypeParameters = new(
        id: "W4KE003",
        title: "Ambiguous type parameters",
        messageFormat: "The type {0} is both generic and has types specified in attribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor NoTypeParameter = new(
        id: "W4KE004",
        title: "No type parameters",
        messageFormat: "The type {0} is missing type parameter, either use attribute or use generics",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    public static readonly DiagnosticDescriptor TypeMustBeUnique = new(
        id: "W4KE005",
        title: "Using one type twice is not allowed",
        messageFormat: "The type {0} is already used",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    public static readonly DiagnosticDescriptor TypeParameterMustBeBound = new(
        id: "W4KE006",
        title: "Using open generics is not allowed",
        messageFormat: "The type {0} must have all type parameters bound",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
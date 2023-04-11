// ReSharper disable CheckNamespace

// https://github.com/dotnet/runtime/blob/v7.0.0/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/CallerArgumentExpressionAttribute.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices
{
#if !NET6_0_OR_GREATER

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }

#endif
}

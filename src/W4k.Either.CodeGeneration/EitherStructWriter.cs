﻿using System.Text;
using W4k.Either.CodeGeneration.Context;

namespace W4k.Either.CodeGeneration;

internal static class EitherStructWriter
{
    public static void Write(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeName = CreateTypeName(context);
        
        WriteFileHeader(sb);
        WriteUsing(sb);
        StartNamespace(context, sb);
        StartTypeDeclaration(sb, typeName);
        WriteFields(context, sb);
        WriteConstructors(context, sb);
        WriteProperties(context, sb);
        WriteOperators(context, sb, typeName);
        WriteObjectOverrides(context, sb, typeName);
        WriteEquatableEquals(context, sb, typeName);
        WriteGetObjectData(context, sb);
        WriteTryPick(context, sb);
        WriteMatch(context, sb);
        WriteMatchWithState(context, sb);
        WriteAsyncMatch(context, sb);
        WriteAsyncMatchWithState(context, sb);
        WriteSwitch(context, sb);
        WriteSwitchWithState(context, sb);
        WriteAsyncSwitch(context, sb);
        WriteAsyncSwitchWithState(context, sb);
        EndTypeDeclaration(sb);
        EndNamespace(sb);
    }

    private static string CreateTypeName(EitherStructGenerationContext context)
    {
        if (!context.IsGenericType)
        {
            return context.TargetTypeName;
        }

        var typeParams = context.TypeParameters;
        var sb = new StringBuilder(64);

        sb.Append(context.TargetTypeName);
        sb.Append("<");

        for (var i = 0; i < typeParams.Count; i++)
        {
            sb.Append(typeParams[i].Name);
            if (i < typeParams.Count - 1)
            {
                sb.Append(", ");
            }
        }
        
        sb.Append(">");

        return sb.ToString();
    }

    private static void WriteFileHeader(StringBuilder sb)
    {
        sb.AppendLine("// <auto-generated />");
        sb.AppendLine();
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
    }

    private static void WriteUsing(StringBuilder sb)
    {
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
        sb.AppendLine("using System.Diagnostics.Contracts;");
        sb.AppendLine("using System.Runtime.Serialization;");
        sb.AppendLine("using System.Threading;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("using W4k.Either.Abstractions;");
        sb.AppendLine();
    }

#region Namespace

    private static void StartNamespace(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.Append("namespace ");
        sb.AppendLine(context.TargetNamespace);
        sb.AppendLine("{");
    }

    private static void EndNamespace(StringBuilder sb)
    {
        sb.AppendLine("}");
    }

#endregion
    
#region Type declaration

    private static void StartTypeDeclaration(StringBuilder sb, string typeName)
    {
        sb.AppendLine("    [Serializable]");
        sb.AppendLine($"    readonly partial struct {typeName} : IEquatable<{typeName}>, ISerializable");
        sb.AppendLine("    {");
    }

    private static void EndTypeDeclaration(StringBuilder sb)
    {
        sb.AppendLine("    }");
    }
    
#endregion

#region Fields

    private static void WriteFields(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine("        private readonly byte _idx;");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"        private readonly {typeParam.AsFieldType} {typeParam.FieldName};");
        }

        sb.AppendLine();
    }
    
#endregion

#region Constructors

    private static void WriteConstructors(EitherStructGenerationContext context, StringBuilder sb)
    {
        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"        public {context.TargetTypeName}({typeParam.AsArgument} value)");
            sb.AppendLine("        {");

            if (typeParam.IsNonNullableReferenceType)
            {
                sb.AppendLine("            ThrowHelper.ThrowIfNull(value);");                
            }

            sb.AppendLine($"            _idx = {typeParam.Index};");
            sb.AppendLine($"            {typeParam.FieldName} = value;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        WriteSerializableConstructor(context, sb);
    }

    private static void WriteSerializableConstructor(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine($"        private {context.TargetTypeName}(SerializationInfo info, StreamingContext context)");
        sb.AppendLine("        {");
        sb.AppendLine("            _idx = info.GetByte(nameof(_idx));");
        sb.AppendLine("            switch (_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            // specifying type using attribute: for value types we want to avoid "Unboxing a possibly null value" warning
            var nullForgiving = typeParam.IsValueType && !typeParam.IsNullable
                ? "!"
                : string.Empty;
            
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    {typeParam.FieldName} = ({typeParam.AsFieldType})info.GetValue(\"{typeParam.FieldName}\", typeof({typeParam.Name})){nullForgiving};");
            sb.AppendLine("                    break;");
        }
        
        sb.AppendLine("                default:");
        sb.AppendLine("                    ThrowHelper.ThrowOnInvalidState();");
        sb.AppendLine("                    break;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
#endregion

#region Properties

    private static void WriteProperties(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine("        [Pure]");
        sb.AppendLine("        public object? Case");
        sb.AppendLine("        {");
        sb.AppendLine("            get");
        sb.AppendLine("            {");
        sb.AppendLine("                switch (_idx)");
        sb.AppendLine("                {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                    case {typeParam.Index}:");
            sb.AppendLine($"                        return {typeParam.FieldName};");            
        }

        sb.AppendLine("                    default:");
        sb.AppendLine("                        return ThrowHelper.ThrowOnInvalidState<object?>();");
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

#endregion

#region Operators

    private static void WriteOperators(EitherStructGenerationContext context, StringBuilder sb, string typeName)
    {
        sb.AppendLine("        [Pure]");    
        sb.AppendLine($"        public static bool operator ==({typeName} left, {typeName} right) => left.Equals(right);");
        sb.AppendLine();

        sb.AppendLine("        [Pure]");
        sb.AppendLine($"        public static bool operator !=({typeName} left, {typeName} right) => !left.Equals(right);");
        sb.AppendLine();

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine("        [Pure]");
            sb.AppendLine($"        public static implicit operator {typeName}({typeParam.AsArgument} value) => new(value);");
            sb.AppendLine();
        }
    }

#endregion

#region Object overrides

    private static void WriteObjectOverrides(EitherStructGenerationContext context, StringBuilder sb, string typeName)
    {
        WriteGetHashCode(context, sb);
        WriteToString(context, sb);
        WriteObjectEquals(sb, typeName);
    }

    private static void WriteGetHashCode(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine("        [Pure]");
        sb.AppendLine("        public override int GetHashCode()");
        sb.AppendLine("        {");
        sb.AppendLine("            switch (_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine(
                typeParam.IsNullable
                    ? $"                    return {typeParam.AsFieldInvoker}.GetHashCode() ?? 0;"
                    : $"                    return {typeParam.AsFieldInvoker}.GetHashCode();");
        }
        
        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<int>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteToString(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine("        [Pure]");
        sb.AppendLine("        public override string ToString()");
        sb.AppendLine("        {");
        sb.AppendLine("            switch (_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    return {typeParam.AsFieldInvoker}.ToString() ?? string.Empty;");
        }
        
        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<string>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

    private static void WriteObjectEquals(StringBuilder sb, string typeName)
    {
        sb.AppendLine("        [Pure]");
        sb.AppendLine("        public override bool Equals(object? obj)");
        sb.AppendLine("        {");
        sb.AppendLine($"            if (obj is not {typeName} other)");
        sb.AppendLine("            {");
        sb.AppendLine("                return false;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine("            return Equals(other);");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

#endregion

#region IEquatable

    private static void WriteEquatableEquals(EitherStructGenerationContext context, StringBuilder sb, string typeName)
    {
        sb.AppendLine("        [Pure]");
        sb.AppendLine($"        public bool Equals({typeName} other)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (_idx != other._idx)");
        sb.AppendLine("            {");
        sb.AppendLine("                return false;");
        sb.AppendLine("            }");
        sb.AppendLine();
        sb.AppendLine("            switch (_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine(
                typeParam.IsNullable
                    ? $"                    return {typeParam.AsFieldInvoker}.Equals(other.{typeParam.FieldName}) ?? false;"
                    : $"                    return {typeParam.AsFieldInvoker}.Equals(other.{typeParam.FieldName});");
        }
        
        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<bool>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

#endregion

#region ISerializable

    private static void WriteGetObjectData(EitherStructGenerationContext context, StringBuilder sb)
    {
        sb.AppendLine("        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)");
        sb.AppendLine("        {");
        sb.AppendLine("            info.AddValue(\"_idx\", _idx);");
        sb.AppendLine("            switch (_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    info.AddValue(\"{typeParam.FieldName}\", {typeParam.FieldName});");
            sb.AppendLine("                    break;");
        }
        
        sb.AppendLine("                default:");
        sb.AppendLine("                    ThrowHelper.ThrowOnInvalidState();");
        sb.AppendLine("                    break;");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

#endregion

    private static void WriteTryPick(EitherStructGenerationContext context, StringBuilder sb)
    {
        foreach (var typeParam in context.TypeParameters)
        {
            var notNullWhenTrue = typeParam.IsNonNullableReferenceType
                ? "[NotNullWhen(true)] "
                : string.Empty;

            sb.AppendLine("        [Pure]");
            sb.AppendLine($"        public bool TryPick({notNullWhenTrue}out {typeParam.AsFieldType} value)");
            sb.AppendLine("        {");
            sb.AppendLine($"            if (_idx == {typeParam.Index})");
            sb.AppendLine("            {");
            sb.AppendLine($"                value = {typeParam.AsFieldReceiver};");
            sb.AppendLine("                return true;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine($"            value = {typeParam.AsDefault};");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }
    }

#region Match
    
    private static void WriteMatch(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;

        sb.AppendLine("        public TResult Match<TResult>(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            sb.Append($"            Func<{typeParam.AsArgument}, TResult> f{typeParam.Index}");
            sb.AppendLine(
                typeParam.Index < arity
                    ? ","
                    : ")");
        }
        
        sb.AppendLine("        {");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"            ThrowHelper.ThrowIfNull(f{i});");
        }

        sb.AppendLine();
        
        // switch
        sb.AppendLine("            switch(_idx)");
        sb.AppendLine("            {");

        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    return f{typeParam.Index}({typeParam.AsFieldReceiver});");
        }

        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<TResult>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteMatchWithState(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;

        sb.AppendLine("        public TResult Match<TState, TResult>(");

        // parameters
        sb.AppendLine("            TState state,");
        foreach (var typeParam in typeParams)
        {
            sb.Append($"            Func<TState, {typeParam.AsArgument}, TResult> f{typeParam.Index}");
            sb.AppendLine(
                typeParam.Index < arity
                    ? ","
                    : ")");
        }        
        
        sb.AppendLine("        {");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"            ThrowHelper.ThrowIfNull(f{i});");
        }

        sb.AppendLine();
        
        // switch
        sb.AppendLine("            switch(_idx)");
        sb.AppendLine("            {");
        
        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    return f{typeParam.Index}(state, {typeParam.AsFieldReceiver});");
        }        

        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<TResult>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
#endregion

#region MatchAsync

    private static void WriteAsyncMatch(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;

        sb.AppendLine("        public Task<TResult> MatchAsync<TResult>(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            sb.AppendLine($"            Func<{typeParam.AsArgument}, CancellationToken, Task<TResult>> f{typeParam.Index},");
        }

        sb.AppendLine("            CancellationToken cancellationToken = default)");
        sb.AppendLine("        {");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"            ThrowHelper.ThrowIfNull(f{i});");
        }

        sb.AppendLine();
        
        // switch
        sb.AppendLine("            switch(_idx)");
        sb.AppendLine("            {");
        
        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    return f{typeParam.Index}({typeParam.AsFieldReceiver}, cancellationToken);");
        }        

        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteAsyncMatchWithState(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;

        sb.AppendLine("        public Task<TResult> MatchAsync<TState, TResult>(");

        // parameters
        sb.AppendLine("            TState state,");
        foreach (var typeParam in typeParams)
        {
            sb.AppendLine($"            Func<TState, {typeParam.AsArgument}, CancellationToken, Task<TResult>> f{typeParam.Index},");
        }

        sb.AppendLine("            CancellationToken cancellationToken = default)");
        sb.AppendLine("        {");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"            ThrowHelper.ThrowIfNull(f{i});");
        }

        sb.AppendLine();
        
        // switch
        sb.AppendLine("            switch(_idx)");
        sb.AppendLine("            {");
        
        foreach (var typeParam in context.TypeParameters)
        {
            sb.AppendLine($"                case {typeParam.Index}:");
            sb.AppendLine($"                    return f{typeParam.Index}(state, {typeParam.AsFieldReceiver}, cancellationToken);");
        }

        sb.AppendLine("                default:");
        sb.AppendLine("                    return ThrowHelper.ThrowOnInvalidState<Task<TResult>>();");
        sb.AppendLine("            }");
        sb.AppendLine("        }");
        sb.AppendLine();
    }

#endregion

#region Switch (all)

    private static void WriteSwitch(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;
        
        sb.AppendLine("        public void Switch(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            sb.Append($"            Action<{typeParam.AsArgument}> a{typeParam.Index}");
            sb.AppendLine(
                typeParam.Index < arity
                    ? ","
                    : ")");
        }        
        
        sb.AppendLine("        {");
        sb.AppendLine("            Match(");

        for (var i = 1; i <= arity; i++)
        {
            sb.Append($"                v => {{ a{i}(v); return Unit.Default; }}");
            sb.AppendLine(
                i < arity
                    ? ","
                    : ");");
        }

        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteSwitchWithState(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;
        
        sb.AppendLine("        public void Switch<TState>(");

        // parameters
        sb.AppendLine("            TState state,");
        foreach (var typeParam in typeParams)
        {
            sb.Append($"            Action<TState, {typeParam.AsArgument}> a{typeParam.Index}");
            sb.AppendLine(
                typeParam.Index < arity
                    ? ","
                    : ")");
        }      
        
        sb.AppendLine("        {");
        sb.AppendLine("            Match(");
        sb.AppendLine("                state,");

        for (var i = 1; i <= arity; i++)
        {
            sb.Append($"                (s, v) => {{ a{i}(s, v); return Unit.Default; }}");
            sb.AppendLine(
                i < arity
                    ? ","
                    : ");");
        }

        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteAsyncSwitch(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;
        
        sb.AppendLine("        public Task SwitchAsync(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            sb.AppendLine($"            Func<{typeParam.AsArgument}, CancellationToken, Task> a{typeParam.Index},");
        }

        sb.AppendLine("            CancellationToken cancellationToken = default)");
        sb.AppendLine("        {");
        sb.AppendLine("            return MatchAsync(");

        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"                async (v, ct) => {{ await a{i}(v, ct); return Unit.Default; }},");
        }

        sb.AppendLine("                cancellationToken);");
        sb.AppendLine("        }");
        sb.AppendLine();
    }
    
    private static void WriteAsyncSwitchWithState(EitherStructGenerationContext context, StringBuilder sb)
    {
        var typeParams = context.TypeParameters;
        var arity = typeParams.Count;
        
        sb.AppendLine("        public Task SwitchAsync<TState>(");

        // parameters
        sb.AppendLine("            TState state,");
        foreach (var typeParam in typeParams)
        {
            sb.AppendLine($"            Func<TState, {typeParam.AsArgument}, CancellationToken, Task> a{typeParam.Index},");
        }

        sb.AppendLine("            CancellationToken cancellationToken = default)");
        sb.AppendLine("        {");
        sb.AppendLine("            return MatchAsync(");
        sb.AppendLine("                state,");

        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($"                async (s, v, ct) => {{ await a{i}(s, v, ct); return Unit.Default; }},");
        }

        sb.AppendLine("                cancellationToken);");
        sb.AppendLine("        }");
    }

#endregion
}

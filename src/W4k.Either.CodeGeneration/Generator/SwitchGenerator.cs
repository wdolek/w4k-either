﻿namespace W4k.Either.CodeGeneration.Generator;

internal sealed class SwitchGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public SwitchGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter writer)
    {
        WriteSwitch(writer);
        WriteSwitchWithState(writer);
        WriteAsyncSwitch(writer);
        WriteAsyncSwitchWithState(writer);
    }

    private void WriteSwitch(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;
        
        writer.AppendIndentedLine("public void Switch(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndented($"    global::System.Action<{typeParam.AsArgument}> a{typeParam.Index}");
            writer.Append(
                typeParam.Index < arity
                    ? ","
                    : ")");
            writer.AppendLineBreak();
        }        
        
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    Match(");

        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndented($"        v => {{ a{i}(v); return global::W4k.Either.Unit.Default; }}");
            writer.Append(
                i < arity
                    ? ","
                    : ");");
            writer.AppendLineBreak();
        }

        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteSwitchWithState(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;
        
        writer.AppendIndentedLine("public void Switch<TState>(");

        // parameters
        writer.AppendIndentedLine("    TState state,");
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndented($"    global::System.Action<TState, {typeParam.AsArgument}> a{typeParam.Index}");
            writer.Append(
                typeParam.Index < arity
                    ? ","
                    : ")");
            writer.AppendLineBreak();
        }      
        
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    Match(");
        writer.AppendIndentedLine("        state,");

        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndented($"        (s, v) => {{ a{i}(s, v); return global::W4k.Either.Unit.Default; }}");
            writer.Append(
                i < arity
                    ? ","
                    : ");");
            writer.AppendLineBreak();
        }

        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteAsyncSwitch(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;
        
        writer.AppendIndentedLine("public global::System.Threading.Tasks.Task SwitchAsync(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"    global::System.Func<{typeParam.AsArgument}, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> a{typeParam.Index},");
        }

        writer.AppendIndentedLine("    global::System.Threading.CancellationToken cancellationToken = default)");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    return MatchAsync(");

        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"        async (v, ct) => {{ await a{i}(v, ct); return global::W4k.Either.Unit.Default; }},");
        }

        writer.AppendIndentedLine("        cancellationToken);");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteAsyncSwitchWithState(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;
        
        writer.AppendIndentedLine("public global::System.Threading.Tasks.Task SwitchAsync<TState>(");

        // parameters
        writer.AppendIndentedLine("    TState state,");
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"    global::System.Func<TState, {typeParam.AsArgument}, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task> a{typeParam.Index},");
        }

        writer.AppendIndentedLine("    global::System.Threading.CancellationToken cancellationToken = default)");
        writer.AppendIndentedLine("{");
        writer.AppendIndentedLine("    return MatchAsync(");
        writer.AppendIndentedLine("        state,");

        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"        async (s, v, ct) => {{ await a{i}(s, v, ct); return global::W4k.Either.Unit.Default; }},");
        }

        writer.AppendIndentedLine("        cancellationToken);");
        writer.AppendIndentedLine("}");
    }
}

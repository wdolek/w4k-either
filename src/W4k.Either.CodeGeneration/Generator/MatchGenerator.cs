namespace W4k.Either.CodeGeneration.Generator;

internal class MatchGenerator : IMemberCodeGenerator
{
    private readonly GeneratorContext _context;

    public MatchGenerator(GeneratorContext context)
    {
        _context = context;
    }

    public bool CanGenerate() => true;

    public void Generate(IndentedWriter writer)
    {
        WriteMatch(writer);
        WriteMatchWithState(writer);
        WriteAsyncMatch(writer);
        WriteAsyncMatchWithState(writer);
    }
    
    private void WriteMatch(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;

        writer.AppendIndentedLine("public TResult Match<TResult>(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndented($"    global::System.Func<{typeParam.AsArgument}, TResult> f{typeParam.Index}");
            writer.Append(
                typeParam.Index < arity
                    ? ","
                    : ")");
            
            writer.AppendLineBreak();
        }
        
        writer.AppendIndentedLine("{");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"    global::System.ArgumentNullException.ThrowIfNull(f{i});");
        }

        writer.AppendLineBreak();
        
        // switch
        writer.AppendIndentedLine("    switch(_idx)");
        writer.AppendIndentedLine("    {");

        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            return f{typeParam.Index}({typeParam.AsFieldReceiver});");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<TResult>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteMatchWithState(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;

        writer.AppendIndentedLine("public TResult Match<TState, TResult>(");

        // parameters
        writer.AppendIndentedLine("    TState state,");
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndented($"    global::System.Func<TState, {typeParam.AsArgument}, TResult> f{typeParam.Index}");
            writer.Append(
                typeParam.Index < arity
                    ? ","
                    : ")");
            writer.AppendLineBreak();
        }        
        
        writer.AppendIndentedLine("{");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"    global::System.ArgumentNullException.ThrowIfNull(f{i});");
        }

        writer.AppendLineBreak();
        
        // switch
        writer.AppendIndentedLine("    switch(_idx)");
        writer.AppendIndentedLine("    {");
        
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            return f{typeParam.Index}(state, {typeParam.AsFieldReceiver});");
        }        

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<TResult>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteAsyncMatch(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;

        writer.AppendIndentedLine("public global::System.Threading.Tasks.Task<TResult> MatchAsync<TResult>(");

        // parameters
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"    global::System.Func<{typeParam.AsArgument}, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TResult>> f{typeParam.Index},");
        }

        writer.AppendIndentedLine("    global::System.Threading.CancellationToken cancellationToken = default)");
        writer.AppendIndentedLine("{");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"    global::System.ArgumentNullException.ThrowIfNull(f{i});");
        }

        writer.AppendLineBreak();
        
        // switch
        writer.AppendIndentedLine("    switch(_idx)");
        writer.AppendIndentedLine("    {");
        
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            return f{typeParam.Index}({typeParam.AsFieldReceiver}, cancellationToken);");
        }        

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<global::System.Threading.Tasks.Task<TResult>>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
    
    private void WriteAsyncMatchWithState(IndentedWriter writer)
    {
        var typeParams = _context.TypeParameters;
        var arity = typeParams.Length;

        writer.AppendIndentedLine("public global::System.Threading.Tasks.Task<TResult> MatchAsync<TState, TResult>(");

        // parameters
        writer.AppendIndentedLine("    TState state,");
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"    global::System.Func<TState, {typeParam.AsArgument}, global::System.Threading.CancellationToken, global::System.Threading.Tasks.Task<TResult>> f{typeParam.Index},");
        }

        writer.AppendIndentedLine("    global::System.Threading.CancellationToken cancellationToken = default)");
        writer.AppendIndentedLine("{");

        // null checks
        for (var i = 1; i <= arity; i++)
        {
            writer.AppendIndentedLine($"    global::System.ArgumentNullException.ThrowIfNull(f{i});");
        }

        writer.AppendLineBreak();
        
        // switch
        writer.AppendIndentedLine("    switch(_idx)");
        writer.AppendIndentedLine("    {");
        
        foreach (var typeParam in typeParams)
        {
            writer.AppendIndentedLine($"        case {typeParam.Index}:");
            writer.AppendIndentedLine($"            return f{typeParam.Index}(state, {typeParam.AsFieldReceiver}, cancellationToken);");
        }

        writer.AppendIndentedLine("        default:");
        writer.AppendIndentedLine("            return global::W4k.Either.ThrowHelper.ThrowOnInvalidState<global::System.Threading.Tasks.Task<TResult>>();");
        writer.AppendIndentedLine("    }");
        writer.AppendIndentedLine("}");
        writer.AppendLineBreak();
    }
}

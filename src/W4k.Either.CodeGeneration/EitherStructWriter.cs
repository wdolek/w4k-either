using System.Text;

namespace W4k.Either.CodeGeneration;

internal readonly struct EitherStructWriter
{
    private readonly EitherStructGenerationContext _context;
    private readonly StringBuilder _stringBuilder;

    private readonly string _typeName;

    public EitherStructWriter(EitherStructGenerationContext context, StringBuilder stringBuilder)
    {
        _context = context;
        _stringBuilder = stringBuilder;

        var typeParams = string.Join(", ", _context.TypeParameters);
        _typeName = _context.IsGenericType
            ? $"{_context.TargetTypeName}<{typeParams}>"
            : _context.TargetTypeName;
    }

    public void Write()
    {
        var typeParameters = _context.TypeParameters;

        // Using directives
        _stringBuilder.AppendLine("using System;");
        _stringBuilder.AppendLine("using System.Diagnostics.CodeAnalysis;");
        _stringBuilder.AppendLine("using System.Diagnostics.Contracts;");
        _stringBuilder.AppendLine("using System.Runtime.Serialization;");
        _stringBuilder.AppendLine("using System.Threading;");
        _stringBuilder.AppendLine("using System.Threading.Tasks;");
        _stringBuilder.AppendLine("using W4k.Either.Abstractions;");
        _stringBuilder.AppendLine();

        // Namespace
        _stringBuilder.AppendLine($"namespace {_context.TargetNamespace}");
        _stringBuilder.AppendLine("{");

        // Struct declaration
        _stringBuilder.Append($"partial readonly struct {_typeName}");

        if (_context.IsGenericType)
        {
            _stringBuilder.Append($" : IEquatable<{_typeName}>, ISerializable");
            _stringBuilder.AppendLine();

            for (var i = 0; i < typeParameters.Count; i++)
            {
                _stringBuilder.AppendLine($"    where {typeParameters[i]} : notnull");
            }
        }
        else
        {
            _stringBuilder.Append($" : IEquatable<{_typeName}>, ISerializable");
            _stringBuilder.AppendLine();
        }

        _stringBuilder.AppendLine("{");

        // Fields
        _stringBuilder.AppendLine("    private readonly byte _idx;");
        for (var i = 0; i < _context.TypeParameters.Count; i++)
        {
            _stringBuilder.AppendLine($"    private readonly {_context.TypeParameters[i]}? _v{i};");
        }

        _stringBuilder.AppendLine();

        // The rest of the code should be adapted based on the context.
        // You need to modify the original code to handle different numbers of type parameters
        // and adjust the code for the non-generic case.

        // ...
        // (Adapt the rest of the code here, with necessary modifications.)

        _stringBuilder.AppendLine("}");
    }
}

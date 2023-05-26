using System.Text;

namespace W4k.Either.CodeGeneration.Generator;

internal readonly struct IndentedWriter
{
    private readonly StringBuilder _sb;
    private readonly string _indentation;

    public IndentedWriter(StringBuilder sb) 
        : this(sb, 0)
    {
    }

    private IndentedWriter(StringBuilder sb, int indent)
    {
        _sb = sb;
        _indentation = indent == 0
            ? ""
            : new string(' ', indent);
    }

    public void Append(string value) => _sb.Append(value);

    public void AppendIndented(string value)
    {
        _sb.Append(_indentation);
        _sb.Append(value);
    }
    
    public void AppendLineBreak() => _sb.AppendLine();

    public void AppendIndentedLine(string line)
    {
        _sb.Append(_indentation);
        _sb.AppendLine(line);
    }
    
    public IndentedWriter Indent() => new(_sb, _indentation.Length + 4);
}

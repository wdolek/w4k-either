using System.Text;

namespace W4k.Either.Generator;

internal readonly struct IndentedWriter
{
    private readonly StringBuilder _sb;
    private readonly int _indentBy;
    private readonly int _level;
    private readonly string _indentation;

    public IndentedWriter(StringBuilder sb, int indentBy = 4)
        : this(sb, indentBy, 0)
    {
    }

    private IndentedWriter(StringBuilder sb, int indentBy, int level)
    {
        _sb = sb;
        _indentBy = indentBy;
        _level = level;

        _indentation = level == 0
            ? ""
            : new string(' ', indentBy * level);
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

    public void RemoveLastLineBreak()
    {
        var penultimatePos = _sb.Length - 2;
        var previousPos = _sb.Length - 1;

        if (_sb.Length >= 2 && _sb[penultimatePos] == '\r' && _sb[previousPos] == '\n')
        {
            _sb.Remove(penultimatePos, 2);
        }
        else if (_sb.Length >= 1 && _sb[previousPos] == '\n')
        {
            _sb.Remove(previousPos, 1);
        }
    }
    
    public IndentedWriter Indent() => new(_sb, _indentBy, _level + 1);
}

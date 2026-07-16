namespace Compiler.utils;

public struct Position
{
    private int _line,
        _column;
    private readonly string _fileName;

    public Position(int line, int column, string fileName)
    {
        _line = line;
        _column = column;
        _fileName = fileName;
    }

    public void Advance(char currentChar)
    {
        _column++;
        if (currentChar == '\n')
        { //TODO: Handle \r\n
            _line++;
            _column = 0;
        }
    }

    public override string ToString() => $"{_fileName}:{_line + 1}:{_column + 1}";
}

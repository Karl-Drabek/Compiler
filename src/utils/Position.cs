namespace Compiler.utils;

/// <summary>
/// Represents a position in the source code, including the line, column, and file name.
/// </summary>
public struct Position
{
    private int _line,
        _column;
    private readonly string _fileName;

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> struct with the specified line, column, and file name.
    /// </summary>
    /// <param name="line">The line number in the source code.</param>
    /// <param name="column">The column number in the source code.</param>
    /// <param name="fileName">The name of the source file.</param>
    public Position(int line, int column, string fileName)
    {
        _line = line;
        _column = column;
        _fileName = fileName;
    }
/// <summary>
/// Advances the position by one column, and if the current character is a newline, increments the line number and resets the column to zero.
/// </summary>
/// <param name="currentChar">The current character being processed, used to determine if a newline has occurred.</param>
    public void Advance(char currentChar)
    {
        _column++;
        if (currentChar == '\n')
        { //TODO: Handle \r\n. It is not clear if this is necessary, so use some reasoning to decide
            _line++;
            _column = 0;
        }
    }

    public override string ToString() => $"{_fileName}:{_line + 1}:{_column + 1}";
}

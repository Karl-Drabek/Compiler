namespace Compiler.utils;

/// <summary>
/// Represents an error encountered during compilation, including its location and details.
/// </summary>
public class Error
{
    internal Position posStart,
        posEnd;
    internal string errorName,
        details;
/// <summary>
/// Initializes a new instance of the <see cref="Error"/> class with the specified start and end positions, error name, and details.
/// </summary>
/// <param name="posStart">The starting position of the error in the source code.</param>
/// <param name="posEnd">The ending position of the error in the source code.</param>
/// <param name="errorName">The name of the error.</param>
/// <param name="details">The details describing the error.</param>
    public Error(
        Position posStart,
        Position posEnd,
        string errorName = "Error",
        string details = "Something went wrong"
    )
    {
        this.posStart = posStart;
        this.posEnd = posEnd;
        this.errorName = errorName;
        this.details = details;
    }

    public override string ToString() => $"{errorName}: {details}; ({posStart} to {posEnd}).";
}

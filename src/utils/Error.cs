namespace Compiler.utils;

public class Error
{
    internal Position posStart,
        posEnd;
    internal string errorName,
        details;

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

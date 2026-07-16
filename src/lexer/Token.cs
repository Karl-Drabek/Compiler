using Compiler.utils;

namespace Compiler.Lexer;

public enum TokenType
{
    Int,
    String,
    Double,
    Bool,
    Func,
    IntType,
    StringType,
    DoubleType,
    BoolType,
    Plus,
    Minus,
    Multiply,
    Divide,
    Power,
    EQ,
    EPlus,
    EMinus,
    EMult,
    EDiv,
    EPow,
    PP,
    MM,
    LParen,
    RParen,
    LBrace,
    RBrace,
    LBrack,
    RBrack,
    EE,
    NE,
    LT,
    GT,
    LTE,
    GTE,
    Comma,
    SC,
    Point,
    Collon,
    And,
    Or,
    Not,
    If,
    Elif,
    Try,
    Eltry,
    Else,
    Repeat,
    Loop,
    While,
    Return,
    Continue,
    Break,
    EOF,
    Identifier,
}

public class Token
{
    public readonly TokenType Type;
    public readonly Position PosStart,
        PosEnd;

    public Token(TokenType type, Position posStart, Position posEnd)
    {
        Type = type;
        PosStart = posStart;
        PosEnd = posEnd;
    }

    public static TokenType? GetKeywordTokenType(string identifier) =>
        identifier switch
        {
            "func" => TokenType.Func,
            "int" => TokenType.IntType,
            "string" => TokenType.StringType,
            "double" => TokenType.DoubleType,
            "bool" => TokenType.BoolType,
            "and" => TokenType.And,
            "or" => TokenType.Or,
            "not" => TokenType.Not,
            "if" => TokenType.If,
            "elif" => TokenType.Elif,
            "try" => TokenType.Try,
            "eltry" => TokenType.Eltry,
            "else" => TokenType.Else,
            "repeat" => TokenType.Repeat,
            "loop" => TokenType.Loop,
            "while" => TokenType.While,
            "return" => TokenType.Return,
            "continue" => TokenType.Continue,
            "break" => TokenType.Break,
            _ => null,
        };

    public override string ToString() => $"[{Type}]";
}

public class IdentifierToken : Token
{
    public readonly string Value;

    public IdentifierToken(string value, Position posStart, Position posEnd)
        : base(TokenType.Identifier, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Identifier, {Value}]";
}

public class IntToken : Token
{
    public readonly int Value;

    public IntToken(int value, Position posStart, Position posEnd)
        : base(TokenType.Int, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Int, {Value}]";
}

public class DoubleToken : Token
{
    public readonly double Value;

    public DoubleToken(double value, Position posStart, Position posEnd)
        : base(TokenType.Double, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Double, {Value}]";
}

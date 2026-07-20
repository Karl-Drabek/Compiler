using Compiler.Grammar;
using Compiler.utils;

namespace Compiler.Lexer;

/// <summary>
/// Represents a lexical token produced by the lexer, containing its type and position information within the source code.
/// </summary>
public class Token
{
    public readonly TerminalSymbol.Type Type;
    public readonly Position PosStart,
        PosEnd;

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class with the specified type and position information.   
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="posStart">The starting position of the token in the source code.</param>
    /// <param name="posEnd">The ending position of the token in the source code.</param>
    public Token(TerminalSymbol.Type type, Position posStart, Position posEnd)
    {
        Type = type;
        PosStart = posStart;
        PosEnd = posEnd;
    }

    /// <summary>
    /// Creates a token based on the given identifier and position information. If the identifier matches a known keyword, a corresponding token is created; otherwise, an identifier token is created.
    /// </summary>
    /// <param name="identifier">The identifier string to create a token for.</param>
    /// <param name="startPosition">The starting position of the token in the source code.</param>
    /// <param name="endposition">The ending position of the token in the source code.</param>
    /// <returns>The created token, either a keyword token or an identifier token.</returns>
    public static Token MakeTokenFromString(string identifier, Position startPosition, Position endposition){
        TerminalSymbol.Type? tokenType = Token.StringToToken(identifier);

        if (tokenType is not null)
        {
            return new Token(tokenType.Value, startPosition, endposition);
        }
        else
        {
            return new IdentifierToken(identifier, startPosition, endposition);
        }
    }

    /// <summary>
    /// Determines the keyword type for a given identifier, if it matches a known keyword.
    /// </summary>
    /// <param name="identifier">The identifier to check.</param>
    /// <returns>The corresponding keyword type if the identifier is a keyword; otherwise, null.</returns>
    private static TerminalSymbol.Type? StringToToken(string identifier) =>
        identifier switch //TODO: Consider 'and', 'or', 'not' keywords as logical operators 
        {
            // Types
            "int" => TerminalSymbol.Type.IntType,
            "string" => TerminalSymbol.Type.StringType,
            "double" => TerminalSymbol.Type.DoubleType,
            "bool" => TerminalSymbol.Type.BoolType,
            "char" => TerminalSymbol.Type.CharType,
            "void" => TerminalSymbol.Type.VoidType,

            // Keywords
            "if" => TerminalSymbol.Type.If,
            "else" => TerminalSymbol.Type.Else,
            "try" => TerminalSymbol.Type.Try,
            "catch" => TerminalSymbol.Type.Catch,
            "finally" => TerminalSymbol.Type.Finally,
            "for" => TerminalSymbol.Type.For,
            "foreach" => TerminalSymbol.Type.Foreach,
            "in" => TerminalSymbol.Type.In,
            "loop" => TerminalSymbol.Type.Loop,
            "do" => TerminalSymbol.Type.Do,
            "while" => TerminalSymbol.Type.While,
            "switch" => TerminalSymbol.Type.Switch,
            "case" => TerminalSymbol.Type.Case,
            "default" => TerminalSymbol.Type.Default,
            "return" => TerminalSymbol.Type.Return,
            "continue" => TerminalSymbol.Type.Continue,
            "break" => TerminalSymbol.Type.Break,
            "class" => TerminalSymbol.Type.Class,
            "extends" => TerminalSymbol.Type.Extends,
            "this" => TerminalSymbol.Type.This,
            "parent" => TerminalSymbol.Type.Parent,
            "new" => TerminalSymbol.Type.New,
            "is" => TerminalSymbol.Type.Is,

            // Anything else
            "null" => TerminalSymbol.Type.NullLiteral,
            _ => null,
        };

    public override string ToString() => $"[{Type}]";
}

/// <summary>
/// Represents an identifier token, containing the identifier's value and position information.
/// </summary>
public class IdentifierToken : Token
{
    public readonly string Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierToken"/> class with the specified value and position information.
    /// </summary>
    /// <param name="value">The identifier value of the token.</param>
    /// <param name="posStart">The starting position of the token in the source code.</param>
    /// <param name="posEnd">The ending position of the token in the source code.</param>
    public IdentifierToken(string value, Position posStart, Position posEnd)
        : base(TerminalSymbol.Type.Identifier, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Identifier, {Value}]";
}

/// <summary>
/// Represents an integer token, containing the integer value and position information.
/// </summary>
public class IntToken : Token
{
    public readonly int Value;
    /// <summary>
    /// Initializes a new instance of the <see cref="IntToken"/> class with the specified value and position information.
    /// </summary>
    /// <param name="value">The integer value of the token.</param>
    /// <param name="posStart">The starting position of the token in the source code.</param>
    /// <param name="posEnd">The ending position of the token in the source code.</param>
    public IntToken(int value, Position posStart, Position posEnd)
        : base(TerminalSymbol.Type.IntLiteral, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Int, {Value}]";
}

/// <summary>
/// Represents a string token, containing the string value and position information.
/// </summary>
public class StringToken : Token
{
    public readonly string Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringToken"/> class with the specified type, value, and position information.
    /// </summary>
    /// <param name="value">The string value of the token.</param>
    /// <param name="posStart">The starting position of the token in the source code.</param>
    /// <param name="posEnd">The ending position of the token in the source code.</param>
    public StringToken(string value, Position posStart, Position posEnd)
        : base(TerminalSymbol.Type.StringLiteral, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[String, {Value}]";
}

/// <summary>
/// Represents a double token, containing the double value and position information.
/// </summary>
public class DoubleToken : Token
{
    public readonly double Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleToken"/> class with the specified value and position information.
    /// </summary>
    /// <param name="value">The double value of the token.</param>
    /// <param name="posStart">The starting position of the token in the source code.</param>
    /// <param name="posEnd">The ending position of the token in the source code.</param>
    public DoubleToken(double value, Position posStart, Position posEnd)
        : base(TerminalSymbol.Type.DoubleLiteral, posStart, posEnd)
    {
        Value = value;
    }

    public override string ToString() => $"[Double, {Value}]";
}

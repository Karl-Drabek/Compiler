namespace Compiler.Grammar;


/// <summary>
/// Represents a symbol in the grammar, which can be either a terminal or non-terminal symbol.
/// </summary>
public interface ISymbol;

/// <summary>
/// Represents a terminal symbol in the grammar. These are the same symbols produced by a tokenizer 
/// of an input stream for the compiler. 
/// </summary>
public struct TerminalSymbol : ISymbol, IEquatable<TerminalSymbol>
{
    public enum Type //TODO: Update and fix names
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
        EOF, // Used for the end of the input stream
        Identifier,
    }

    public readonly Type type { get; }

/// <summary>
/// Initializes a new instance of the TerminalSymbol struct with the specified type.
/// </summary>
/// <param name="type">The type of the terminal symbol.</param>
    public TerminalSymbol(Type type) 
    {
        this.type = type;
    }

    public bool Equals(TerminalSymbol other)
    {
        return type == other.type;
    }

#nullable enable
    public override bool Equals(object? obj)
    {
        return obj is TerminalSymbol other && Equals(other);
    }

#nullable disable

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }

    public static bool operator ==(TerminalSymbol left, TerminalSymbol right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TerminalSymbol left, TerminalSymbol right)
    {
        return !left.Equals(right);
    }
}

/// <summary>
/// Represents a non-terminal symbol in the grammar.
/// These symbols are used for the creation of productions.
/// </summary>
public struct NonTerminalSymbol : ISymbol, IEquatable<NonTerminalSymbol>
{
    public enum Type
    {
        START, // Only Used for the start symbol of the grammar
    }

    public readonly Type type { get; }
/// <summary>
/// Initializes a new instance of the NonTerminalSymbol struct with the specified type.
/// </summary>
/// <param name="type">The type of the non-terminal symbol.</param>
    public NonTerminalSymbol(Type type)
    {
        this.type = type;
    }

    public bool Equals(NonTerminalSymbol other)
    {
        return type == other.type;
    }

#nullable enable
    public override bool Equals(object? obj)
    {
        return obj is NonTerminalSymbol other && Equals(other);
    }

#nullable disable

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }

    public static bool operator ==(NonTerminalSymbol left, NonTerminalSymbol right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonTerminalSymbol left, NonTerminalSymbol right)
    {
        return !left.Equals(right);
    }
}

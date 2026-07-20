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
    public enum Type
    {
        //Literals
        IntLiteral,
        StringLiteral,
        DoubleLiteral, 
        BoolLiteral,
        CharLiteral,
        NullLiteral,

        // Types
        IntType, // int
        CharType, // char
        StringType, // string
        DoubleType, // double
        BoolType, // bool
        VoidType, // void

        // Arithmetic Operators
        Plus, // +
        Minus, // -
        Multiply, // *
        Divide, // /
        Modulo, // %

        //Logical Operators
        LogicalNot, // !
        LogicalAnd, // &&
        LogicalOr, // ||
        NullCoalescing, // ??

        // Comparison Operators
        EqualEqual, // ==
        NotEqual, // !=
        LessThan, // <
        GreaterThan, // >
        LessThanOrEqual, // <=
        GreaterThanOrEqual, // >=

        // Bitwise Operators
        BitwiseXor, // ^
        BitwiseOr, // |
        BitwiseAnd, // &
        BitwiseNot, // ~
        LeftShift, // <<
        RightShift, // >>

        // Binary Assignment Operators
        Equals, // =
        EqualsPlus, // +=
        EqualsMinus, // -=
        EqualsMultiply, // *=
        EqualsDivide, // /=
        EqualsModulo, // %=
        LeftShiftEquals, // <<=
        RightShiftEquals, // >>=
        AndEquals, // &=
        OrEquals, // |=
        XorEquals, // ^=
        NullCoalescingEquals, // ??=

        // Unary Assignment Operators
        Increment, // ++
        Decrement, // --
        LogicalNegation, // !!
        BitwiseNegation, // ~~

        // Punctuation
        LeftParen, // (
        RightParen, // )
        LBrace, // {
        RightBrace, // }
        LBrack, // [
        RBrack, // ]
        Comma, // ,
        Semicolon, // ;
        Dot, // .
        NullConditionalFieldAccess, // ?.
        NullConditionalArrayAccess, // ?[
        Colon, // :
        QuestionMark, // ?

        // Keywords
        If,
        Else,
        Try,
        Catch,
        Finally,
        For,
        Foreach,
        In,
        Loop,
        Do,
        While,
        Switch,
        Case,
        Default,
        Return,
        Continue,
        Break,
        Class,
        Extends,
        This,
        Parent,
        New,
        Is,

        // Special cases
        EOF, // Used for the end of the input stream
        Identifier, // User defined identifier
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

using Compiler.Parser.AST;

namespace Compiler.Grammar;


/// <summary>
/// Represents a symbol in the grammar, which can be either a terminal or non-terminal symbol.
/// </summary>
public interface ISymbol
{
    /// <summary>
    /// Combines two symbols into an EBNFBuilder using the OR operator. This allows for the creation of 
    /// an EBNF expression that represents a choice between the two symbols.
    /// </summary>
    /// <param name="left">The left-hand side symbol of the OR operation.</param>
    /// <param name="right">The right-hand side symbol of the OR operation.</param>
    /// <returns>An EBNFBuilder representing the choice between the two symbols.</returns>
    public static EBNFProduction operator |(ISymbol left, ISymbol right)
    {
        return EBNFProduction.Or(left, right);
    }

    /// <summary>
    /// Combines two symbols into an EBNFBuilder using the AND operator. This allows for the creation of 
    /// an EBNF expression that represents a sequence of the two symbols.
    /// </summary>
    /// <param name="left">The left-hand side symbol of the AND operation.</param>
    /// <param name="right">The right-hand side symbol of the AND operation.</param>
    /// <returns>An EBNFBuilder representing the sequence of the two symbols.</returns>
    public static EBNFProduction operator &(ISymbol left, ISymbol right)
    {
        return EBNFProduction.And(left, right);
    }
}

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
public abstract class NonTerminalSymbol : ISymbol
{
    /// <summary>
    /// Retrieves the corresponding INodal instance for this non-terminal symbol using the provided node stack.
    /// </summary>
    /// <param name="nodeStack">The stack of INodal instances used to retrieve the corresponding node.</param>
    /// <returns>The INodal instance corresponding to this non-terminal symbol.</returns>
    internal abstract INodal getNodal(Stack<INodal> nodeStack);
}

/// <summary>
/// Represents a non-terminal symbol that is used to denote a collection in the grammar. Each instance has a unique type identifier.
/// </summary>
public class CollectionNonTerminalSymbol : NonTerminalSymbol, IEquatable<CollectionNonTerminalSymbol>
{
    public static int GlobalID;
    public readonly int type;
    internal override INodal getNodal(Stack<INodal> nodeStack)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance of the CollectionNonTerminalSymbol class with a unique type identifier.
    /// </summary>
    public CollectionNonTerminalSymbol()
    {
        type = GlobalID++;
    }

    public bool Equals(CollectionNonTerminalSymbol other)
    {
        return type == other.type;
    }

#nullable enable
    public override bool Equals(object? obj)
    {
        return obj is CollectionNonTerminalSymbol other && Equals(other);
    }

#nullable disable

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }

    public static bool operator ==(CollectionNonTerminalSymbol left, CollectionNonTerminalSymbol right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CollectionNonTerminalSymbol left, CollectionNonTerminalSymbol right)
    {
        return !left.Equals(right);
    }
}

/// <summary>
/// Represents the start symbol of the grammar. This symbol is used as the initial non-terminal symbol in the grammar and has a singleton instance.
/// </summary>
public class StartSymbol : NonTerminalSymbol
{
    private static readonly StartSymbol instance = new StartSymbol();
    /// <summary>
    /// Gets the singleton instance of the StartSymbol.
    /// </summary>
    public static StartSymbol Instance => instance;
    /// <summary>
    /// Retrieves the corresponding INodal instance for the start symbol using the provided node stack.
    /// </summary>
    /// <param name="nodeStack">The stack of INodal instances used to retrieve the corresponding node.</param>
    /// <returns>The INodal instance corresponding to the start symbol.</returns>
    internal override INodal getNodal(Stack<INodal> nodeStack)
    {
        // TODO
        throw new NotImplementedException();
    }
}

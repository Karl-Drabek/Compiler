using Compiler.Parser.SLR;

namespace Compiler.Grammar;

/// <summary>
/// Represents an Extended Backus-Naur Form (EBNF) production, which can be converted into one or more standard productions for a context-free grammar.
/// </summary>
public class EBNFProduction
{
    HashSet<List<ISymbol>> symbols;

    /// <summary>
    /// Initializes a new instance of the EBNFProduction class with an empty set of symbol sequences.
    /// </summary>
    public EBNFProduction()
    {
        symbols = new();
    }

    /// <summary>
    /// Initializes a new instance of the EBNFProduction class with the specified set of symbol sequences.
    /// </summary>
    /// <param name="symbols">The set of symbol sequences to initialize the EBNF production with.</param>
    public EBNFProduction(HashSet<List<ISymbol>> symbols)
    {
        this.symbols = [.. symbols];
    }

    /// <summary>
    /// Converts the EBNF production into a list of standard productions for the specified non-terminal symbol. Each symbol sequence in the EBNF production becomes a separate production rule for the non-terminal symbol.
    /// </summary>
    /// <param name="symbol">The non-terminal symbol for which the standard productions are generated.</param>
    /// <returns>A list of standard productions corresponding to the symbol sequences in the EBNF production.</returns>
    public List<Production> GetProductions(NonTerminalSymbol symbol)
    {
        List<Production> productions = new();
        foreach (var symbolList in symbols)
        {
            productions.Add(new Production(symbol, symbolList));
        }
        return productions;
    }

    // OR
    /// <summary>
    /// Creates a new EBNF production that represents the logical OR (alternation) of the two specified EBNF productions. The resulting EBNF production contains all symbol sequences from both the left and right productions.
    /// </summary>
    /// <param name="left">The left EBNF production in the OR operation.</param>
    /// <param name="right">The right EBNF production in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left and right productions.</returns>
    public static EBNFProduction Or(EBNFProduction left, EBNFProduction right)
    {
        return new EBNFProduction([.. left.symbols, .. right.symbols]);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical OR (alternation) of the specified EBNF production and a single symbol. The resulting EBNF production contains all symbol sequences from the left production and a new sequence containing the right symbol.
    /// </summary>
    /// <param name="left">The EBNF production in the OR operation.</param>
    /// <param name="right">The symbol to be included in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left production and the right symbol.</returns>
    public static EBNFProduction Or(EBNFProduction left, ISymbol right)
    {
        return new EBNFProduction([.. left.symbols, new List<ISymbol> { right }]);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical OR (alternation) of a single symbol and an EBNF production. The resulting EBNF production contains a new sequence containing the left symbol and all symbol sequences from the right production.
    /// </summary>
    /// <param name="left">The symbol to be included in the OR operation.</param>
    /// <param name="right">The EBNF production in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left symbol and the right production.</returns>
    public static EBNFProduction Or(ISymbol left, EBNFProduction right)
    {
        return Or(right, left);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical OR (alternation) of two symbols. The resulting EBNF production contains two sequences, one for each symbol.
    /// </summary>
    /// <param name="left">The left symbol in the OR operation.</param>
    /// <param name="right">The right symbol in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the two symbols.</returns>
    public static EBNFProduction Or(ISymbol left, ISymbol right)
    {
        return new EBNFProduction([new List<ISymbol> { left }, new List<ISymbol> { right }]);
    }

    // OR operator overloads

    /// <summary>
    /// Overloads the | operator to perform the logical OR (alternation) of two EBNF productions.
    /// </summary>
    /// <param name="left">The left EBNF production in the OR operation.</param>
    /// <param name="right">The right EBNF production in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left and right productions.</returns>
    public static EBNFProduction operator |(EBNFProduction left, EBNFProduction right)
    {
        return Or(left, right);
    }

    /// <summary>
    /// Overloads the | operator to perform the logical OR (alternation) of an EBNF production and a single symbol.
    /// </summary>
    /// <param name="left">The EBNF production in the OR operation.</param>
    /// <param name="right">The symbol in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left production and the right symbol.</returns>
    public static EBNFProduction operator |(EBNFProduction left, ISymbol right)
    {
        return Or(left, right);
    }

    /// <summary>
    /// Overloads the | operator to perform the logical OR (alternation) of a single symbol and an EBNF production.
    /// </summary>
    /// <param name="left">The symbol in the OR operation.</param>
    /// <param name="right">The EBNF production in the OR operation.</param>
    /// <returns>A new EBNF production representing the logical OR of the left symbol and the right production.</returns>
    public static EBNFProduction operator |(ISymbol left, EBNFProduction right)
    {
        return Or(left, right);
    }

    // AND

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of two EBNF productions. The resulting EBNF production contains all possible concatenations of the symbol sequences from the left and right productions.
    /// </summary>
    /// <param name="left">The left EBNF production in the AND operation.</param>
    /// <param name="right">The right EBNF production in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left and right productions.</returns>
    public static EBNFProduction And(EBNFProduction left, EBNFProduction right)
    {
        HashSet<List<ISymbol>> symbols = new();
        foreach (var leftSymbols in left.symbols)
        {
            foreach (var rightSymbols in right.symbols)
            {
                symbols.Add(leftSymbols.Concat(rightSymbols).ToList());
            }
        }
        return new EBNFProduction(symbols);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of an EBNF production and a single symbol. The resulting EBNF production contains all possible concatenations of the symbol sequences from the left production with the right symbol appended to each sequence.
    /// </summary>
    /// <param name="left">The EBNF production in the AND operation.</param>
    /// <param name="right">The symbol in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left production and the right symbol.</returns>
    public static EBNFProduction And(EBNFProduction left, ISymbol right)
    {
        HashSet<List<ISymbol>> symbols = new();
        foreach (var leftSymbols in left.symbols)
        {
            leftSymbols.Add(right);
            symbols.Add(leftSymbols.ToList());
        }
        return new EBNFProduction(symbols);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of a single symbol and an EBNF production. The resulting EBNF production contains all possible concatenations of the symbol sequences from the right production with the left symbol prepended to each sequence.
    /// </summary>
    /// <param name="left">The symbol in the AND operation.</param>
    /// <param name="right">The EBNF production in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left symbol and the right production.</returns>
    public static EBNFProduction And(ISymbol left, EBNFProduction right)
    {
        return And(right, left);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of two symbols. The resulting EBNF production contains a single sequence consisting of the left symbol followed by the right symbol.
    /// </summary>
    /// <param name="left">The left symbol in the AND operation.</param>
    /// <param name="right">The right symbol in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the two symbols.</returns>
    public static EBNFProduction And(ISymbol left, ISymbol right)
    {
        return new EBNFProduction([new List<ISymbol> { left, right }]);
    }

    // AND operator overloads

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of two EBNF productions. The resulting EBNF production contains all possible concatenations of the symbol sequences from the left and right productions.
    /// </summary>
    /// <param name="left">The left EBNF production in the AND operation.</param>
    /// <param name="right">The right EBNF production in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left and right productions.</returns>
    public static EBNFProduction operator &(EBNFProduction left, EBNFProduction right)
    {
        return And(left, right);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of an EBNF production and a single symbol. The resulting EBNF production contains all possible concatenations of the symbol sequences from the left production with the right symbol appended to each sequence.
    /// </summary>
    /// <param name="left">The EBNF production in the AND operation.</param>
    /// <param name="right">The symbol in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left production and the right symbol.</returns>
    public static EBNFProduction operator &(EBNFProduction left, ISymbol right)
    {
        return And(left, right);
    }

    /// <summary>
    /// Creates a new EBNF production that represents the logical AND (concatenation) of a single symbol and an EBNF production. The resulting EBNF production contains all possible concatenations of the symbol sequences from the right production with the left symbol prepended to each sequence.
    /// </summary>
    /// <param name="left">The symbol in the AND operation.</param>
    /// <param name="right">The EBNF production in the AND operation.</param>
    /// <returns>A new EBNF production representing the logical AND of the left symbol and the right production.</returns>
    public static EBNFProduction operator &(ISymbol left, EBNFProduction right)
    {
        return And(left, right);
    }

    // Optional

    /// <summary>
    /// Creates a new EBNF production that represents an optional EBNF production. The resulting EBNF production contains all the symbol sequences from the original production, as well as an empty sequence representing the optionality.
    /// </summary>
    /// <param name="builder">The EBNF production to make optional.</param>
    /// <returns>A new EBNF production representing the optional version of the input production.</returns>
    public static EBNFProduction Optional(EBNFProduction builder)
    {
        builder.symbols.Add(new List<ISymbol>());
        return new EBNFProduction(builder.symbols);
    }

    /// <summary>
    /// Creates a new EBNF production that represents an optional single symbol. The resulting EBNF production contains the symbol sequence as well as an empty sequence representing the optionality.
    /// </summary>
    /// <param name="symbol">The symbol to make optional.</param>
    /// <returns>A new EBNF production representing the optional version of the input symbol.</returns>
    public static EBNFProduction Optional(ISymbol symbol)
    {
        return Optional(new EBNFProduction([new List<ISymbol> { symbol }]));
    }

    // TODO: Zero or more
}

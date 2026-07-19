using Compiler.Parser.SLR;

namespace Compiler.Grammar;

/// <summary>
/// Represents a context-free grammar in Backus-Naur Form (BNF) and provides 
/// methods to define productions and generate an LR DFA.
/// </summary>
class BackusNaurForm
{
    private readonly NonTerminalSymbol START_SYMBOL = new NonTerminalSymbol(
        NonTerminalSymbol.Type.START
    );
    private readonly TerminalSymbol EOF = new TerminalSymbol(TerminalSymbol.Type.EOF);
    private readonly Dictionary<NonTerminalSymbol, List<Production>> productions;
    
    /// <summary>
    /// Initializes a new instance of the BackusNaurForm class with an empty set of productions.
    /// </summary>
    /// <remarks>
    /// The grammar starts with no production rules, and the start symbol must be assigned 
    /// using the assignStartSymbol method before generating the LR DFA.
    /// </remarks>
    public BackusNaurForm()
    {
        productions = new Dictionary<NonTerminalSymbol, List<Production>>();
    }

/// <summary>
/// Adds a production rule to the grammar. If the non-terminal symbol of the 
/// production does not exist in the grammar, it will be added.
/// </summary>
/// <param name="production">The production rule to add to the grammar.</param>
    public void AddProduction(Production production)
    {
        NonTerminalSymbol nonTerminal = production.NonTerminal;
        if (!productions.ContainsKey(nonTerminal))
        {
            productions[nonTerminal] = new List<Production>();
        }
        productions[nonTerminal].Add(production);
    }

    /// <summary>
    /// Assigns the start symbol of the grammar by adding a production rule for it.
    /// </summary>
    /// <param name="startSymbol">The non-terminal symbol to be used as the start symbol.</param>
    public void assignStartSymbol(NonTerminalSymbol startSymbol)
    {
        AddProduction(new StartProduction(startSymbol));
    }

    /// <summary>
    /// Generates an LR DFA (Deterministic Finite Automaton) for the grammar.
    /// </summary>
    /// <returns>An LR DFA representing the grammar.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the start symbol has not been assigned before generating the DFA.</exception>
    public LRDFA toLRDFA()
    {
        if (!productions.ContainsKey(START_SYMBOL))
        {
            throw new InvalidOperationException(
                "Start symbol not assigned before generating DFA. Try assignStartSymbol()."
            );
        }
        return new LRDFA(productions);
    }
}

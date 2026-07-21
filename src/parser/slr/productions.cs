using Compiler.Grammar;
using Compiler.Parser.AST;

namespace Compiler.Parser.SLR;

public class Production
{
    public NonTerminalSymbol NonTerminal { get; }
    public List<ISymbol> Symbols { get; }

    public Production(NonTerminalSymbol nonTerminal, List<ISymbol> symbols)
    {
        NonTerminal = nonTerminal;
        Symbols = symbols;
    }

    public LRItem ToLRItem()
    {
        return new LRItem(this);
    }

// TODO
    //public abstract Node ToNode(Stack<Node> nodeStack);
}

public class StartProduction : Production
{
    public StartProduction(NonTerminalSymbol startSymbol)
        : base(
            new NonTerminalSymbol(NonTerminalSymbol.Type.START),
            new List<ISymbol> { startSymbol, new TerminalSymbol(TerminalSymbol.Type.EOF) }
        ) { }
}

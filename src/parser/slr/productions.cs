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

    public List<INodal> GetNodes(Stack<INodal> nodeStack)
    {
        List<INodal> nodes = new List<INodal>();
        foreach (var symbol in Symbols)
        {
            nodes.Add(symbol is NonTerminalSymbol nonTerminal ? nonTerminal.getNodal(nodeStack) : nodeStack.Pop());
        }
        return nodes;
    }
}

public class StartProduction : Production
{
    public StartProduction(NonTerminalSymbol startSymbol)
        : base(
            StartSymbol.Instance,
            new List<ISymbol> { startSymbol, new TerminalSymbol(TerminalSymbol.Type.EOF) }
        ) { }
}

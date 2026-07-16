namespace Compiler.Grammar;

class BackusNaurForm
{
    private readonly NonTerminalSymbol START_SYMBOL = new NonTerminalSymbol(
        NonTerminalSymbol.Type.START
    );
    private readonly TerminalSymbol EOF = new TerminalSymbol(TerminalSymbol.Type.EOF);
    private readonly Dictionary<NonTerminalSymbol, List<Production>> productions;

    public BackusNaurForm()
    {
        productions = new Dictionary<NonTerminalSymbol, List<Production>>();
    }

    public void AddProduction(Production production)
    {
        NonTerminalSymbol nonTerminal = production.NonTerminal;
        if (!productions.ContainsKey(nonTerminal))
        {
            productions[nonTerminal] = new List<Production>();
        }
        productions[nonTerminal].Add(production);
    }

    public void assignStartSymbol(NonTerminalSymbol startSymbol)
    {
        AddProduction(new StartProduction(startSymbol));
    }

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

public abstract class Production
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

    public abstract Node ToNode(Stack<Node> nodeStack);
}

public class StartProduction : Production
{
    public StartProduction(NonTerminalSymbol startSymbol)
        : base(
            new NonTerminalSymbol(NonTerminalSymbol.Type.START),
            new List<ISymbol> { startSymbol, new TerminalSymbol(TerminalSymbol.Type.EOF) }
        ) { }

    public override Node ToNode(Stack<Node> nodeStack)
    {
        return new Node();
    }
}

public interface ISymbol;

public struct TerminalSymbol : ISymbol
{
    public enum Type
    {
        EOF, // Only Used for the end of the input stream
    }

    public readonly Type type { get; }

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

public struct NonTerminalSymbol : ISymbol
{
    public enum Type
    {
        START, // Only Used for the start symbol of the grammar
    }

    public readonly Type type { get; }

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

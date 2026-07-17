using Compiler.Grammar;

namespace Compiler.Parser.SLR;

public class LRDFA
{
    public readonly LRState startState;

    public readonly ISet<LRState> states;

    public LRDFA(Dictionary<NonTerminalSymbol, List<Production>> productions)
    {
        startState = new LRState();

        foreach (var keyValue in productions)
        {
            foreach (var prod in keyValue.Value)
            {
                startState.AddItem(prod.ToLRItem());
            }
        }

        startState.findClosure(productions);
        states.Add(startState);

        HashSet<LRState> unvisitedStates = [startState];

        while (unvisitedStates.Count > 0)
        {
            var currentState = unvisitedStates.First();
            unvisitedStates.Remove(currentState);

            Dictionary<ISymbol, LRState> newStates = new();

            // Find all possible transitions from the current state
            foreach (var item in currentState.items)
            {
                ISymbol nextSymbol = item.getNextSymbol();
                if (nextSymbol != null)
                {
                    var newItem = item.advance();
                    if (!newStates.ContainsKey(nextSymbol))
                    {
                        newStates[nextSymbol] = new LRState();
                    }
                    newStates[nextSymbol].AddItem(newItem);
                }
            }

            // Find the closure of all the transitions in the new states.
            foreach (var keyValue in newStates)
            {
                keyValue.Value.findClosure(productions);
                currentState.AddTransition(keyValue.Key, keyValue.Value);
                if (!states.Contains(keyValue.Value))
                {
                    unvisitedStates.Add(keyValue.Value);
                }
            }
        }
    }
}

public class LRState
{
    public ISet<LRItem> items { get; private set; }

    public readonly Dictionary<ISymbol, LRState> transitions;
    public bool isAccepting;

    public LRState()
    {
        items = new HashSet<LRItem>();
        transitions = new Dictionary<ISymbol, LRState>();
        isAccepting = false;
    }

    public void AddItem(LRItem item)
    {
        items.Add(item);
    }

    public void AddTransition(ISymbol symbol, LRState state)
    {
        transitions[symbol] = state;
    }

    public void findClosure(Dictionary<NonTerminalSymbol, List<Production>> productions)
    {
        bool addedNewItem;
        do
        {
            addedNewItem = false;
            var newItems = new HashSet<LRItem>(items);

            foreach (var item in items)
            {
                ISymbol nextSymbol = item.getNextSymbol();

                if (nextSymbol is null)
                {
                    isAccepting = true;
                    continue;
                }

                if (
                    nextSymbol is NonTerminalSymbol nonTerminal
                    && productions.TryGetValue(nonTerminal, out var productionList)
                )
                {
                    foreach (var production in productionList)
                    {
                        var lrItem = production.ToLRItem();
                        if (!newItems.Contains(lrItem))
                        {
                            newItems.Add(lrItem);
                            addedNewItem = true;
                        }
                    }
                }
            }

            items = newItems;
        } while (addedNewItem);
    }

    public bool Equals(LRState other)
    {
        return items.SetEquals(other.items);
    }

#nullable enable
    public override bool Equals(object? obj)
    {
        return obj is LRState other && Equals(other);
    }

#nullable disable

    public override int GetHashCode()
    {
        return items.GetHashCode();
    }

    public static bool operator ==(LRState left, LRState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(LRState left, LRState right)
    {
        return !left.Equals(right);
    }
}

public class LRItem
{
    public readonly Production Production;
    public readonly int Position;

    public LRItem(Production production, int position = 0)
    {
        Production = production;
        Position = position;
    }

    public ISymbol getNextSymbol()
    {
        if (Position < Production.Symbols.Count)
        {
            return Production.Symbols[Position];
        }
        return null;
    }

    public LRItem advance()
    {
        return new LRItem(Production, Position + 1);
    }
}

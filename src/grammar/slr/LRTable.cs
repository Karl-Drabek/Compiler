namespace Compiler.Grammar;

class LRTable
{
    private readonly Dictionary<LRState, Dictionary<TerminalSymbol, ILRAction>> actionTable;
    private readonly Dictionary<LRState, Dictionary<NonTerminalSymbol, LRState>> gotoTable;
    private readonly Stack<LRState> stateStack;

    public LRTable()
    {
        actionTable = new Dictionary<LRState, Dictionary<TerminalSymbol, ILRAction>>();
        gotoTable = new Dictionary<LRState, Dictionary<NonTerminalSymbol, LRState>>();
        stateStack = new Stack<LRState>();
    }

    public Node Parse(List<TerminalSymbol> input, LRDFA dfa)
    {
        stateStack.Push(dfa.startState);

        foreach (var currentSymbol in input)
        {
            LRState currentState = stateStack.Peek();

            if (
                !actionTable.ContainsKey(currentState)
                || !actionTable[currentState].TryGetValue(currentSymbol, out ILRAction action)
            )
            {
                throw new InvalidOperationException(
                    $"No action defined for state {currentState} and symbol {currentSymbol}"
                );
            }

            if (action is LRShiftAction shiftAction)
            {
                stateStack.Push(shiftAction.NextState);
            }
            else if (action is LRReduceAction reduceAction)
            {
                Production production = reduceAction.Production;
                foreach (var symbol in production.Symbols)
                {
                    stateStack.Pop();
                }

                LRState nextState = gotoTable[stateStack.Peek()][production.NonTerminal];
                stateStack.Push(nextState);
            }
            else if (action is LRAcceptAction)
            {
                return new Node("Accept");
            }
        }
    }
}

public interface ILRAction;

public class LRShiftAction : ILRAction
{
    public LRState NextState { get; }

    public LRShiftAction(LRState nextState)
    {
        NextState = nextState;
    }
}

public class LRReduceAction : ILRAction
{
    public Production Production { get; }

    public LRReduceAction(Production production)
    {
        Production = production;
    }
}

public class LRAcceptAction : ILRAction;

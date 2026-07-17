using Compiler.Grammar;
using Compiler.Parser.AST;

namespace Compiler.Parser.SLR;

class LRTable
{
    private readonly Dictionary<LRState, Dictionary<TerminalSymbol, ILRAction>> actionTable;
    private readonly Dictionary<LRState, Dictionary<NonTerminalSymbol, LRState>> gotoTable;
    private readonly Stack<LRState> stateStack;
    private readonly Stack<Node> nodeStack;

    public LRTable()
    {
        actionTable = new Dictionary<LRState, Dictionary<TerminalSymbol, ILRAction>>();
        gotoTable = new Dictionary<LRState, Dictionary<NonTerminalSymbol, LRState>>();
        stateStack = new Stack<LRState>();
        nodeStack = new Stack<Node>();
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
                Node node = reduceAction.Production.ToNode(nodeStack);
                nodeStack.Push(node);

                for (int i = 0; i < production.Symbols.Count; i++)
                {
                    stateStack.Pop();
                }

                LRState nextState = gotoTable[stateStack.Peek()][production.NonTerminal];
                stateStack.Push(nextState);
            }
            else if (action is LRAcceptAction)
            {
                return nodeStack.Pop();
            }
        }

        return null;
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

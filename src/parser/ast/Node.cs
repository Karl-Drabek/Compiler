namespace Compiler.Parser.AST;

public interface INodal;

public class Collection<T> : INodal
{
    public List<T> Items { get; }

    public Collection(List<T> items)
    {
        Items = items;
    }
}

/// <summary>
/// Represents a generic node in the abstract syntax tree (AST).
/// </summary>
public class Node : INodal;


/// <summary>
/// Represents the root node of the AST, containing the top-level class declaration.
/// </summary>
public class ProgramNode : Node
{
    private ClassDeclarationNode ClassDeclaration { get; }

/// <summary>
/// Initializes a new instance of the <see cref="ProgramNode"/> class with the specified class declaration.
/// </summary>
/// <param name="classDeclaration">The top-level class declaration node for the program.</param>
    public ProgramNode(ClassDeclarationNode classDeclaration)
    {
        ClassDeclaration = classDeclaration;
    }
}

/// <summary>
/// Represents a class declaration node in the AST, containing the class name, optional base class, and member declarations.
/// </summary>
public class ClassDeclarationNode : Node
{
    private string Name { get; }
    private string Extends { get; }
    private List<DeclarationNode> Declarations { get; }

/// <summary>
/// Initializes a new instance of the <see cref="ClassDeclarationNode"/> class with the specified name, member declarations, and optional base class.
/// </summary>
/// <param name="name">The name of the class being declared.</param>
/// <param name="declarations">The list of member declarations within the class.</param>
/// <param name="extends">The name of the base class, if any.</param>
    public ClassDeclarationNode(
        string name,
        List<DeclarationNode> declarations,
        string extends = null
    )
    {
        Name = name;
        Extends = extends;
        Declarations = declarations;
    }
}

/// <summary>
/// Represents a generic declaration node in the AST.
/// </summary>
public class DeclarationNode : Node;

/// <summary>
/// Represents a variable declaration node in the AST, containing the variable name, type, and optional initializer expression.
/// </summary>
public class VariableDeclarationNode : DeclarationNode
{
    private string Name { get; }
    private TypeNode Type { get; }
    private ExpressionNode Expression { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableDeclarationNode"/> class with the specified variable name, type, and optional initializer expression.
    /// </summary>
    /// <param name="name">The name of the variable being declared.</param>
    /// <param name="type">The type of the variable.</param>
    /// <param name="expression">The initializer expression for the variable, if any.</param>
    public VariableDeclarationNode(string name, TypeNode type, ExpressionNode expression = null)
    {
        Name = name;
        Type = type;
        Expression = expression;
    }
}

/// <summary>
/// Represents a method declaration node in the AST, containing the method name, return type, parameters, and body statements.
/// </summary>
public class MethodDeclarationNode : DeclarationNode
{
    private string Name { get; }
    private ReturnTypeNode ReturnType { get; }
    private List<ParameterNode> Parameters { get; }
    private StatementsNode Statements { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MethodDeclarationNode"/> class with the specified method name, return type, parameters, and body statements.
    /// </summary>
    /// <param name="name">The name of the method being declared.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="parameters">The list of parameters for the method.</param>
    /// <param name="statements">The body statements of the method.</param>
    public MethodDeclarationNode(
        string name,
        TypeNode returnType,
        List<ParameterNode> parameters,
        StatementsNode statements
    )
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Statements = statements;
    }
}

/// <summary>
/// Represents a constructor declaration node in the AST, containing the constructor name, parameters, and body statements.
/// </summary>
public class ConstructorDeclarationNode : DeclarationNode
{
    private string Name { get; }
    private List<ParameterNode> Parameters { get; }
    private StatementsNode Statements { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorDeclarationNode"/> class with the specified constructor name, parameters, and body statements.
    /// </summary>
    /// <param name="name">The name of the constructor being declared.</param>
    /// <param name="parameters">The list of parameters for the constructor.</param>
    /// <param name="statements">The body statements of the constructor.</param>
    public ConstructorDeclarationNode(
        string name,
        List<ParameterNode> parameters,
        StatementsNode statements
    )
    {
        Name = name;
        Parameters = parameters;
        Statements = statements;
    }
}

/// <summary>
/// Represents a block of statements in the AST.
/// </summary>
public class StatementsNode : Node
{
    private List<StatementNode> Statements { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatementsNode"/> class with the specified list of statements.
    /// </summary>
    /// <param name="statements">The list of statements contained in the block.</param>
    public StatementsNode(List<StatementNode> statements)
    {
        Statements = statements;
    }
}

/// <summary>
/// Represents a single statement in the AST.
/// </summary>
public class StatementNode : Node;

/// <summary>
/// Represents a continue statement node in the AST.
/// </summary>
public class ContinueNode : StatementNode;

/// <summary>
/// Represents a break statement node in the AST.
/// </summary>
public class BreakNode : StatementNode;

/// <summary>
/// Represents a return statement node in the AST.
/// </summary>
public class ReturnNode : StatementNode
{
    private ExpressionNode Expression { get; }
/// <summary>
/// Initializes a new instance of the <see cref="ReturnNode"/> class with the specified expression.
/// </summary>
/// <param name="expression">The expression to be returned by the return statement, or null if there is no expression.</param>
    public ReturnNode(ExpressionNode expression = null)
    {
        Expression = expression;
    }
}

/// <summary>
/// Represents a for loop statement node in the AST.
/// </summary>
public class ForNode : StatementNode
{
    private ExpressionNode Initialization { get; }
    private ExpressionNode Condition { get; }
    private ExpressionNode Update { get; }
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForNode"/> class with the specified body, initialization, condition, and update expressions.
    /// </summary>
    /// <param name="body">The body of the for loop.</param>
    /// <param name="initialization">The initialization expression for the for loop, if any.</param>
    /// <param name="condition">The condition expression for the for loop, if any.</param>
    /// <param name="update">The update expression for the for loop, if any.</param>
    public ForNode(
        StatementNode body,
        ExpressionNode initialization = null,
        ExpressionNode condition = null,
        ExpressionNode update = null
    )
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
    }
}

/// <summary>
/// Represents a foreach loop statement node in the AST.
/// </summary>
public class ForeachNode : StatementNode
{
    private TypeNode Type { get; }
    private string Identifier { get; }
    private ExpressionNode Collection { get; }
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForeachNode"/> class with the specified type, identifier, collection, and body.
    /// </summary>
    /// <param name="type">The type of the loop variable.</param>
    /// <param name="identifier">The name of the loop variable.</param>
    /// <param name="collection">The collection being iterated over.</param>
    /// <param name="body">The body of the foreach loop.</param>
    public ForeachNode(
        TypeNode type,
        string identifier,
        ExpressionNode collection,
        StatementNode body
    )
    {
        Type = type;
        Identifier = identifier;
        Collection = collection;
        Body = body;
    }
}

/// <summary>
/// Represents a while loop statement node in the AST.
/// </summary>
public class WhileNode : StatementNode
{
    private ExpressionNode Condition { get; }
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WhileNode"/> class with the specified condition and body.
    /// </summary>
    /// <param name="condition">The condition expression for the while loop.</param>
    /// <param name="body">The body of the while loop.</param>
    public WhileNode(ExpressionNode condition, StatementNode body)
    {
        Condition = condition;
        Body = body;
    }
}

/// <summary>
/// Represents a do-while loop statement node in the AST.
/// </summary>
public class DoWhileNode : StatementNode
{
    private StatementNode Body { get; }
    private ExpressionNode Condition { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DoWhileNode"/> class with the specified body and condition.
    /// </summary>
    /// <param name="body">The body of the do-while loop.</param>
    /// <param name="condition">The condition expression for the do-while loop.</param>
    public DoWhileNode(StatementNode body, ExpressionNode condition)
    {
        Body = body;
        Condition = condition;
    }
}

/// <summary>
/// Represents a loop statement node in the AST.
/// </summary>
public class LoopNode : StatementNode
{
    private ExpressionNode Condition { get; }
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoopNode"/> class with the specified body and optional condition.
    /// </summary>
    /// <param name="body">The body of the loop.</param>
    /// <param name="condition">The condition expression for the loop, or null if the loop has no condition.</param>
    public LoopNode(StatementNode body, ExpressionNode condition = null)
    {
        Body = body;
        Condition = condition;
    }
}

/// <summary>
/// Represents a switch statement node in the AST.
/// </summary>
public class SwitchNode : StatementNode
{
    private ExpressionNode Expression { get; }
    private List<SwitchCaseNode> Cases { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchNode"/> class with the specified expression and cases.
    /// </summary>
    /// <param name="expression">The expression being switched on.</param>
    /// <param name="cases">The list of switch cases.</param>
    public SwitchNode(ExpressionNode expression, List<SwitchCaseNode> cases)
    {
        Expression = expression;
        Cases = cases;
    }
}

/// <summary>
/// Represents a switch case node in the AST.
/// </summary>
public class SwitchCaseNode : Node
{
    private SwitchLabelNode Label { get; }
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchCaseNode"/> class with the specified label and optional body.
    /// </summary>
    /// <param name="label">The switch label for the case.</param>
    /// <param name="body">The body of the switch case, or null if there is no body.</param>
    public SwitchCaseNode(SwitchLabelNode label, StatementNode body = null)
    {
        Label = label;
        Body = body;
    }
}

/// <summary>
/// Represents a switch label node in the AST.
/// </summary>
public class SwitchLabelNode : Node
{
    private ExpressionNode Expression { get; }
    private bool IsDefault { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchLabelNode"/> class with the specified expression.
    /// </summary>
    /// <param name="expression">The expression for the switch label.</param>
    public SwitchLabelNode(ExpressionNode expression)
    {
        Expression = expression;
        IsDefault = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchLabelNode"/> class representing the default label.
    /// </summary>
    public SwitchLabelNode()
    {
        Expression = null;
        IsDefault = true;
    }
}

/// <summary>
/// Represents a try statement node in the AST.
/// </summary>
public class TryNode : StatementNode
{
    private StatementNode TryBlock { get; }
    private CatchesNode CatchBlocks { get; }
    private StatementNode FinallyBlock { get; }
/// <summary>
/// Initializes a new instance of the <see cref="TryNode"/> class with the specified try block, optional catch blocks, and optional finally block.
/// </summary>
/// <param name="tryBlock">The try block statement.</param>
/// <param name="catchBlocks">The collection of catch clauses, or null if there are none.</param>
/// <param name="finallyBlock">The finally block statement, or null if there is none.</param>
    public TryNode(
        StatementNode tryBlock,
        CatchesNode catchBlocks = null,
        StatementNode finallyBlock = null
    )
    {
        TryBlock = tryBlock;
        CatchBlocks = catchBlocks;
        FinallyBlock = finallyBlock;
    }
}

/// <summary>
/// Represents a collection of catch clauses node in the AST.
/// </summary>
public class CatchesNode : Node
{
    private List<CatchNode> CatchClauses { get; }
/// <summary>
/// Initializes a new instance of the <see cref="CatchesNode"/> class with the specified list of catch clauses.
/// </summary>
/// <param name="catchClauses">The list of catch clauses for the try statement.</param>
    public CatchesNode(List<CatchNode> catchClauses)
    {
        CatchClauses = catchClauses;
    }
}

/// <summary>
/// Represents a catch clause node in the AST.
/// </summary>
public class CatchNode : Node
{
    private ParamNode Parameter { get; }
    private StatementNode Body { get; }
/// <summary>
/// Initializes a new instance of the <see cref="CatchNode"/> class with the specified parameter and body.
/// </summary>
/// <param name="parameter">The parameter for the catch clause.</param>
/// <param name="body">The body of the catch clause.</param>
    public CatchNode(ParamNode parameter, StatementNode body)
    {
        Parameter = parameter;
        Body = body;
    }
}

/// <summary>
/// Represents a finally clause node in the AST.
/// </summary>
public class FinallyNode : StatementNode
{
    private StatementNode Body { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FinallyNode"/> class with the specified body.
    /// </summary>
    /// <param name="body">The body of the finally clause.</param>
    public FinallyNode(StatementNode body)
    {
        Body = body;
    }
}

/// <summary>
/// Represents a block node in the AST.
/// </summary>
public class BlockNode : StatementNode
{
    private StatementsNode Statements { get; }
/// <summary>
/// Initializes a new instance of the <see cref="BlockNode"/> class with the specified statements node.
/// </summary>
/// <param name="statements">The statements node representing the block's statements.</param>
    public BlockNode(StatementsNode statements)
    {
        Statements = statements;
    }
}

/// <summary>
/// Represents an if statement node in the AST.
/// </summary>
public class IfNode : StatementNode
{
    private ExpressionNode Condition { get; }
    private StatementNode ThenBranch { get; }
    private StatementNode ElseBranch { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IfNode"/> class with the specified condition, then branch, and optional else branch.
    /// </summary>
    /// <param name="condition">The condition expression for the if statement.</param>
    /// <param name="thenBranch">The statement to execute if the condition is true.</param>
    /// <param name="elseBranch">The statement to execute if the condition is false, or null if there is no else branch.</param>
    public IfNode(
        ExpressionNode condition,
        StatementNode thenBranch,
        StatementNode elseBranch = null
    )
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }
}

/// <summary>
/// Represents an expression node in the AST.
/// </summary>
public class ExpressionNode : Node;

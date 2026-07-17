namespace Compiler.Parser.AST;

/// <summary>
/// Represents a generic node in the abstract syntax tree (AST).
/// </summary>
public class Node;

/// <summary>
/// Represents the root node of the AST, containing the top-level class declaration.
/// </summary>
public class ProgramNode : Node
{
    private ClassDeclarationNode ClassDeclaration { get; }

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

    public SwitchLabelNode(ExpressionNode expression)
    {
        Expression = expression;
        IsDefault = false;
    }

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

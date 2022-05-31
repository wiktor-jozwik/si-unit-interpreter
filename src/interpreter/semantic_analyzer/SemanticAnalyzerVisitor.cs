using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class SemanticAnalyzerVisitor : IVisitor
{
    private readonly SemanticFunctionCallContext _semanticFunctionCallContext = new();

    private readonly Dictionary<string, FunctionStatement> _functions = new();
    private readonly Dictionary<string, IType> _builtInFunctions;
    private IDictionary<string, UnitType> _units = new Dictionary<string, UnitType>();
    

    public SemanticAnalyzerVisitor(BuiltInFunctionsProvider builtInFunctionsProvider)
    {
        var builtInFunctionsAndItsTypes = builtInFunctionsProvider.GetOneArgumentFunctionReturnTypes();
        _builtInFunctions = builtInFunctionsAndItsTypes;
    }

    // public SemanticAnalyzerVisitor(funtions )
    public void Visit(TopLevel element)
    {
        _units = element.Units;

        foreach (var (name, function) in element.Functions)
        {
            _semanticFunctionCallContext.FunctionName = name;
            _semanticFunctionCallContext.Parameters = new Dictionary<string, IType>();

            _functions[name] = function;
            function.Accept(this);
        }
    }

    public void Visit(FunctionStatement element)
    {
        foreach (var parameter in element.Parameters)
        {
            parameter.Accept(this);
        }

        element.Statements.Accept(this);
    }

    public void Visit(Block element)
    {
        _semanticFunctionCallContext.Scopes.AddLast(new SemanticScope());
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }

        _semanticFunctionCallContext.Scopes.RemoveLast();
    }

    public void Visit(VariableDeclaration element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);
        var variableType = element.Accept(typeVisitor);

        _semanticFunctionCallContext.Scopes.Last().Variables[element.Parameter.Name] = variableType;
    }

    public void Visit(IfStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);

        element.Condition.Accept(typeVisitor);
        element.Statements.Accept(this);

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            elseIfStatement.Condition.Accept(typeVisitor);
            elseIfStatement.Accept(this);
        }

        element.ElseStatement.Accept(this);
    }

    public void Visit(ElseIfStatement element)
    {
        element.Statements.Accept(this);
    }

    public void Visit(WhileStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);

        element.Condition.Accept(typeVisitor);
        element.Statements.Accept(this);
    }

    public void Visit(AssignStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(FunctionCall element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(ReturnStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContext, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(Parameter element)
    {
        _semanticFunctionCallContext.Parameters[element.Name] = element.Type;
    }
}
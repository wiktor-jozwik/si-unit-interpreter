using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticAnalyzerVisitor : IVisitor
{
    private readonly FunctionCallContext _functionCallContext = new();

    private readonly Dictionary<string, FunctionStatement> _functions = new();
    private IDictionary<string, UnitType> _units = new Dictionary<string, UnitType>();

    public void Visit(TopLevel element)
    {
        _units = element.Units;

        foreach (var (name, function) in element.Functions)
        {
            _functionCallContext.FunctionName = name;
            _functionCallContext.Parameters = new Dictionary<string, IType>();

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
        _functionCallContext.Scopes.AddLast(new SemanticScope());
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }

        _functionCallContext.Scopes.RemoveLast();
    }

    public void Visit(VariableDeclaration element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);
        var variableType = element.Accept(typeVisitor);

        _functionCallContext.Scopes.Last().Variables[element.Parameter.Name] = variableType;
    }

    public void Visit(IfStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);

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
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);

        element.Condition.Accept(typeVisitor);
        element.Statements.Accept(this);
    }

    public void Visit(AssignStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(FunctionCall element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(ReturnStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_functionCallContext, _functions, _units);
        element.Accept(typeVisitor);
    }
    
    public void Visit(Parameter element)
    {
        _functionCallContext.Parameters[element.Name] = element.Type;
    }
}
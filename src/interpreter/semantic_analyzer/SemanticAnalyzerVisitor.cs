using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class SemanticAnalyzerVisitor : IVisitor
{
    private readonly LinkedList<SemanticFunctionCallContext> _semanticFunctionCallContexts = new();

    private readonly Dictionary<string, FunctionStatement> _functions = new();
    private readonly Dictionary<string, IType> _builtInFunctions;
    private IDictionary<string, UnitType> _units = new Dictionary<string, UnitType>();


    public SemanticAnalyzerVisitor(BuiltInFunctionsProvider builtInFunctionsProvider)
    {
        var builtInFunctionsAndItsTypes = builtInFunctionsProvider.GetOneArgumentFunctionReturnTypes();
        _builtInFunctions = builtInFunctionsAndItsTypes;
    }

    public void Visit(TopLevel element)
    {
        _units = element.Units;

        foreach (var (name, function) in element.Functions)
        {
            _functions[name] = function;
        }

        foreach (var (name, functionStatement) in _functions)
        {
            _AddNewEmptyFunctionCallContext(name);
            functionStatement.Accept(this);
            _RemoveLastFunctionCallContext();
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
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }
    }

    public void Visit(VariableDeclaration element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);
        var variableType = element.Accept(typeVisitor);

        _GetLastScopeOfCurrentFunctionCall().Variables[element.Parameter.Name] = variableType;
    }

    public void Visit(IfStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);

        element.Condition.Accept(typeVisitor);

        _AddNewScopeToCurrentFunctionCallContext();
        element.Statements.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            elseIfStatement.Condition.Accept(typeVisitor);
            elseIfStatement.Accept(this);
        }

        _AddNewScopeToCurrentFunctionCallContext();
        element.ElseStatement.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();
    }

    public void Visit(ElseIfStatement element)
    {
        _AddNewScopeToCurrentFunctionCallContext();
        element.Statements.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();
    }

    public void Visit(WhileStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);

        element.Condition.Accept(typeVisitor);

        _AddNewScopeToCurrentFunctionCallContext();
        element.Statements.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();
    }

    public void Visit(AssignStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(FunctionCall element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(ReturnStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_semanticFunctionCallContexts, _functions, _builtInFunctions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(Parameter element)
    {
        if (_GetLastScopeOfCurrentFunctionCall().Variables.ContainsKey(element.Name))
        {
            throw new NotUniqueParametersNamesException(_GetNameOfCurrentFunctionName(), element.Name);
        }

        _GetLastScopeOfCurrentFunctionCall().Variables[element.Name] = element.Type;
    }

    private void _AddNewEmptyFunctionCallContext(string name)
    {
        var functionCallContext = new SemanticFunctionCallContext
        {
            FunctionName = name
        };
        functionCallContext.Scopes.AddLast(new SemanticScope());
        _semanticFunctionCallContexts.AddLast(functionCallContext);
    }

    private void _RemoveLastFunctionCallContext()
    {
        _semanticFunctionCallContexts.RemoveLast();
    }

    private void _AddNewScopeToCurrentFunctionCallContext()
    {
        _semanticFunctionCallContexts.Last().Scopes.AddLast(new SemanticScope());
    }

    private string _GetNameOfCurrentFunctionName()
    {
        return _semanticFunctionCallContexts.Last().FunctionName;
    }

    private void _RemoveLastScopeFromCurrentFunctionCallContext()
    {
        _semanticFunctionCallContexts.Last().Scopes.RemoveLast();
    }

    private SemanticScope _GetLastScopeOfCurrentFunctionCall()
    {
        return _semanticFunctionCallContexts.Last().Scopes.Last();
    }
}
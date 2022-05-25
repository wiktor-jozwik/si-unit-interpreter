using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.interpreter;

public class InterpreterVisitor : IVisitor
{
    private readonly FunctionCallContext _functionCallContext = new();
    private readonly Dictionary<string, dynamic> _functions = new();

    private readonly Dictionary<dynamic, Func<dynamic, dynamic>> _builtInFunctions;
    public InterpreterVisitor(Dictionary<dynamic, Func<dynamic, dynamic>> builtInFunctions)
    {
        _builtInFunctions = builtInFunctions;
    }

    public void Visit(TopLevel element)
    {
        foreach (var (name, function) in element.Functions)
        {
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
        _functionCallContext.Scopes.AddLast(new Scope());
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }

        _functionCallContext.Scopes.RemoveLast();
        
    }

    public void Visit(VariableDeclaration element)
    {
        var valueVisitor = new ValueVisitor(_functionCallContext, _functions);
        var variableValue = element.Expression.Accept(valueVisitor);
        
        _functionCallContext.Scopes.Last().Variables[element.Parameter.Name] = variableValue;
    }

    public void Visit(IfStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(ElseIfStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(WhileStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(AssignStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(ReturnStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(FunctionCall element)
    {
        var name = element.Name;
        var valueVisitor = new ValueVisitor(_functionCallContext, _functions);

        if (_builtInFunctions.TryGetValue(name, out var function))
        {
            var arguments = element.Arguments;
            var value = arguments[0].Accept(valueVisitor);

            function(value);
        }
    }

    public void Visit(Parameter element)
    {
        // _functionCallContext.Parameters[element.Name] = element.
    }
}
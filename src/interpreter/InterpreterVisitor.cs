using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.interpreter;

public class InterpreterVisitor : IInterpreterVisitor
{
    private readonly FunctionCallContext _functionCallContext = new();
    private readonly Dictionary<string, dynamic> _functions = new();

    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly Dictionary<string, Func<dynamic, dynamic, dynamic>> _twoArgumentFunctions;

    public InterpreterVisitor(IBuiltInFunctionsProvider builtInFunctionsProvider)
    {
        _oneArgumentFunctions = builtInFunctionsProvider.GetOneArgumentFunctions();
        _twoArgumentFunctions = builtInFunctionsProvider.GetTwoArgumentFunctions();
    }

    public dynamic Visit(TopLevel element)
    {
        foreach (var (name, function) in element.Functions)
        {
            function.Accept(this);
        }

        return true;
    }

    public dynamic Visit(FunctionStatement element)
    {
        foreach (var parameter in element.Parameters)
        {
            parameter.Accept(this);
        }

        element.Statements.Accept(this);

        return true;
    }

    public dynamic Visit(Block element)
    {
        _functionCallContext.Scopes.AddLast(new Scope());
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }

        _functionCallContext.Scopes.RemoveLast();

        return true;
    }

    public dynamic Visit(VariableDeclaration element)
    {
        var variableValue = element.Expression.Accept(this);

        _functionCallContext.Scopes.Last().Variables[element.Parameter.Name] = variableValue;

        return true;
    }

    public dynamic Visit(IfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);

        if (conditionValue)
        {
            element.Statements.Accept(this);
            return true;
        }

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            if (elseIfStatement.Accept(this))
            {
                return true;
            }
        }

        element.ElseStatement.Accept(this);
        return true;
    }

    public dynamic Visit(ElseIfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);
        if (conditionValue)
        {
            element.Statements.Accept(this);
            return true;
        }

        return false;
    }

    public dynamic Visit(WhileStatement element)
    {
        throw new NotImplementedException();
    }

    public dynamic Visit(AssignStatement element)
    {
        throw new NotImplementedException();
    }

    public dynamic Visit(ReturnStatement element)
    {
        throw new NotImplementedException();
    }

    public dynamic Visit(FunctionCall element)
    {
        var name = element.Name;

        if (_oneArgumentFunctions.TryGetValue(name, out var function))
        {
            var arguments = element.Arguments;
            var value = arguments[0].Accept(this);
            function(value);
        }

        return true;
    }

    public dynamic Visit(Parameter element)
    {
        // _functionCallContext.Parameters[element.Name] = element.
        return true;
    }

    public dynamic Visit(Expression element)
    {
        return element.Left.Accept(this) || element.Right.Accept(this);
    }

    public dynamic Visit(LogicFactor element)
    {
        return element.Left.Accept(this) && element.Right.Accept(this);
    }

    public dynamic Visit(EqualExpression element)
    {
        return element.Left.Accept(this) == element.Right.Accept(this);
    }

    public dynamic Visit(NotEqualExpression element)
    {
        return element.Left.Accept(this) != element.Right.Accept(this);
    }

    public dynamic Visit(GreaterThanExpression element)
    {
        return element.Left.Accept(this) > element.Right.Accept(this);
    }

    public dynamic Visit(GreaterEqualThanExpression element)
    {
        return element.Left.Accept(this) >= element.Right.Accept(this);
    }

    public dynamic Visit(SmallerThanExpression element)
    {
        return element.Left.Accept(this) < element.Right.Accept(this);
    }

    public dynamic Visit(SmallerEqualThanExpression element)
    {
        return element.Left.Accept(this) <= element.Right.Accept(this);
    }

    public dynamic Visit(AddExpression element)
    {
        return element.Left.Accept(this) + element.Right.Accept(this);
    }

    public dynamic Visit(SubtractExpression element)
    {
        return element.Left.Accept(this) - element.Right.Accept(this);
    }

    public dynamic Visit(MultiplicateExpression element)
    {
        return element.Left.Accept(this) * element.Right.Accept(this);
    }

    public dynamic Visit(DivideExpression element)
    {
        return (double) element.Left.Accept(this) / element.Right.Accept(this);
    }

    public dynamic Visit(MinusExpression element)
    {
        return element.Child.Accept(this) * -1;
    }

    public dynamic Visit(NotExpression element)
    {
        return !element.Child.Accept(this);
    }

    public dynamic Visit(Identifier element)
    {
        var name = element.Name;
        foreach (var scope in _functionCallContext.Scopes)
        {
            if (scope.Variables.TryGetValue(name, out var value))
            {
                return value;
            }
        }

        if (_functionCallContext.Parameters.TryGetValue(name, out var parameterValue))
        {
            return parameterValue;
        }

        return null!;
    }

    public dynamic Visit(BoolLiteral element)
    {
        return element.Value;
    }

    public dynamic Visit(FloatLiteral element)
    {
        return element.Value;
    }

    public dynamic Visit(IntLiteral element)
    {
        return element.Value;
    }

    public dynamic Visit(StringLiteral element)
    {
        return element.Value;
    }
}
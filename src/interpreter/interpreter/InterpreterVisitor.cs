using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.interpreter;

public class InterpreterVisitor : IInterpreterVisitor
{
    private readonly FunctionCallContext _functionCallContext = new();
    private readonly Dictionary<string, FunctionStatement> _functions = new();

    private readonly string _mainFunctionName;
    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly Dictionary<string, Func<dynamic, dynamic, dynamic>> _twoArgumentFunctions;

    public InterpreterVisitor(string mainFunctionName, BuiltInFunctionsProvider builtInFunctionsProvider)
    {
        _mainFunctionName = mainFunctionName;
        _oneArgumentFunctions = builtInFunctionsProvider.GetOneArgumentFunctions();
        _twoArgumentFunctions = builtInFunctionsProvider.GetTwoArgumentFunctions();
    }

    public dynamic? Visit(TopLevel element)
    {
        foreach (var (name, function) in element.Functions)
        {
            _functions[name] = function;
        }

        _functions[_mainFunctionName].Accept(this);

        return true;
    }

    public dynamic? Visit(FunctionStatement element)
    {
        _functionCallContext.Scopes.AddLast(new Scope());

        var parameterIndex = 0;
        foreach (var parameter in element.Parameters)
        {
            _functionCallContext.Scopes.Last().Variables[parameter.Accept(this)] =
                _functionCallContext.ParameterScopes.Last().Parameters[parameterIndex];
            parameterIndex++;
        }

        var functionReturnValue = element.Statements.Accept(this);
        
        _functionCallContext.Scopes.RemoveLast();

        return functionReturnValue;
    }

    public dynamic? Visit(Block element)
    {
        dynamic? blockValue = null;
        foreach (var statement in element.Statements)
        {
            blockValue = statement.Accept(this);
        }

        return blockValue;
    }

    public dynamic? Visit(VariableDeclaration element)
    {
        var variableValue = element.Expression.Accept(this);

        _functionCallContext.Scopes.Last().Variables[element.Parameter.Name] = variableValue;

        return null;
    }

    public dynamic? Visit(IfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);

        if (conditionValue)
        {
            _functionCallContext.Scopes.AddLast(new Scope());

            var value = element.Statements.Accept(this);
            
            _functionCallContext.Scopes.RemoveLast();

            return value;
        }

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            var elseIfValue = elseIfStatement.Accept(this);
            if (elseIfValue != null) {
                return elseIfValue;
            }
        }

        _functionCallContext.Scopes.AddLast(new Scope());
        var elseValue = element.ElseStatement.Accept(this);
        _functionCallContext.Scopes.RemoveLast();
        
        return elseValue;
    }

    public dynamic? Visit(ElseIfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);
        if (!conditionValue) return null;
        
        _functionCallContext.Scopes.AddLast(new Scope());
        var elseIfStatement = element.Statements.Accept(this);
        _functionCallContext.Scopes.RemoveLast();
        
        return elseIfStatement;
    }

    public dynamic? Visit(WhileStatement element)
    {
        var conditionValue = element.Condition.Accept(this);
        dynamic? value = null;
        while (conditionValue)
        {
            _functionCallContext.Scopes.AddLast(new Scope());
            value = element.Statements.Accept(this);
            _functionCallContext.Scopes.RemoveLast(); 
            
            conditionValue = element.Condition.Accept(this);
        }

        return value;
    }

    public dynamic? Visit(AssignStatement element)
    {
        var name = element.Identifier.Name;
        foreach (var scope in _functionCallContext.Scopes)
        {
            if (scope.Variables.ContainsKey(name))
            {
                var variableValue = element.Expression.Accept(this);
                scope.Variables[name] = variableValue!;
            }
        }

        return null;
    }

    public dynamic? Visit(ReturnStatement element)
    {
        return element.Expression?.Accept(this);
    }

    public dynamic? Visit(FunctionCall element)
    {
        var name = element.Name;

        if (_oneArgumentFunctions.TryGetValue(name, out var function))
        {
            var arguments = element.Arguments;
            var argValue = arguments[0].Accept(this);
            function(argValue);
            return true;
        }

        if (_functions.TryGetValue(name, out var functionStatement))
        {
            _functionCallContext.ParameterScopes.AddLast(new ParameterScope());
            // _functionCallContext.Parameters = new List<dynamic>();
            foreach (var argument in element.Arguments)
            {
                var argumentValue = argument.Accept(this);
                
                _functionCallContext.ParameterScopes.Last().Parameters.Add(argumentValue);

            }
            var functionValue = functionStatement.Accept(this);
            _functionCallContext.ParameterScopes.RemoveLast();
            return functionValue;
        }

        return null;
    }

    public dynamic? Visit(Parameter element)
    {
        return element.Name;
        
        // _functionCallContext.Scopes.Last().Variables[e]
        // _functionCallContext.Parameters[element.Name] = element.
        return null;
    }

    public dynamic? Visit(Expression element)
    {
        return element.Left.Accept(this) || element.Right.Accept(this);
    }

    public dynamic? Visit(LogicFactor element)
    {
        return element.Left.Accept(this) && element.Right.Accept(this);
    }

    public dynamic? Visit(EqualExpression element)
    {
        return element.Left.Accept(this) == element.Right.Accept(this);
    }

    public dynamic? Visit(NotEqualExpression element)
    {
        return element.Left.Accept(this) != element.Right.Accept(this);
    }

    public dynamic? Visit(GreaterThanExpression element)
    {
        return element.Left.Accept(this) > element.Right.Accept(this);
    }

    public dynamic? Visit(GreaterEqualThanExpression element)
    {
        return element.Left.Accept(this) >= element.Right.Accept(this);
    }

    public dynamic? Visit(SmallerThanExpression element)
    {
        return element.Left.Accept(this) < element.Right.Accept(this);
    }

    public dynamic? Visit(SmallerEqualThanExpression element)
    {
        return element.Left.Accept(this) <= element.Right.Accept(this);
    }

    public dynamic? Visit(AddExpression element)
    {
        return element.Left.Accept(this) + element.Right.Accept(this);
    }

    public dynamic? Visit(SubtractExpression element)
    {
        return element.Left.Accept(this) - element.Right.Accept(this);
    }

    public dynamic? Visit(MultiplicateExpression element)
    {
        return element.Left.Accept(this) * element.Right.Accept(this);
    }

    public dynamic? Visit(DivideExpression element)
    {
        var leftValue = element.Left.Accept(this);
        var rightValue = element.Right.Accept(this);
        // if (leftValue == null && rightValue == null)
        // {
        //     return null;
        // }

        return (double) leftValue! / rightValue;
    }

    public dynamic? Visit(MinusExpression element)
    {
        return element.Child.Accept(this) * -1;
    }

    public dynamic? Visit(NotExpression element)
    {
        return !element.Child.Accept(this);
    }

    public dynamic? Visit(Identifier element)
    {
        var name = element.Name;
        
        var scopeClone = new LinkedList<Scope>(_functionCallContext.Scopes);

        while (scopeClone.Count > 0)
        {
            if (scopeClone.Last().Variables.TryGetValue(name, out var value))
            {
                return value;
            }
            scopeClone.RemoveLast();
        }

        // foreach (var scope in _functionCallContext.Scopes)
        // {
        //     if (scope.Variables.TryGetValue(name, out var value))
        //     {
        //         return value;
        //     }
        // }

        // if (_functionCallContext.Parameters.TryGetValue(name, out var parameterValue))
        // {
        //     return parameterValue;
        // }

        return null!;
    }

    public dynamic? Visit(BoolLiteral element)
    {
        return element.Value;
    }

    public dynamic? Visit(FloatLiteral element)
    {
        return element.Value;
    }

    public dynamic? Visit(IntLiteral element)
    {
        return element.Value;
    }

    public dynamic? Visit(StringLiteral element)
    {
        return element.Value;
    }
}
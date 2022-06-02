using si_unit_interpreter.exceptions.interpreter;
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.interpreter;

public class InterpreterVisitor : IInterpreterVisitor
{
    private readonly LinkedList<FunctionCallContext> _functionCallContexts = new();
    private readonly Dictionary<string, FunctionStatement> _functions = new();

    private readonly string _mainFunctionName;
    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly int _maxIterationAllowed;

    public InterpreterVisitor(string mainFunctionName, BuiltInFunctionsProvider builtInFunctionsProvider,
        int maxIterationAllowed = 1_000_000)
    {
        _mainFunctionName = mainFunctionName;
        _oneArgumentFunctions = builtInFunctionsProvider.GetOneArgumentFunctions();
        _maxIterationAllowed = maxIterationAllowed;
    }

    public dynamic Visit(TopLevel element)
    {
        foreach (var (name, function) in element.Functions)
        {
            _functions[name] = function;
        }

        if (_functions.TryGetValue(_mainFunctionName, out var mainFunction))
        {
            _AddNewEmptyFunctionCallContext();

            mainFunction.Accept(this);
        }
        else
        {
            throw new LackOfMainFunctionException();
        }

        return true;
    }

    public dynamic? Visit(FunctionStatement element)
    {
        var functionReturnValue = element.Statements.Accept(this);

        if (functionReturnValue == null)
        {
            if (element.ReturnType.GetType() != typeof(VoidType))
            {
                throw new LackOfValidReturnException();
            }
        }
        else
        {
            if (functionReturnValue.GetType() == typeof(VoidReturn))
            {
                if (element.ReturnType.GetType() != typeof(VoidType))
                {
                    throw new LackOfValidReturnException();
                }
            }
        }

        return functionReturnValue;
    }

    public dynamic? Visit(Block element)
    {
        foreach (var statement in element.Statements)
        {
            var blockValue = statement.Accept(this);
            if (blockValue != null)
            {
                return blockValue;
            }
        }

        return null;
    }

    public dynamic? Visit(VariableDeclaration element)
    {
        var variableValue = element.Expression.Accept(this);

        _GetLastScopeOfCurrentFunctionCall().Variables[element.Parameter.Name] = variableValue!;

        return null;
    }

    public dynamic? Visit(IfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);

        if (conditionValue)
        {
            _AddNewScopeToCurrentFunctionCallContext();
            var value = element.Statements.Accept(this);
            _RemoveLastScopeFromCurrentFunctionCallContext();

            return value;
        }

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            var elseIfCondition = elseIfStatement.Condition.Accept(this);
            if (!elseIfCondition) continue;
            
            return elseIfStatement.Accept(this);
        }

        _AddNewScopeToCurrentFunctionCallContext();
        var elseValue = element.ElseStatement.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();

        return elseValue;
    }

    public dynamic? Visit(ElseIfStatement element)
    {
        var conditionValue = element.Condition.Accept(this);
        if (!conditionValue) return null;

        _AddNewScopeToCurrentFunctionCallContext();
        var elseIfStatementValue = element.Statements.Accept(this);
        _RemoveLastScopeFromCurrentFunctionCallContext();

        return elseIfStatementValue;
    }

    public dynamic? Visit(WhileStatement element)
    {
        var conditionValue = element.Condition.Accept(this);
        dynamic? value = null;
        var iteration = 0;
        while (conditionValue)
        {
            _AddNewScopeToCurrentFunctionCallContext();
            value = element.Statements.Accept(this);
            _RemoveLastScopeFromCurrentFunctionCallContext();

            if (value != null)
            {
                return value;
            }

            conditionValue = element.Condition.Accept(this);
            iteration += 1;
            if (iteration >= _maxIterationAllowed)
            {
                throw new MaxNumberIterationReachedException();
            }
        }

        return value;
    }

    public dynamic? Visit(AssignStatement element)
    {
        var name = element.Identifier.Name;
        foreach (var scope in _GetAllScopesOfCurrentFunctionCall())
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
        return element.Expression == null ? new VoidReturn() : element.Expression?.Accept(this);
    }

    public dynamic? Visit(FunctionCall element)
    {
        var name = element.Name;

        if (_oneArgumentFunctions.TryGetValue(name, out var function))
        {
            var arguments = element.Arguments;
            var argValue = arguments[0].Accept(this);
            function(argValue);
            return null;
        }

        if (_functions.TryGetValue(name, out var functionStatement))
        {
            var argumentsValues = element.Arguments.Select(arg => arg.Accept(this)).ToList();
            
            _AddNewEmptyFunctionCallContext();
            foreach (var (argumentValue, functionParameter) in argumentsValues.Zip(functionStatement.Parameters))
            {
                _GetLastScopeOfCurrentFunctionCall().Variables[functionParameter.Name] = argumentValue!;
            }
            
            var functionValue = functionStatement.Accept(this);
            
            _RemoveLastFunctionCallContext();
            return functionValue;
        }

        return null;
    }

    // czy potrzebne?
    public dynamic Visit(Parameter element)
    {
        return element.Name;
    }

    public dynamic? Visit(OrExpression element)
    {
        return element.Left.Accept(this) || element.Right.Accept(this);
    }

    public dynamic? Visit(AndExpression element)
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
        return (double) element.Left.Accept(this)! / element.Right.Accept(this);
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

        var scopeClone = new LinkedList<Scope>(_GetAllScopesOfCurrentFunctionCall());

        while (scopeClone.Count > 0)
        {
            if (scopeClone.Last().Variables.TryGetValue(name, out var value))
            {
                return value;
            }

            scopeClone.RemoveLast();
        }

        return null;
    }

    
    // dodac obiekt z value i unitTypem?
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

    private void _AddNewEmptyFunctionCallContext()
    {
        var functionCallContext = new FunctionCallContext();
        functionCallContext.Scopes.AddLast(new Scope());
        _functionCallContexts.AddLast(functionCallContext);
    }

    private void _RemoveLastFunctionCallContext()
    {
        _functionCallContexts.RemoveLast();
    }

    private void _AddNewScopeToCurrentFunctionCallContext()
    {
        _functionCallContexts.Last().Scopes.AddLast(new Scope());
    }
    
    private void _RemoveLastScopeFromCurrentFunctionCallContext()
    {
        _functionCallContexts.Last().Scopes.RemoveLast();
    }

    private LinkedList<Scope> _GetAllScopesOfCurrentFunctionCall()
    {
        return _functionCallContexts.Last().Scopes;
    }

    private Scope _GetLastScopeOfCurrentFunctionCall()
    {
        return _functionCallContexts.Last().Scopes.Last();
    }
}
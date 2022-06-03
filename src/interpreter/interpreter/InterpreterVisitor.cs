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
    private readonly Dictionary<string, FunctionStatement> _functions;

    private readonly string _mainFunctionName;
    private readonly int _maxIterationAllowed;

    public InterpreterVisitor(string mainFunctionName, BuiltInFunctionsProvider builtInFunctionsProvider,
        int maxIterationAllowed = 1_000_000)
    {
        _functions = builtInFunctionsProvider.GetBuiltInFunctions();
        _mainFunctionName = mainFunctionName;
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

        _GetLastScopeOfCurrentFunctionCall().Variables[element.Parameter.Name] =
            new LiteralValue(variableValue!, _TryGetVariableUnitType(element.Parameter.Type));

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
            if (scope.Variables.TryGetValue(name, out var literalValue))
            {
                var variableValue = element.Expression.Accept(this);
                literalValue.Value = variableValue!;
                scope.Variables[name] = literalValue;
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

        if (!_functions.TryGetValue(name, out var functionStatement)) return null;

        var argumentsValues = new List<LiteralValue>();

        foreach (var argument in element.Arguments)
        {
            var argumentValue = argument.Accept(this);

            if (argument.GetType() == typeof(Identifier))
            {
                var argumentName = ((Identifier) argument).Name;
                foreach (var scope in _GetAllScopesOfCurrentFunctionCall())
                {
                    scope.Variables.TryGetValue(argumentName, out var literalValue);
                    {
                        argumentsValues.Add(literalValue!.Copy());
                        break;
                    }
                }
            }
            else if (argument.GetType() == typeof(FunctionCall))
            {
                argumentsValues.Add(new LiteralValue(argumentValue!, _TryGetVariableUnitType(_functions[((FunctionCall) argument).Name].ReturnType)));
            }
            else
            {
                argumentsValues.Add(new LiteralValue(argumentValue, null));
            }
        }

        _AddNewEmptyFunctionCallContext();
        foreach (var (argumentValue, functionParameter) in argumentsValues.Zip(functionStatement.Parameters))
        {
            _GetLastScopeOfCurrentFunctionCall().Variables[functionParameter.Name] = argumentValue;
        }

        var functionValue = functionStatement.Accept(this);
        _RemoveLastFunctionCallContext();

        return functionValue;
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
        var right = element.Right.Accept(this);

        if (right == 0)
        {
            throw new ZeroDivisionError();
        }

        return (double) element.Left.Accept(this)! / right;
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
            if (scopeClone.Last().Variables.TryGetValue(name, out var variable))
            {
                return variable.Value;
            }

            scopeClone.RemoveLast();
        }

        return null;
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

    public dynamic? Visit(BuiltInFunction element)
    {
        var literalValue = _GetLastScopeOfCurrentFunctionCall().Variables[element.Name];
        element.Function(literalValue);
        return null;
    }

    public dynamic Visit(Parameter element)
    {
        // as it's not needed
        throw new NotImplementedException();
    }

    // private

    private UnitType? _TryGetVariableUnitType(IType type)
    {
        if (type.GetType() == typeof(UnitType))
        {
            return (UnitType) type;
        }

        return null;
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
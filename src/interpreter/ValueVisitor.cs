using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;

namespace si_unit_interpreter.interpreter;

public class ValueVisitor: IValueVisitor
{
    private readonly FunctionCallContext _functionCallContext;
    private readonly Dictionary<string, dynamic> _functions;
    
    public ValueVisitor(FunctionCallContext functionCallContext, Dictionary<string, dynamic> functions)
    {
        _functionCallContext = functionCallContext;
        _functions = functions;
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
        return (decimal) element.Left.Accept(this) / element.Right.Accept(this);
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

    public dynamic Visit(FunctionCall element)
    {
        var name = element.Name;

        if (_functions.TryGetValue(name, out var value))
        {
            return value;
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
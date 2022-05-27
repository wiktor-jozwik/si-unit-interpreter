using si_unit_interpreter.interpreter;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.multiplicative;

public class DivideExpression : IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public DivideExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
    
    public dynamic Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
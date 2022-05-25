using si_unit_interpreter.interpreter;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.negate;

public class NotExpression : IExpression
{
    public readonly IExpression Child;

    public NotExpression(IExpression child)
    {
        Child = child;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
    
    public dynamic Accept(IValueVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
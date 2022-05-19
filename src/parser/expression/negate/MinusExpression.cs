using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.negate;

public class MinusExpression: IExpression
{
    public readonly IExpression Child;

    public MinusExpression(IExpression child)
    {
        Child = child;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
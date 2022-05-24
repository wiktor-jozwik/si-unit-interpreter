using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.multiplicative;

public class MultiplicateExpression : IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public MultiplicateExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
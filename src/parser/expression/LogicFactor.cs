using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public class LogicFactor: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public LogicFactor(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
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
namespace si_unit_interpreter.parser.expression.comparison;

public class NotEqualExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public NotEqualExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
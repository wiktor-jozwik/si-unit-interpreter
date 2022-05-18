namespace si_unit_interpreter.parser.expression.comparison;

public class GreaterThanExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public GreaterThanExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
namespace si_unit_interpreter.parser.expression.comparison;

public class NotEqualExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public NotEqualExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
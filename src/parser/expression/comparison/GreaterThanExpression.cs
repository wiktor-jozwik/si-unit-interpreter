namespace si_unit_interpreter.parser.expression.comparison;

public class GreaterThanExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public GreaterThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
namespace si_unit_interpreter.parser.expression.comparison;

public class GreaterEqualThanExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public GreaterEqualThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
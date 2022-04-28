namespace si_unit_interpreter.parser.expression.comparison;

public class SmallerEqualThanExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public SmallerEqualThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
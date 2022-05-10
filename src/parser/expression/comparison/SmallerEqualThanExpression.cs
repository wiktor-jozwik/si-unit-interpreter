namespace si_unit_interpreter.parser.expression.comparison;

public class SmallerEqualThanExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression? Right;

    public SmallerEqualThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
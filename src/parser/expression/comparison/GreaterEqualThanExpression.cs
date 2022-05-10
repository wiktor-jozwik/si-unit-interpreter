namespace si_unit_interpreter.parser.expression.comparison;

public class GreaterEqualThanExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression? Right;

    public GreaterEqualThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
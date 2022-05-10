namespace si_unit_interpreter.parser.expression.comparison;

public class SmallerThanExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression? Right;

    public SmallerThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
namespace si_unit_interpreter.parser.expression.comparison;

public class SmallerThanExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public SmallerThanExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
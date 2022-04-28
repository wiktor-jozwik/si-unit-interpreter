namespace si_unit_interpreter.parser.expression.comparison;

public class EqualExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public EqualExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
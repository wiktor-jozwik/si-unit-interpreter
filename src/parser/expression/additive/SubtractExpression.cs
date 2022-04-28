namespace si_unit_interpreter.parser.expression.additive;

public class SubtractExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public SubtractExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
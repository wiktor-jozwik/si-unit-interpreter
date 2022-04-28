namespace si_unit_interpreter.parser.expression.multiplicative;

public class MultiplicateExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public MultiplicateExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
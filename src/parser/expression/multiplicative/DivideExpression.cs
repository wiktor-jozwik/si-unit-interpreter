namespace si_unit_interpreter.parser.expression.multiplicative;

public class DivideExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public DivideExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
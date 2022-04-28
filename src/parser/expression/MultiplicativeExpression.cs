namespace si_unit_interpreter.parser.expression;

public class MultiplicationExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public MultiplicationExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
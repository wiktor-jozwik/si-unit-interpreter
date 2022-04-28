namespace si_unit_interpreter.parser.expression.additive;

public class AddExpression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public AddExpression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
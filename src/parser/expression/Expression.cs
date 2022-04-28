namespace si_unit_interpreter.parser.expression;

public class Expression: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public Expression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
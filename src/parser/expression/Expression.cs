namespace si_unit_interpreter.parser.expression;

public class Expression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression? Right;

    public Expression(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
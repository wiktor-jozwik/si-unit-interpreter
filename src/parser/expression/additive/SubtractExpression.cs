namespace si_unit_interpreter.parser.expression.additive;

public class SubtractExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public SubtractExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }
}
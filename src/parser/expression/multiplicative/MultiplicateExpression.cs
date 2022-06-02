namespace si_unit_interpreter.parser.expression.multiplicative;

public class MultiplicateExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public MultiplicateExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }
}
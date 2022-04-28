namespace si_unit_interpreter.parser.expression;

public class LogicFactor: IExpression
{
    public IExpression Left;
    public IExpression? Right;

    public LogicFactor(IExpression left, IExpression? right)
    {
        Left = left;
        Right = right;
    }
}
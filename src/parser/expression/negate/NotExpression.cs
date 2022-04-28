namespace si_unit_interpreter.parser.expression.negate;

public class NotExpression: IExpression
{
    public IExpression Child;

    public NotExpression(IExpression child)
    {
        Child = child;
    }
}
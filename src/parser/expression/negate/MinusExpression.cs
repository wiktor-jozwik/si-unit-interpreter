namespace si_unit_interpreter.parser.expression.negate;

public class MinusExpression: IExpression
{
    public IExpression Child;

    public MinusExpression(IExpression child)
    {
        Child = child;
    }
}
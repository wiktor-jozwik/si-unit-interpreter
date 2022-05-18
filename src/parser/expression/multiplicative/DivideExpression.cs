namespace si_unit_interpreter.parser.expression.multiplicative;

public class DivideExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public DivideExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
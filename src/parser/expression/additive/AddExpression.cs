namespace si_unit_interpreter.parser.expression.additive;

public class AddExpression: IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public AddExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
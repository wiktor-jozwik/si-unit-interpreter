namespace si_unit_interpreter.parser.expression;

public class FunctionCall: IExpression
{
    public readonly string Name;
    public readonly List<IExpression> Arguments;

    public FunctionCall(string name, List<IExpression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public class Identifier: IExpression
{
    public readonly string Name;

    public Identifier(string name)
    {
        Name = name;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
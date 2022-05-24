using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class StringLiteral: IExpression
{
    public readonly string Value;

    public StringLiteral(string value)
    {
        Value = value;
    }
    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
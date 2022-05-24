using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class BoolLiteral : IExpression
{
    public readonly bool Value;

    public BoolLiteral(bool value)
    {
        Value = value;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
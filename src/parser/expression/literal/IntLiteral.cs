using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class IntLiteral: IExpression
{
    public readonly long Value;
    public readonly UnitType? UnitType;
    
    public IntLiteral(long value, UnitType? unitType)
    {
        Value = value;
        UnitType = unitType;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
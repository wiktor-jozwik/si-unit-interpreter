using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class FloatLiteral: IExpression
{
    public readonly double Value;
    public readonly UnitType UnitType;
    
    public FloatLiteral(double value, UnitType unitType)
    {
        Value = value;
        UnitType = unitType;
    }
    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
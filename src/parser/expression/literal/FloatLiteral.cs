using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class FloatLiteral: IExpression
{
    public double Value;
    public UnitType? UnitType;
    
    public FloatLiteral(double value, UnitType? unitType)
    {
        Value = value;
        UnitType = unitType;
    } 
}
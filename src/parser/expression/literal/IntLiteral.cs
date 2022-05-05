using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class IntLiteral
{
    public int Value;
    public UnitType? UnitType;
    
    public IntLiteral(int value, UnitType? unitType)
    {
        Value = value;
        UnitType = unitType;
    } 
}
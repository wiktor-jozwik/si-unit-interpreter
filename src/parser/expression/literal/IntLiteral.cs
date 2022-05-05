using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class IntLiteral: IExpression
{
    public long Value;
    public UnitType? UnitType;
    
    public IntLiteral(long value, UnitType? unitType)
    {
        Value = value;
        UnitType = unitType;
    } 
}
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.interpreter;

public class LiteralValue
{
    public dynamic Value;
    public readonly UnitType? UnitType;

    public LiteralValue(dynamic value, UnitType? unitType)
    {
        Value = value;
        UnitType = unitType;
    }

    public LiteralValue Copy()
    {
        return new LiteralValue(Value, UnitType);
    }
}
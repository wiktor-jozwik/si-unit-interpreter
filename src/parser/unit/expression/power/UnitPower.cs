namespace si_unit_interpreter.parser.unit.expression.power;

public class UnitPower: IUnitPower
{
    public readonly long Value;

    public UnitPower(long value)
    {
        Value = value;
    }
}
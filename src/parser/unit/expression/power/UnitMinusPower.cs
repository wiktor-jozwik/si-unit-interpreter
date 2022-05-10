namespace si_unit_interpreter.parser.unit.expression.power;

public class UnitMinusPower: IUnitPower
{
    public readonly long Value;

    public UnitMinusPower(long value)
    {
        Value = value;
    }
}
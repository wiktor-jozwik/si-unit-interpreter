using si_unit_interpreter.parser.unit.expression.power;

namespace si_unit_interpreter.parser.unit.expression;

public class UnitUnaryExpression: IUnitExpression
{
    public readonly string Name;
    public readonly IUnitPower? UnitPower;

    public UnitUnaryExpression(string name, IUnitPower? unitPower)
    {
        Name = name;
        UnitPower = unitPower;
    }
}
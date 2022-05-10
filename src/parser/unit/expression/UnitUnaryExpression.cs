using si_unit_interpreter.parser.unit.expression.power;

namespace si_unit_interpreter.parser.unit.expression;

public class UnitUnaryExpression: IUnitExpression
{
    public readonly string Identifier;
    public readonly IUnitPower? UnitPower;

    public UnitUnaryExpression(string identifier, IUnitPower? unitPower)
    {
        Identifier = identifier;
        UnitPower = unitPower;
    }
}
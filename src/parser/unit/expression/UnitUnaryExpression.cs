namespace si_unit_interpreter.parser.unit.expression;

public class UnitUnaryExpression: IUnitExpression
{
    public string Identifier;
    public IUnitPower UnitPower;

    public UnitUnaryExpression(string identifier, IUnitPower unitPower)
    {
        Identifier = identifier;
        UnitPower = unitPower;
    }
}
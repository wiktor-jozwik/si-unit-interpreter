using si_unit_interpreter.parser.unit.expression;

namespace si_unit_interpreter.parser.type;

public class UnitType: IType
{
    public readonly IUnitExpression? Expression;

    public UnitType(IUnitExpression? expression)
    {
        Expression = expression;
    }
}
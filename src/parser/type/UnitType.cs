using si_unit_interpreter.parser.unit.expression;

namespace si_unit_interpreter.parser.type;

using si_unit_interpreter.parser.unit;

public class UnitType: IType
{
    public IUnitExpression? Expression;

    public UnitType(IUnitExpression? expression)
    {
        Expression = expression;
    }
}
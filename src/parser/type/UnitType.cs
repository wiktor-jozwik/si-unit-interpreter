using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.parser.type;

public class UnitType: IType
{
    public IList<Unit> Units;

    public UnitType(IList<Unit> units)
    {
        Units = units;
    }
}
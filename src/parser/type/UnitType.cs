using System.Text;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.parser.type;

public class UnitType : IType
{
    public IList<Unit> Units;

    public UnitType(IList<Unit> units)
    {
        Units = units;
    }

    public string Format()
    {
        var unitStringBuilder = new StringBuilder();

        foreach (var unit in Units)
        {
            var power = "";
            if (unit.Power != 1)
            {
                power = $"^{unit.Power}";
            }

            unitStringBuilder.Append($"{unit.Name}{power}*");
        }

        // Remove added * in the end after all
        if (unitStringBuilder.Length > 0)
        {
            unitStringBuilder.Remove(unitStringBuilder.Length - 1, 1);
        }

        return $"[{unitStringBuilder}]";
    }
}
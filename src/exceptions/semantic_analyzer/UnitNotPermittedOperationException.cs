using System.Text;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class UnitNotPermittedOperationException: Exception
{
    public UnitNotPermittedOperationException(IList<Unit> leftUnit, IList<Unit> rightUnit)
        : base($"[{FormatUnit(rightUnit)}] type does not match required [{FormatUnit(leftUnit)}] type") {}

    private static string FormatUnit(IList<Unit> units)
    {
        var unitStringBuilder = new StringBuilder();
        
        foreach (var unit in units)
        {
            var power = "";
            if (unit.Power != 1)
            {
                power = $"^{unit.Power}";
            }
            unitStringBuilder.Append($"{unit.Name}{power}*");
        }

        // Remove added * in the end
        if (unitStringBuilder.Length > 0)
        {
            unitStringBuilder.Remove(unitStringBuilder.Length - 1, 1);
        }

        return unitStringBuilder.ToString();
    }
}
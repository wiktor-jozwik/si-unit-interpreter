using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser;

public class TopLevel
{
    public readonly IDictionary<string, FunctionStatement> Functions;
    public readonly IDictionary<string, UnitType> Units;

    public TopLevel(
        IDictionary<string, FunctionStatement> functions,
        IDictionary<string, UnitType> units
        )
    {
        Functions = functions;
        Units = units;
    }
}

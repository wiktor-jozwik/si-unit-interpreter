using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser;

public class Program
{
    public readonly IList<IStatement> Statements;
    public readonly IDictionary<string, IList<IStatement>> Functions;
    public readonly IDictionary<string, UnitType> Units;

    public Program(
        IList<IStatement> statements, 
        IDictionary<string, IList<IStatement>> functions,
        IDictionary<string, UnitType> units
        )
    {
        Statements = statements;
        Functions = functions;
        Units = units;
    }
}

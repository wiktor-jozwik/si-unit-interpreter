using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser;

public class TopLevel : IStatement
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

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public dynamic? Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.parser;

public class Program
{
    public IDictionary<string, IStatement> Statements;

    public Program(IDictionary<string, IStatement> statements)
    {
        Statements = statements;
    }
}

using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.parser;

public class Program
{
    private readonly IList<IStatement> _statements = new List<IStatement>();
    private readonly IDictionary<string, IList<IStatement>> _functions = new Dictionary<string, IList<IStatement>>();

    public IList<IStatement> Statements;
    public IDictionary<string, IList<IStatement>> Functions;

    public Program(
        IList<IStatement> statements, 
        IDictionary<string, IList<IStatement>> functions
        )
    {
        Statements = statements;
        Functions = functions;
    }
}

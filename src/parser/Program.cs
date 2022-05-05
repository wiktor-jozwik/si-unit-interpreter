using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.parser;

public class Program
{
    private readonly IList<IStatement> _statements = new List<IStatement>();
    private readonly IDictionary<string, IList<IStatement>> _functions = new Dictionary<string, IList<IStatement>>();
    private readonly SortedSet<AssignStatement> _variables = new SortedSet<AssignStatement>();

    public IList<IStatement> Statements;
    public IDictionary<string, IList<IStatement>> Functions;
    public SortedSet<AssignStatement> Variables;

    public Program(
        IList<IStatement> statements, 
        IDictionary<string, IList<IStatement>> functions, 
        SortedSet<AssignStatement> variables
        )
    {
        Statements = statements;
        Functions = functions;
        Variables = variables;
    }
}

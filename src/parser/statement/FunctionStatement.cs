using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class FunctionStatement: IStatement
{
    public string Name;
    public IList<Parameter> Parameters;
    public IType ReturnType;
    public IList<IStatement> Statements;

    public FunctionStatement(string name, IList<Parameter> parameters, IType returnType, IList<IStatement> statements)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        Statements = statements;
    }
}
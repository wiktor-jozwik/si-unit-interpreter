using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class FunctionStatement: IStatement
{
    public readonly string Name;
    public readonly IList<Parameter> Parameters;
    public readonly IType ReturnType;
    public readonly IList<IStatement> Statements;

    public FunctionStatement(string name, IList<Parameter> parameters, IType returnType, IList<IStatement> statements)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        Statements = statements;
    }
}
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class FunctionStatement: IStatement
{
    public readonly string Name;
    public readonly IList<Parameter> Parameters;
    public readonly IType ReturnType;
    public readonly Block Statements;

    public FunctionStatement(string name, IList<Parameter> parameters, IType returnType, Block statements)
    {
        Name = name;
        Parameters = parameters;
        ReturnType = returnType;
        Statements = statements;
    }
}
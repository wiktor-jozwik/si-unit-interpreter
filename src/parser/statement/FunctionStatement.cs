namespace si_unit_interpreter.parser.statement;

public class FunctionStatement: IStatement
{
    public string Name;
    public IList<Parameter> Parameters;
    public Type ReturnType;
    public IList<IStatement> Statements;
}
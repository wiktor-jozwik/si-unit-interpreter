namespace si_unit_interpreter.parser.statement;

public class Block: IStatement
{
    public readonly IList<IStatement> Statements;

    public Block()
    {
        Statements = new List<IStatement>();
    }

    public Block(IList<IStatement> statements)
    {
        Statements = statements;
    }
}
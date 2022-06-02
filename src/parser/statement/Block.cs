using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;

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
    
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public dynamic? Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
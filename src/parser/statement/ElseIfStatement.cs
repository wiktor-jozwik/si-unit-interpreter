using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class ElseIfStatement: IStatement
{
    public readonly IExpression Condition;
    public readonly Block Statements;

    public ElseIfStatement(
        IExpression condition,
        Block statements
    )
    {
        Condition = condition;
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
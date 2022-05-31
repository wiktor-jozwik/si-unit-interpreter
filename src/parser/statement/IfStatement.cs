using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class IfStatement : IStatement
{
    public readonly IExpression Condition;
    public readonly Block Statements;
    public readonly IList<ElseIfStatement> ElseIfStatements;
    public readonly Block ElseStatement;

    public IfStatement(
        IExpression condition,
        Block statements,
        IList<ElseIfStatement> elseIfStatements,
        Block elseStatement
    )
    {
        Condition = condition;
        Statements = statements;
        ElseIfStatements = elseIfStatements;
        ElseStatement = elseStatement;
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
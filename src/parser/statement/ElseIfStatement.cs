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
}
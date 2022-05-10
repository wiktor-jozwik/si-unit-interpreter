using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class ElseIfStatement: IStatement
{
    public readonly IExpression Condition;
    public readonly IList<IStatement> Statements;

    public ElseIfStatement(
        IExpression condition,
        IList<IStatement> statements
    )
    {
        Condition = condition;
        Statements = statements;
    }
}
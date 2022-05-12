using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class WhileStatement: IStatement
{
    public readonly IExpression Condition;
    public readonly Block Statements;

    public WhileStatement(IExpression condition, Block statements)
    {
        Condition = condition;
        Statements = statements;
    }
}
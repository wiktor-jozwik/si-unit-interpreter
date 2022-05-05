using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class WhileStatement: IStatement
{
    public IExpression Condition;
    public IList<IStatement> Statements;

    public WhileStatement(IExpression condition, IList<IStatement> statements)
    {
        Condition = condition;
        Statements = statements;
    }
}
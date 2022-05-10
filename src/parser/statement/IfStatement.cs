using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class IfStatement: IStatement
{
    public readonly IExpression Condition;
    public readonly IList<IStatement> Statements;
    public readonly IList<ElseIfStatement>? ElseIfStatements;
    public readonly IList<IStatement>? ElseStatement;

    public IfStatement(
        IExpression condition,
        IList<IStatement> statements,
        IList<ElseIfStatement> elseIfStatements,
        IList<IStatement> elseStatement
    )
    {
        Condition = condition;
        Statements = statements;
        ElseIfStatements = elseIfStatements;
        ElseStatement = elseStatement;
    }
}
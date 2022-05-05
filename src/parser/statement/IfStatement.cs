using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class IfStatement: IStatement
{
    public IExpression Condition;
    public IList<IStatement> Statements;
    public IList<ElseIfStatement>? ElseIfStatements;
    public IList<IStatement>? ElseStatement;

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
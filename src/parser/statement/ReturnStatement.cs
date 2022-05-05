using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class ReturnStatement: IStatement
{
    public IExpression? Expression;

    public ReturnStatement(IExpression? expression)
    {
        Expression = expression;
    }
}
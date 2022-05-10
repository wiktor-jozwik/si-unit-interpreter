using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class ReturnStatement: IStatement
{
    public readonly IExpression? Expression;

    public ReturnStatement(IExpression? expression)
    {
        Expression = expression;
    }
}
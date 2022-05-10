using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class AssignStatement: IStatement
{
    public readonly Parameter Parameter;
    public readonly IExpression Expression;

    public AssignStatement(Parameter parameter, IExpression expression)
    {
        Parameter = parameter;
        Expression = expression;
    }
}
using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class AssignStatement: IStatement
{
    public readonly string Name;
    public readonly IExpression Expression;

    public AssignStatement(string name, IExpression expression)
    {
        Name = name;
        Expression = expression;
    }
}
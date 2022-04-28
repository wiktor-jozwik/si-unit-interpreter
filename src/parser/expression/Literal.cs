namespace si_unit_interpreter.parser.expression;

public class Literal: IExpression
{
    public dynamic Value;

    public Literal(dynamic value)
    {
        Value = value;
    }
}
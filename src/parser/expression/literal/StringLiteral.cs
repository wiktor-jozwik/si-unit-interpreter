namespace si_unit_interpreter.parser.expression.literal;

public class StringLiteral: IExpression
{
    public readonly string Value;

    public StringLiteral(string value)
    {
        Value = value;
    }
}
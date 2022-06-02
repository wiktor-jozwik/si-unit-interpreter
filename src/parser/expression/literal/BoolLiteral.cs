namespace si_unit_interpreter.parser.expression.literal;

public class BoolLiteral: IExpression
{
    public readonly bool Value;

    public BoolLiteral(bool value)
    {
        Value = value;
    }
}
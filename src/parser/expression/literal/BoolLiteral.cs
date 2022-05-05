namespace si_unit_interpreter.parser.expression.literal;

public class BoolLiteral: IExpression
{
    public bool Value;

    public BoolLiteral(bool value)
    {
        Value = value;
    }
}
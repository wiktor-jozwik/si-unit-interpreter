namespace si_unit_interpreter.parser.expression;

public class Identifier: IExpression
{
    public string Name;

    public Identifier(string name)
    {
        Name = name;
    }
}
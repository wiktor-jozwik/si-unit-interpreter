namespace si_unit_interpreter.parser.expression;

public class FunctionCall: IExpression
{
    public string Name;
    public List<IExpression> Arguments;

    public FunctionCall(string name, List<IExpression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }
}
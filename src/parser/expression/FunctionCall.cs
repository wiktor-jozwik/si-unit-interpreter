using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.parser.expression;

public class FunctionCall: IExpression, IStatement
{
    public readonly string Name;
    public readonly List<IExpression> Arguments;

    public FunctionCall(string name, List<IExpression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }
}
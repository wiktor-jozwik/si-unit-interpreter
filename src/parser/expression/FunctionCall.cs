using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public class FunctionCall : IExpression, IStatement
{
    public readonly string Name;
    public readonly List<IExpression> Arguments;

    public FunctionCall(string name, List<IExpression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
using si_unit_interpreter.interpreter;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class StringLiteral : IExpression, IVisitable<string>
{
    public readonly string Value;

    public StringLiteral(string value)
    {
        Value = value;
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
    
    public string Accept(IVisitor<string> visitor)
    {
        return visitor.Visit(this);
    }
    
    public dynamic Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
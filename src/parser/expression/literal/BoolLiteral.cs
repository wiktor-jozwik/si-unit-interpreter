using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class BoolLiteral : IExpression
{
    public readonly bool Value;

    public BoolLiteral(bool value)
    {
        Value = value;
    }

    public IType Accept(ITypeVisitor visitor)
    {
        return visitor.Visit(this);
    }
    
    public dynamic? Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression.literal;

public class FloatLiteral : IExpression
{
    public readonly double Value;
    public readonly UnitType UnitType;

    public FloatLiteral(double value, UnitType unitType)
    {
        Value = value;
        UnitType = unitType;
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
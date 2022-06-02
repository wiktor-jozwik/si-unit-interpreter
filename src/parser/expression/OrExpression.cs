using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public class OrExpression : IExpression
{
    public readonly IExpression Left;
    public readonly IExpression Right;

    public OrExpression(IExpression left, IExpression right)
    {
        Left = left;
        Right = right;
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
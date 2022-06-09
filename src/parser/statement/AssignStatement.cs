using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class AssignStatement : ITypeCheck, IStatement
{
    public readonly Identifier Identifier;
    public readonly IExpression Expression;

    public AssignStatement(Identifier identifier, IExpression expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
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
using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class VariableDeclaration : ITypeCheck, IStatement
{
    public readonly Parameter Parameter;
    public readonly IExpression Expression;

    public VariableDeclaration(Parameter parameter, IExpression expression)
    {
        Parameter = parameter;
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
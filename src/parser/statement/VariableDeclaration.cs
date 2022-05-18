using si_unit_interpreter.parser.expression;

namespace si_unit_interpreter.parser.statement;

public class VariableDeclaration: IStatement
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
}
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class ReturnStatement: ITypeCheck, IStatement
{
    public readonly IExpression? Expression;

    public ReturnStatement(IExpression? expression)
    {
        Expression = expression;
    }
    
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public IType Accept(IVisitor<IType> visitor)
    {
        return visitor.Visit(this);
    }
}
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class AssignStatement: IVisitable<IType>, IStatement
{
    public readonly string Name;
    public readonly IExpression Expression;

    public AssignStatement(string name, IExpression expression)
    {
        Name = name;
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
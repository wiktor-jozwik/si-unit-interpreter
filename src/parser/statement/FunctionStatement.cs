using si_unit_interpreter.interpreter;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class FunctionStatement : IStatement
{
    public readonly IList<Parameter> Parameters;
    public readonly IType ReturnType;
    public readonly Block Statements;

    public FunctionStatement(IList<Parameter> parameters, IType returnType, Block statements)
    {
        Parameters = parameters;
        ReturnType = returnType;
        Statements = statements;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public dynamic? Accept(IInterpreterVisitor visitor)
    {
        return visitor.Visit(this);
    }
}
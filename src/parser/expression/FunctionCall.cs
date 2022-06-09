using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public class FunctionCall : IExpression, IStatement
{
    public string Name;
    public readonly List<IExpression> Arguments;

    public FunctionCall(string name, List<IExpression> arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public IType Accept(ITypeVisitor visitor)
    {
        return visitor.Visit(this);
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
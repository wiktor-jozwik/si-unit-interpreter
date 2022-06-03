using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;

namespace si_unit_interpreter.parser.statement;

public class BuiltInFunction : IStatement
{
    public readonly Func<dynamic, dynamic> Function;
    public readonly string Name;

    public BuiltInFunction(Func<dynamic, dynamic> function, string name)
    {
        Function = function;
        Name = name;
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
using si_unit_interpreter.interpreter.interpreter;

namespace si_unit_interpreter.interpreter;

public interface IInterpreterVisitable
{
    dynamic? Accept(IInterpreterVisitor visitor);
}
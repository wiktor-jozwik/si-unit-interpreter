namespace si_unit_interpreter.interpreter.interpreter;

public interface IInterpreterVisitable
{
    dynamic? Accept(IInterpreterVisitor visitor);
}
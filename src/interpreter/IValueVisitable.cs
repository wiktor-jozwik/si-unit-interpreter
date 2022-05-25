namespace si_unit_interpreter.interpreter;

public interface IValueVisitable
{
    dynamic Accept(IValueVisitor visitor);
}
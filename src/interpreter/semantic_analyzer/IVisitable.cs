namespace si_unit_interpreter.interpreter.semantic_analyzer;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}

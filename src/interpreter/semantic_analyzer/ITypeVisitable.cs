using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public interface ITypeVisitable
{
    IType Accept(ITypeVisitor visitor);
}
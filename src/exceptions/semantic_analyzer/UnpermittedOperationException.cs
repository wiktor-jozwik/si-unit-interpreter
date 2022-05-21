using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class UnpermittedOperationException: Exception
{
    public UnpermittedOperationException(IType left, string sign, IType right)
        : base($"Unsupported operator '{sign}' for {left.Format()} and {right.Format()}") {}
}
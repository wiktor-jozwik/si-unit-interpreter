using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class TypeMismatchException : Exception
{
    public TypeMismatchException(string name, IType left, IType right)
        : base($"'{name}' requires {left.Format()} type but received {right.Format()}")
    {
    }
}
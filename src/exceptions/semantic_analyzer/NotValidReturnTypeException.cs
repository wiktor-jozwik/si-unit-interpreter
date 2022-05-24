using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class NotValidReturnTypeException : Exception
{
    public NotValidReturnTypeException(string functionName, IType expectedReturn, IType actualReturn)
        : base($"'{functionName}' return requires {expectedReturn.Format()} type but returned {actualReturn.Format()}")
    {
    }
}
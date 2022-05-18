namespace si_unit_interpreter.exceptions.lexer;

public class ExponentPartExceededSizeException : Exception
{
    public ExponentPartExceededSizeException(int maxExponentSize)
        : base($"Exponent part can be up to: {maxExponentSize}")
    {
    }
}
namespace si_unit_interpreter.exceptions.lexer;

public class NumberExceededSizeException : Exception
{
    public NumberExceededSizeException(long maxIntSize)
        : base($"Number can be up to: {maxIntSize}")
    {
    }
}
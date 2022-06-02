namespace si_unit_interpreter.exceptions.lexer;

public class ExponentPartExceededSizeException : Exception
{
    public ExponentPartExceededSizeException(string message)
        : base(message)
    {
    }
}
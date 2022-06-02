namespace si_unit_interpreter.exceptions.lexer;

public class NumberExceededSizeException : Exception
{
    public NumberExceededSizeException(string message)
        : base(message)
    {
    }
}
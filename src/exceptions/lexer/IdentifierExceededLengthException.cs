namespace si_unit_interpreter.exceptions.lexer;

public class IdentifierExceededLengthException : Exception
{
    public IdentifierExceededLengthException(string message)
        : base(message)
    {
    }
}
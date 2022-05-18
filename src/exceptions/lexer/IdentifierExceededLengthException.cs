namespace si_unit_interpreter.exceptions.lexer;

public class IdentifierExceededLengthException : Exception
{
    public IdentifierExceededLengthException(int maxLength)
        : base($"Identifier can have maximum: {maxLength} chars")
    {
    }
}
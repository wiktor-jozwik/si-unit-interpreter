namespace si_unit_interpreter.exceptions.lexer;

public class TextExceededLengthException : Exception
{
    public TextExceededLengthException(int maxLength)
        : base($"Text can have maximum: {maxLength} chars")
    {
    }
}
namespace si_unit_interpreter.exceptions.lexer;

public class TextExceededLengthException : Exception
{
    public TextExceededLengthException(string message)
        : base(message)
    {
    }
}
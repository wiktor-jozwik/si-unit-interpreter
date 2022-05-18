namespace si_unit_interpreter.exceptions.lexer;

public class CommentExceededLengthException : Exception
{
    public CommentExceededLengthException(int maxLength)
        : base($"Comment can have maximum: {maxLength} chars")
    {
    }
}
namespace si_unit_interpreter.exceptions.lexer;

public class CommentExceededLengthException : Exception
{
    public CommentExceededLengthException(string message)
        : base(message)
    {
    }
}
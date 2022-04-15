namespace si_unit_interpreter;

public class ErrorHandler
{
    public List<Exception> Errors = new();

    public void RegisterError(Exception e)
    {
        Errors.Add(e);
    }
}

public class CommentExceededLengthException : Exception
{
    public CommentExceededLengthException(string message)
        : base(message)
    {
    }
}

public class TextExceededLengthException : Exception
{
    public TextExceededLengthException(string message)
        : base(message)
    {
    }
}

public class IdentifierExceededLengthException : Exception
{
    public IdentifierExceededLengthException(string message)
        : base(message)
    {
    }
}

public class NumberExceededLengthException : Exception
{
    public NumberExceededLengthException(string message)
        : base(message)
    {
    }
}
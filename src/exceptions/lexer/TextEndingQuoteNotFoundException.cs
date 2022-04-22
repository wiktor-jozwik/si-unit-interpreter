namespace si_unit_interpreter.exceptions.lexer;

public class TextEndingQuoteNotFoundException : Exception
{
    public TextEndingQuoteNotFoundException(string message)
        : base(message)
    {
    }
}
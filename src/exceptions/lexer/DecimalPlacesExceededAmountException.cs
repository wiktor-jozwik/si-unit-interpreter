namespace si_unit_interpreter.exceptions.lexer;

public class DecimalPlacesExceededAmountException : Exception
{
    public DecimalPlacesExceededAmountException(string message)
        : base(message)
    {
    }
}
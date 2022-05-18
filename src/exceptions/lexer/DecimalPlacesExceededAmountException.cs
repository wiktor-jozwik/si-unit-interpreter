namespace si_unit_interpreter.exceptions.lexer;

public class DecimalPlacesExceededAmountException : Exception
{
    public DecimalPlacesExceededAmountException(int maxDecimalPlaces)
        : base($"Number can have up to: {maxDecimalPlaces} decimal places")
    {
    }
}
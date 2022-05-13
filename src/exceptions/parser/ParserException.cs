namespace si_unit_interpreter.exceptions.parser;

public class ParserException: Exception
{
    public ParserException(IEnumerable<TokenType> expectedTokens, TokenType receivedToken, TokenPosition tokenPosition): base(_Message(expectedTokens, receivedToken, tokenPosition))
    {
    } 

    private static string _Message(IEnumerable<TokenType> expectedTokens, TokenType receivedToken, TokenPosition tokenPosition)
    {
        return $"Expected {string.Join(" or ", expectedTokens)} token but received {receivedToken} " +
               $"on row {tokenPosition.RowNumber} and column {tokenPosition.ColumnNumber}";
    }
}


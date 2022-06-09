namespace si_unit_interpreter.lexer;

public class CommentFilteredLexer : Lexer
{
    public CommentFilteredLexer(StreamReader streamReader, int maxCommentLength = 1000, int maxIdentifierLength = 1000,
        int maxTextLength = 100000, int maxDecimalPlaces = 100, int maxExponentSize = 300,
        long maxIntSize = 9223372036854775807) : base(streamReader, maxCommentLength, maxIdentifierLength,
        maxTextLength, maxDecimalPlaces, maxExponentSize, maxIntSize)
    {
    }

    public override void GetNextToken()
    {
        base.GetNextToken();
        while (Token.Type == TokenType.COMMENT)
        {
            base.GetNextToken();
        }
    }
}
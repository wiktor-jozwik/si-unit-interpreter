namespace si_unit_interpreter;

public struct Token
{
    public TokenType Type { get; }
    public TokenPosition Position { get; }
    public dynamic? Value { get; }

    public Token(TokenType type, TokenPosition position, dynamic value)
    {
        Type = type;
        Position = position;
        Value = value;
    }
    public Token(TokenType type, TokenPosition position)
    {
        Type = type;
        Position = position;
        Value = null;
    }
}

public struct TokenPosition
{
    public int RowNumber, ColumnNumber;
}

public enum TokenType
{
    IDENTIFIER,
    
    // Keywords
    WHILE,
    IF,
    ELSE,
    FUNCTION,
    RETURN,
    LET,
    UNIT,
    
    // Single char
    
    // = + - / * > < ^ !
    ASSIGNMENT_OPERATOR,
    PLUS_OPERATOR,
    MINUS_OPERATOR,
    DIVISION_OPERATOR,
    MULTIPLICATION_OPERATOR,
    GREATER_THAN_OPERATOR,
    SMALLER_THAN_OPERATOR,
    POWER_OPERATOR,
    NEGATE_OPERATOR,
    
    // Multi char

    // () [] {}
    LEFT_PARENTHESES,
    RIGHT_PARENTHESES,

    LEFT_SQUARE_BRACKET,
    RIGHT_SQUARE_BRACKET,
    
    LEFT_CURLY_BRACE,
    RIGHT_CURLY_BRACE,
    
    // ',' ':'
    COLON,
    COMMA,
    
    // || && >= <= == != ->
    OR_OPERATOR,
    AND_OPERATOR,
    GREATER_EQUAL_THAN_OPERATOR,
    SMALLER_EQUAL_THAN_OPERATOR,
    EQUAL_OPERATOR,
    NOT_EQUAL_OPERATOR,
    
    RETURN_ARROW,

    // '//'
    COMMENT,

    // Literals
    FLOAT,
    INT,
    STRING,
    TRUE,
    FALSE,
    
    // Types
    STRING_TYPE,
    BOOL_TYPE,
    VOID_TYPE,

    // End of file/text
    ETX,
    
    INVALID,
    UNKNOWN
}

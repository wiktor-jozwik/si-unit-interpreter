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
    
    // = + - / * ^ !
    ASSIGNMENT_OPERATOR,
    PLUS_OPERATOR,
    MINUS_OPERATOR,
    DIVISION_OPERATOR,
    MULTIPLICATION_OPERATOR,
    UNIT_POWER_OPERATOR,
    NEGATE_OPERATOR,
    
    // || && > < >= <= == != ->
    OR,
    AND,
    GREATER_THAN,
    SMALLER_THAN,
    GREATER_EQUAL_THAN,
    SMALLER_EQUAL_THAN,
    EQUAL,
    NOT_EQUAL,
    
    RETURN_ARROW,
    
    // () [] {}
    LEFT_PARANTHESIS,
    RIGHT_PARANTHESIS,

    LEFT_SQUARE_BRACKET,
    RIGHT_SQUARE_BRACKET,
    
    LEFT_CURLY_BRACE,
    RIGHT_CURLY_BRACE,
    
    // ',' ':'
    COLON,
    COMMA,
    
    // '//'
    COMMENT,

    // Literals
    FLOAT,
    INT,
    STRING,
    BOOL,
    
    // Types
    STRING_TYPE,
    BOOL_TYPE,
    VOID_TYPE,

    // End of file/text
    ETX,
    
    INVALID,
    UNKNOWN
}

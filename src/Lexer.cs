using System.Text;

namespace si_unit_interpreter;

public class Lexer
{
    public Token Token;
    
    private readonly StreamReader _streamReader;
    private char _character;
    private TokenPosition _currentPosition;

    private readonly Dictionary<string, TokenType> _keywordDictionary;
    private readonly Dictionary<char, TokenType> _singleCharOperatorDictionary;
    private readonly Dictionary<string, TokenType> _multiCharOperatorDictionary;
    private readonly Dictionary<char, Func<(TokenType, string)>> _collidingOperatorsDictionary;

    private readonly int MAX_COMMENT_LENGTH;
    private readonly int MAX_IDENTIFIER_LENGTH;
    private readonly int MAX_TEXT_LENGTH;
    private readonly long MAX_INT_SIZE;

    public Lexer(
        StreamReader streamReader, 
        int maxCommentLength = 1000, 
        int maxIdentifierLength = 1000, 
        int maxTextLength = 100000, 
        long maxIntSize = 9_223_372_036_854_775_807
        )
    {
        _streamReader = streamReader;

        MAX_COMMENT_LENGTH = maxCommentLength;
        MAX_IDENTIFIER_LENGTH = maxIdentifierLength;
        MAX_TEXT_LENGTH = maxTextLength;
        MAX_INT_SIZE = maxIntSize;

        _keywordDictionary = new Dictionary<string, TokenType>
        {
            ["let"] = TokenType.LET,
            ["unit"] = TokenType.UNIT,
            ["fn"] = TokenType.FUNCTION,
            ["return"] = TokenType.RETURN,
            ["if"] = TokenType.IF,
            ["else"] = TokenType.ELSE,
            ["while"] = TokenType.WHILE,
            ["string"] = TokenType.STRING_TYPE,
            ["bool"] = TokenType.BOOL_TYPE,
            ["void"] = TokenType.VOID_TYPE,
            ["true"] = TokenType.TRUE,
            ["false"] = TokenType.FALSE,
        };
        
        _singleCharOperatorDictionary = new Dictionary<char, TokenType>
        {
            ['+'] = TokenType.PLUS_OPERATOR,
            ['/'] = TokenType.DIVISION_OPERATOR,
            ['*'] = TokenType.MULTIPLICATION_OPERATOR,
            ['^'] = TokenType.POWER_OPERATOR,
            
            ['('] = TokenType.LEFT_PARENTHESES,
            [')'] = TokenType.RIGHT_PARENTHESES,

            ['['] = TokenType.LEFT_SQUARE_BRACKET,
            [']'] = TokenType.RIGHT_SQUARE_BRACKET,

            ['{'] = TokenType.LEFT_CURLY_BRACE,
            ['}'] = TokenType.RIGHT_CURLY_BRACE,

            [','] = TokenType.COMMA,
            [':'] = TokenType.COLON,
        };
        
        _multiCharOperatorDictionary = new Dictionary<string, TokenType>
        {
            ["||"] = TokenType.OR_OPERATOR,
            ["&&"] = TokenType.AND_OPERATOR,
        };

        _collidingOperatorsDictionary = new Dictionary<char, Func<(TokenType, string)>>
        {
            ['>'] = ()=> DetermineOperator('=',TokenType.GREATER_THAN_OPERATOR, TokenType.GREATER_EQUAL_THAN_OPERATOR),
            ['<'] = ()=> DetermineOperator('=',TokenType.SMALLER_THAN_OPERATOR, TokenType.SMALLER_EQUAL_THAN_OPERATOR),
            ['='] = ()=> DetermineOperator('=',TokenType.ASSIGNMENT_OPERATOR, TokenType.EQUAL_OPERATOR),
            ['!'] = ()=> DetermineOperator('=',TokenType.NEGATE_OPERATOR, TokenType.NOT_EQUAL_OPERATOR),
            ['-'] = ()=> DetermineOperator('>',TokenType.MINUS_OPERATOR, TokenType.RETURN_ARROW)
        };
        
        _currentPosition.ColumnNumber = 0;
        _currentPosition.RowNumber = 1;
        GetNextCharacter();
    }

    private (TokenType, string) DetermineOperator(char expectedNextOperator, TokenType singleOperator, TokenType multiOperator)
    {
        var operatorString = $"{_character}";
        
        GetNextCharacter();
        if (_character == expectedNextOperator)
        {
            operatorString += _character;
            GetNextCharacter();
            return (multiOperator, operatorString);
        }

        return (singleOperator, operatorString);
    }

    public void GetNextToken()
    {
        SkipWhites();
        if (
            TryBuildEtx() ||
            TryBuildCommentOrDivideOperator() || 
            TryBuildIdentifierKeywordOrBool() || 
            TryBuildText() || 
            TryBuildNumber() || 
            TryBuildOperator()) return;

        Token = new Token(TokenType.UNKNOWN, _currentPosition, "");
    }

    private void GetNextCharacter()
    {
        _character = (char)_streamReader.Read();
        _currentPosition.ColumnNumber++;

        if (_character == '\n')
        {
            if (_streamReader.Peek() == '\r')
            {
                _streamReader.Read();
            }
            _currentPosition.ColumnNumber = 0;
            _currentPosition.RowNumber++;
        }
        else if (_character == '\r')
        {
            if (_streamReader.Peek() == '\n')
            {
                _streamReader.Read();
            }
            _currentPosition.ColumnNumber = 0;
            _currentPosition.RowNumber++;
        }
    }

    private void SkipWhites()
    {
        while (char.IsWhiteSpace(_character))
        {
            GetNextCharacter();
        }
    }

    private bool TryBuildEtx()
    {
        if (!_streamReader.EndOfStream || _character != '\uffff') return false;
        
        Token = new Token(TokenType.ETX, _currentPosition);
        return true;
    }

    private bool TryBuildCommentOrDivideOperator()
    {
        if (_character != '/') return false;

        var commentDivisionPosition = _currentPosition;

        GetNextCharacter();

        if (_character == '/')
        {
            var value = new StringBuilder();
            
            GetNextCharacter();
            // sprawdzać max długość komentarza
            // atrybut lexera, pewna domyslna wartosc
            while (_character != '\n' && (_character != '\uffff' || !_streamReader.EndOfStream))
            {
                value.Append(_character);
                GetNextCharacter();
            }
                
            Token = new Token(TokenType.COMMENT, commentDivisionPosition, value.ToString());
            return true;
        }

        Token = new Token(_singleCharOperatorDictionary['/'], commentDivisionPosition, "/");
        return true;
    }

    private bool TryBuildIdentifierKeywordOrBool()
    {
        if (!char.IsLetter(_character)) return false;

        var identifierPosition = _currentPosition;
        
        var value = new StringBuilder();
        while (char.IsLetter(_character) || _character == '_' || char.IsDigit(_character))
        {
            value.Append(_character);
            GetNextCharacter();
        }

        var stringValue = value.ToString();

        if (_keywordDictionary.TryGetValue(stringValue, out var tokenType))
        {
            Token = new Token(tokenType, identifierPosition);
            return true;
        }

        Token = new Token(TokenType.IDENTIFIER, identifierPosition, stringValue);
        return true;
    }

    private bool TryBuildText()
    {
        if (_character != '\"') return false;

        var text = new StringBuilder();

        var textPosition = _currentPosition;

        GetNextCharacter();
        // sprawdzac dlugosc stringa, parametr elxera
        while (_character != '\"' && _character != '\uffff')
        {
            if (_character == '\\')
            {
                GetNextCharacter();
                text.Append($"\\{_character}");
            }
            else
            {
                text.Append(_character);
            }
            
            GetNextCharacter();
        }
        
        if (_character != '\"')
        {
            // zarejestrowac blad ale isc dalej
            // handler bledow w lexerze
        }
        else
        {
            GetNextCharacter();
        }
        
        Token = new Token(TokenType.STRING, textPosition, text.ToString());
        return true;
    }

    private bool TryBuildNumber()
    {
        if (!char.IsDigit(_character)) return false;
        
        var numberPosition = _currentPosition;
        
        var (intPartSuccess, intPart) = _TryBuildIntPart();
        var (fractionPartSuccess, fractionPart, decimalPlaces) = _TryBuildFractionPart();
        var (exponentPartSuccess, minusFactor, exponentPart) = _TryBuildExponentPart();

        if (!intPartSuccess)
        {
            Token = new Token(TokenType.INVALID, numberPosition);
            GetNextCharacter();
            return true;
        }

        if (!fractionPartSuccess && !exponentPartSuccess)
        {
            Token = new Token(TokenType.INT, numberPosition, intPart);
            return true;
        }

        double number = intPart;

        if (fractionPartSuccess)
        {
            number += fractionPart / Math.Pow(10, decimalPlaces);
        }

        if (exponentPartSuccess)
        {
            number *= Math.Pow(10, minusFactor * exponentPart);
        }
        
        Token = new Token(TokenType.FLOAT, numberPosition, number);
        return true;
    }
    private bool TryBuildOperator()
    {
        var operatorPosition = _currentPosition;

        if (_singleCharOperatorDictionary.TryGetValue(_character, out var singleOperatorTokenType))
        {
            Token = new Token(singleOperatorTokenType, operatorPosition, char.ToString(_character));
            
            GetNextCharacter();
            return true;
        }
        
        if (_collidingOperatorsDictionary.TryGetValue(_character, out var determineOperator))
        {
            var (singleOrMultiTokenType, operatorString) = determineOperator();
            Token = new Token(singleOrMultiTokenType, operatorPosition, operatorString);
            return true;
        }

        var firstOperatorChar = _character;

        GetNextCharacter();
        var multiOperator = $"{firstOperatorChar}{_character}";

        if (_multiCharOperatorDictionary.TryGetValue(multiOperator, out var multiOperatorTokenType))
        {
            Token = new Token(multiOperatorTokenType, operatorPosition, multiOperator);
            
            GetNextCharacter();
            return true;
        }

        return false;
    }
    
    private (bool, long) _TryBuildIntPart()
    {
        long intPart = 0;
        var intPartSuccess = true;

        if (_character != '0')
        {
            intPart = _character - '0';

            GetNextCharacter();

            while (char.IsDigit(_character))
            {
                intPart = intPart * 10 + _character - '0';
                GetNextCharacter();
            }
        }
        else
        {
            GetNextCharacter();
            if (_character == '0')
            {
                intPartSuccess = false;
            }
        }

        return (intPartSuccess, intPart);
    }

    private (bool, long, int) _TryBuildFractionPart()
    {
        var fractionPartSuccess = false;
        long fractionPart = 0;
        var decimalPlaces = 0;


        if (_character != '.') return (fractionPartSuccess, fractionPart, decimalPlaces);
        
        GetNextCharacter();

        while (char.IsDigit(_character))
        {
            fractionPart = fractionPart * 10 + _character - '0';
            decimalPlaces++;
            GetNextCharacter();
        }
        fractionPartSuccess = true;
        
        return (fractionPartSuccess, fractionPart, decimalPlaces);
    }

    private (bool, int, int) _TryBuildExponentPart()
    {
        var exponentPartSuccess = false;
        var exponentPart = 0;
        var minusFactor = 1;

        if (_character != 'e') return (exponentPartSuccess, minusFactor, exponentPart);
        
        GetNextCharacter();

        if (_character == '-')
        {
            minusFactor = -1;
            GetNextCharacter();
        }

        while (char.IsDigit(_character))
        {
            exponentPart = exponentPart * 10 + _character - '0';
            GetNextCharacter();
        }
        exponentPartSuccess = true;

        return (exponentPartSuccess, minusFactor, exponentPart);
    }
}



using System.Text;
using si_unit_interpreter.exceptions.lexer;

namespace si_unit_interpreter.lexer;

public class Lexer
{
    public Token Token;

    private readonly StreamReader _streamReader;
    private char _character;
    private TokenPosition _currentPosition;

    private readonly Dictionary<string, TokenType> _keywordMap;
    private readonly Dictionary<string, char> _escapeCharsMap;
    private readonly Dictionary<char, TokenType> _singleCharOperatorMap;
    private readonly Dictionary<string, TokenType> _multiCharOperatorMap;
    private readonly Dictionary<char, Func<(TokenType, string)>> _collidingOperatorsMap;

    private readonly int _maxCommentLength;
    private readonly int _maxIdentifierLength;
    private readonly int _maxTextLength;
    private readonly int _maxDecimalPlaces;
    private readonly int _maxExponentSize;
    private readonly long _maxIntSize;

    public Lexer(
        StreamReader streamReader,
        int maxCommentLength = 1000,
        int maxIdentifierLength = 1000,
        int maxTextLength = 100000,
        int maxDecimalPlaces = 100,
        int maxExponentSize = 300,
        long maxIntSize = 9_223_372_036_854_775_807
    )
    {
        _streamReader = streamReader;

        _maxCommentLength = maxCommentLength;
        _maxIdentifierLength = maxIdentifierLength;
        _maxTextLength = maxTextLength;
        _maxDecimalPlaces = maxDecimalPlaces;
        _maxExponentSize = maxExponentSize;
        _maxIntSize = maxIntSize;

        _keywordMap = new Dictionary<string, TokenType>
        {
            ["let"] = TokenType.LET,
            ["unit"] = TokenType.UNIT,
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

        _escapeCharsMap = new Dictionary<string, char>
        {
            ["\\\""] = '\"',
            ["\\\n"] = '\n',
            ["\\\t"] = '\t',
            ["\\\\"] = '\\'
        };

        _singleCharOperatorMap = new Dictionary<char, TokenType>
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

        _multiCharOperatorMap = new Dictionary<string, TokenType>
        {
            ["||"] = TokenType.OR_OPERATOR,
            ["&&"] = TokenType.AND_OPERATOR,
        };

        _collidingOperatorsMap = new Dictionary<char, Func<(TokenType, string)>>
        {
            ['>'] = () =>
                _DetermineOperator('=', TokenType.GREATER_THAN_OPERATOR, TokenType.GREATER_EQUAL_THAN_OPERATOR),
            ['<'] = () =>
                _DetermineOperator('=', TokenType.SMALLER_THAN_OPERATOR, TokenType.SMALLER_EQUAL_THAN_OPERATOR),
            ['='] = () => _DetermineOperator('=', TokenType.ASSIGNMENT_OPERATOR, TokenType.EQUAL_OPERATOR),
            ['!'] = () => _DetermineOperator('=', TokenType.NEGATE_OPERATOR, TokenType.NOT_EQUAL_OPERATOR),
            ['-'] = () => _DetermineOperator('>', TokenType.MINUS_OPERATOR, TokenType.RETURN_ARROW)
        };

        _currentPosition.ColumnNumber = 0;
        _currentPosition.RowNumber = 1;
        GetNextCharacter();
    }

    private (TokenType, string) _DetermineOperator(char multiOperatorNextChar, TokenType singleOperator,
        TokenType multiOperator)
    {
        var operatorString = $"{_character}";

        GetNextCharacter();
        if (_character != multiOperatorNextChar) return (singleOperator, operatorString);

        operatorString += _character;
        GetNextCharacter();
        return (multiOperator, operatorString);
    }

    public virtual void GetNextToken()
    {
        SkipWhites();
        if (
            TryBuildEtx() ||
            TryBuildCommentOrDivideOperator() ||
            TryBuildIdentifierKeywordOrBool() ||
            TryBuildText() ||
            TryBuildNumber() ||
            TryBuildOperator()) return;

        Token = new Token(TokenType.UNKNOWN, _currentPosition, char.ToString(_character));
    }

    private void GetNextCharacter()
    {
        _character = (char) _streamReader.Read();
        _currentPosition.ColumnNumber++;

        if (_character is not ('\n' or '\r')) return;

        var escapingChars = new List<string> {"\n\r", "\r\n"};
        if (escapingChars.Contains($"{_character}{(char) _streamReader.Peek()}"))
        {
            _streamReader.Read();
        }

        _currentPosition.ColumnNumber = 0;
        _currentPosition.RowNumber++;
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
            var comment = new StringBuilder();
            GetNextCharacter();

            while (_character != '\n' && (_character != '\uffff' || !_streamReader.EndOfStream))
            {
                if (comment.Length == _maxCommentLength)
                {
                    throw new CommentExceededLengthException(_maxCommentLength);
                }

                comment.Append(_character);
                GetNextCharacter();
            }

            Token = new Token(TokenType.COMMENT, commentDivisionPosition, comment.ToString());
            return true;
        }

        Token = new Token(_singleCharOperatorMap['/'], commentDivisionPosition, "/");
        return true;
    }

    private bool TryBuildIdentifierKeywordOrBool()
    {
        if (!char.IsLetter(_character)) return false;

        var identifierPosition = _currentPosition;

        var value = new StringBuilder();
        while (char.IsLetter(_character) || _character == '_' || char.IsDigit(_character))
        {
            if (value.Length == _maxIdentifierLength)
            {
                throw new IdentifierExceededLengthException(_maxIdentifierLength);
            }

            value.Append(_character);
            GetNextCharacter();
        }

        var stringValue = value.ToString();

        if (_keywordMap.TryGetValue(stringValue, out var tokenType))
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

        while (_character != '\"' && _character != '\uffff')
        {
            if (text.Length == _maxTextLength) throw new TextExceededLengthException(_maxTextLength);

            if (_character == '\\')
            {
                GetNextCharacter();
                if (_escapeCharsMap.TryGetValue($"\\{_character}", out var escapeChar))
                {
                    text.Append(escapeChar);
                }
                else
                {
                    throw new UnknownEscapeCharException();
                }
            }
            else
            {
                text.Append(_character);
            }

            GetNextCharacter();
        }

        if (_character != '\"') throw new TextEndingQuoteNotFoundException();

        GetNextCharacter();
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

        if (_singleCharOperatorMap.TryGetValue(_character, out var singleOperatorTokenType))
        {
            Token = new Token(singleOperatorTokenType, operatorPosition, char.ToString(_character));

            GetNextCharacter();
            return true;
        }

        if (_collidingOperatorsMap.TryGetValue(_character, out var determineOperator))
        {
            var (singleOrMultiTokenType, operatorString) = determineOperator();

            Token = new Token(singleOrMultiTokenType, operatorPosition, operatorString);
            return true;
        }

        var multiOperator = $"{_character}{(char) _streamReader.Peek()}";

        if (_multiCharOperatorMap.TryGetValue(multiOperator, out var multiOperatorTokenType))
        {
            GetNextCharacter();
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
                if (intPart > _maxIntSize) throw new NumberExceededSizeException(_maxIntSize);

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

            if (decimalPlaces > _maxDecimalPlaces)
            {
                throw new DecimalPlacesExceededAmountException(_maxDecimalPlaces);
            }

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
            if (exponentPart > _maxExponentSize) throw new ExponentPartExceededSizeException(_maxExponentSize);

            GetNextCharacter();
        }

        exponentPartSuccess = true;

        return (exponentPartSuccess, minusFactor, exponentPart);
    }
}
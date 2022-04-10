using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace si_unit_interpreter;

public class Lexer
{
    public Token Token;

    private readonly Dictionary<string, TokenType> _keywordDictionary;
    private readonly Dictionary<char, TokenType> _singleCharOperatorDictionary;
    private readonly Dictionary<string, TokenType> _multiCharOperatorDictionary;
    private readonly StreamReader _streamReader; 

    private char _character;
    private TokenPosition _currentPosition;

    public Lexer(StreamReader streamReader)
    {
        _streamReader = streamReader;

        _currentPosition.ColumnNumber = 0;
        _currentPosition.RowNumber = 1;

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
        };
        
        _singleCharOperatorDictionary = new Dictionary<char, TokenType>
        {
            ['='] = TokenType.ASSIGNMENT_OPERATOR,
            ['+'] = TokenType.PLUS_OPERATOR,
            ['-'] = TokenType.MINUS_OPERATOR,
            ['/'] = TokenType.DIVISION_OPERATOR,
            ['*'] = TokenType.MULTIPLICATION_OPERATOR,
            ['>'] = TokenType.GREATER_THAN_OPERATOR,
            ['<'] = TokenType.SMALLER_THAN_OPERATOR,
            ['^'] = TokenType.POWER_OPERATOR,
            ['!'] = TokenType.NEGATE_OPERATOR,
            
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
            [">="] = TokenType.GREATER_EQUAL_THAN_OPERATOR,
            ["<="] = TokenType.SMALLER_EQUAL_THAN_OPERATOR,
            ["=="] = TokenType.EQUAL_OPERATOR,
            ["!="] = TokenType.NOT_EQUAL_OPERATOR,
            ["->"] = TokenType.RETURN_ARROW,
        };
    }
    
    public void GetNextToken()
    {
        GetNextCharacter();

        SkipWhites();

        if (TryBuildEtx()) return;

        if (TryBuildCommentOrDivideOperator()) return;
        
        if (TryBuildIdentifierKeywordOrBool()) return;

        if (TryBuildText()) return;

        if (TryBuildNumber()) return;

        if (TryBuildOperator()) return;

        Token = new Token(TokenType.UNKNOWN, _currentPosition, "");
    }

    private void GetNextCharacter()
    {
        _character = (char)_streamReader.Read();
        _currentPosition.ColumnNumber++;

        if (_character == '\n')
        {
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

        var divisionChar = _character;
        
        GetNextCharacter();

        var commentPosition = _currentPosition;

        if (_character == '/')
        {
            var value = new StringBuilder();


            while (_character != '\n' && !_streamReader.EndOfStream)
            {
                GetNextCharacter();

                value.Append(_character);
            }
                
            Token = new Token(TokenType.COMMENT, commentPosition, value.ToString());
            return true;
        }

        commentPosition.ColumnNumber--;
        Token = new Token(_singleCharOperatorDictionary[divisionChar], commentPosition, divisionChar);
        return true;
    }

    private bool TryBuildIdentifierKeywordOrBool()
    {
        if (!char.IsLetter(_character)) return false;

        var identifierPosition = _currentPosition;
        
        var value = new StringBuilder();
        while (char.IsLetter(_character))
        {
            value.Append(_character);
            GetNextCharacter();
        }

        var stringValue = value.ToString();

        if (_keywordDictionary.ContainsKey(stringValue))
        {
            Token = new Token(_keywordDictionary[stringValue], identifierPosition, stringValue);
            return true;
        }

        if (stringValue is "true" or "false")
        {
            Token = new Token(TokenType.BOOL, identifierPosition, bool.Parse(stringValue));
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
        while (_character != '\"')
        {
            text.Append(_character);
            if (_streamReader.EndOfStream)
            {
                Token = new Token(TokenType.INVALID, textPosition, text.ToString());
                return true;
            }
            
            GetNextCharacter();
        }

        Token = new Token(TokenType.STRING, textPosition, text.ToString());
        return true;
    }

    private bool TryBuildNumber()
    {
        if (!char.IsDigit(_character)) return false; //should be is digit but no 0

        long intPart = 0;

        var position = _currentPosition; 

        if (_character != '0')
        {
            intPart += _character - '0';
            
            GetNextCharacter();

            while (char.IsDigit(_character))
            {
                intPart = intPart * 10 + _character - '0';
                GetNextCharacter();
            }
        }
        else
        {
            // if 0 is present
            GetNextCharacter();
        }

        if (_character == '.')
        {
            long fractionPart = 0;
            int decimalPlaces = 0;
            
            int exponentPart = 0;
            
            
            GetNextCharacter();

            while (char.IsDigit(_character))
            {
                fractionPart = fractionPart * 10 + _character - '0';
                decimalPlaces++;
                GetNextCharacter();
            }

            if (_character == 'e')
            {
                GetNextCharacter();

                var minusFactor = 1;

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
                
                Token = new Token(TokenType.FLOAT, position,
                    (intPart + fractionPart / Math.Pow(10, decimalPlaces)) * Math.Pow(10, minusFactor * exponentPart));
                return true;
            }

            Token = new Token(TokenType.FLOAT, position, intPart + fractionPart / Math.Pow(10, decimalPlaces));
            return true;
        }
        
        if (_character == 'e')
        {
            int exponentPart = 0;

            GetNextCharacter();

            var minusFactor = 1;

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
                
            Token = new Token(TokenType.FLOAT, position, intPart * Math.Pow(10, minusFactor * exponentPart));
            return true;
        }

        Token = new Token(TokenType.INT, position, intPart);
        return true;
    }
    private bool TryBuildOperator()
    {
        var multiOperatorStartingChars = GetStartingCharsOfMultiCharOperator();

        if (!multiOperatorStartingChars.Contains(_character) && !_singleCharOperatorDictionary.ContainsKey(_character))
        {
            return false;
        }
        
        var operatorPosition = _currentPosition;
        var firstCharacter = _character;
        
        var secondCharacter = GetSecondCharForMultiCharacterOperator(_character);
        
        GetNextCharacter();

        if (_character == secondCharacter)
        {
            var operatorBuilt = $"{firstCharacter}{secondCharacter}";
            Token = new Token(_multiCharOperatorDictionary[operatorBuilt], operatorPosition, operatorBuilt);
            return true;
        }

        // If we have multi operator like -/ we want to return invalid token rather than minus and div operators
        if (_singleCharOperatorDictionary.ContainsKey(_character))
        {
            Token = new Token(TokenType.INVALID, operatorPosition);
            return true;
        }

        if (_singleCharOperatorDictionary.ContainsKey(firstCharacter))
        {
            Token = new Token(_singleCharOperatorDictionary[firstCharacter], operatorPosition, firstCharacter);
            return true;
        }
        
        Token = new Token(TokenType.INVALID, operatorPosition);
        return true;
    }

    private List<char> GetStartingCharsOfMultiCharOperator()
    {
        var multiOperatorStartingChars = new List<char>();

        foreach (var (key, _) in _multiCharOperatorDictionary)
        {
            multiOperatorStartingChars.Add(key[0]);
        }

        return multiOperatorStartingChars;
    }

    private char? GetSecondCharForMultiCharacterOperator(char firstCharacter)
    {
        char? secondChar = null;
        foreach (var (key, _) in _multiCharOperatorDictionary)
        {
            if (key[0] == firstCharacter)
            {
                secondChar = key[1];
            }
        }

        return secondChar;
    }
}



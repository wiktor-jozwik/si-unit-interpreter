using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace si_unit_interpreter;

public class Lexer
{
    public Token Token;

    private TokenPosition _currentPosition;
    private readonly Dictionary<string, TokenType> _keywordDictionary;
    private readonly StreamReader _streamReader; 
    private char _character;

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
    }
    
    public void GetNextToken()
    {
        GetNextCharacter();
        SkipWhites();

        if (TryBuildEtx()) return;

        if (TryBuildCommentOrDivideOperator()) return;
        
        if (TryBuildText()) return;

        if (TryBuildNumber()) return;
        
        // if (TryBuildBoolean()) return;

        if (TryBuildIdentifierOrKeyword()) return;
        
        

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
        if (!_streamReader.EndOfStream) return false;
        
        Token = new Token(TokenType.ETX, _currentPosition);
        return true;
    }

    private bool TryBuildCommentOrDivideOperator()
    {
        if (_character != '/') return false;
        
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
        Token = new Token(TokenType.DIVISION_OPERATOR, commentPosition, '/');
        return true;

    }

    private bool TryBuildIdentifierOrKeyword()
    {
        if (!char.IsLetter(_character)) return false;

        var identifierOrKeywordPosition = _currentPosition;
        
        var value = new StringBuilder();
        while (char.IsLetter(_character))
        {
            value.Append(_character);
            GetNextCharacter();
        }

        var stringValue = value.ToString();

        if (_keywordDictionary.ContainsKey(stringValue))
        {
            Token = new Token(_keywordDictionary[stringValue], identifierOrKeywordPosition, stringValue);

            return true;
        }

        Token = new Token(TokenType.IDENTIFIER, identifierOrKeywordPosition, stringValue);
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

            // TODO
            if (_character == 'e')
            {
                GetNextCharacter();

                while (char.IsDigit(_character))
                {
                    exponentPart = exponentPart * 10 + _character - '0';
                }
                
                // TODO
                // Token = 
                return true;
            }

            Token = new Token(TokenType.FLOAT, position, intPart + fractionPart / Math.Pow(10, decimalPlaces));
            return true;
        }

        Token = new Token(TokenType.INT, position, intPart);
        return true;
    }

    private bool TryBuildBoolean()
    {
        // Token = new Token(TokenType.BOOL, position, boolValue);
        throw new NotImplementedException();
    }

    private bool TryBuildMultiCharacterOperator()
    {
        throw new NotImplementedException();
    }

    private bool TryBuildSingleCharacterOperator()
    {
        throw new NotImplementedException();
    }
}



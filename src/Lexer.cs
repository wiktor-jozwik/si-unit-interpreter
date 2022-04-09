using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace si_unit_interpreter;

public class Lexer
{
    public Token Token;

    private TokenPosition _position;
    private readonly Dictionary<string, TokenType> _keywordDictionary;
    private readonly StreamReader _streamReader; 
    private char _character;

    public Lexer(StreamReader streamReader)
    {
        _streamReader = streamReader;

        _keywordDictionary = new Dictionary<string, TokenType>
        {
            ["let"] = TokenType.LET,
            ["unit"] = TokenType.UNIT,
            ["fn"] = TokenType.FUNCTION,
            ["return"] = TokenType.RETURN,
            ["if"] = TokenType.IF,
            ["else"] = TokenType.ELSE,
            ["while"] = TokenType.WHILE,
            
            // TODO
            // ["string"] = TokenType.STRING_TYPE,
            // ["bool"] = TokenType.STRING_BOOL,
            // ["void"] = TokenType.STRING_VOID,
            //
            // ["true"] = TokenType.BOOL,
            // ["false"] = TokenType.BOOL,
        };
    }
    
    public void GetNextToken()
    {
        GetNextCharacter();
        SkipWhites();

        if (TryBuildEtx()) return;

        if (TryBuildCommentOrDivideOperator()) return;

        if (TryBuildIdentifierOrKeyword()) return;
        
        
        

        Token = new Token(TokenType.UNKNOWN, _position, "");
    }

    private void GetNextCharacter()
    {
        _character = (char)_streamReader.Read();
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
        
        Token = new Token(TokenType.ETX, _position);
        return true;
    }

    private bool TryBuildCommentOrDivideOperator()
    {
        if (_character != '/') return false;
        
        GetNextCharacter();

        if (_character == '/')
        {
            var value = new StringBuilder();


            while (_character != '\n' && !_streamReader.EndOfStream)
            {
                GetNextCharacter();

                value.Append(_character);
            }
                
            Token = new Token(TokenType.COMMENT, _position, value.ToString());
            return true;
        }
        
        // TODO this._position - 1 because we took next char already 
        Token = new Token(TokenType.DIVISION_OPERATOR, _position, '/');
        return true;

    }

    private bool TryBuildIdentifierOrKeyword()
    {
        if (!char.IsLetter(_character)) return false;
        
        var value = new StringBuilder();
        while (char.IsLetter(_character))
        {
            value.Append(_character);
            GetNextCharacter();
        }

        var stringValue = value.ToString();

        if (_keywordDictionary.ContainsKey(stringValue))
        {
            Token = new Token(_keywordDictionary[stringValue], _position, stringValue);

            return true;
        }

        Token = new Token(TokenType.IDENTIFIER, _position, stringValue);
        return true;
    }

    private bool TryBuildText()
    {
        throw new NotImplementedException();
    }

    private bool TryBuildNumber()
    {
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



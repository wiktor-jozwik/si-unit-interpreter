using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace si_unit_interpreter.spec;

public class LexerUnitTests
{
    // SINGLE TOKENS

    // TryBuildEtx
    [Fact]

    public void TestGetEtxToken()
    {
        const string commentText = "";
        var token = GetSingleTokenFromLexerByText(commentText);
        
        Assert.Equal(TokenType.ETX, token.Type);
    }
    
    // TryBuildCommentOrDivideOperator
    [Fact]
    public void TestGetCommentToken()
    {
        const string comment = "let x";
        const string commentText = "//" + comment;
        
        var token = GetSingleTokenFromLexerByText(commentText);
        
        Assert.Equal(TokenType.COMMENT, token.Type);        
        Assert.Equal(comment, token.Value);
    }
    
    [Fact]
    public void TestGetDivisionToken()
    {
        const string divisionText = "/ 5";
        
        var token = GetSingleTokenFromLexerByText(divisionText);
        
        Assert.Equal(TokenType.DIVISION_OPERATOR, token.Type);        
        Assert.Equal('/', token.Value);
    }
    
    // TryBuildIdentifierOrKeyword
    
    [Fact]
    public void TestGetIdentifierToken()
    {
        const string identifierName = "myVariable";
        const string identifierText = identifierName + ": bool";
        
        var token = GetSingleTokenFromLexerByText(identifierText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(identifierName, token.Value);
    }
    
    [Fact]
    public void TestGetLetKeywordToken()
    {
        const string letText = "let x";
        
        var token = GetSingleTokenFromLexerByText(letText);
        
        Assert.Equal(TokenType.LET, token.Type);        
        Assert.Equal("let", token.Value);
    }

    [Fact]
    public void TestGetUnitKeywordToken()
    {
        const string unitText = "unit N";
        
        var token = GetSingleTokenFromLexerByText(unitText);
        
        Assert.Equal(TokenType.UNIT, token.Type);        
        Assert.Equal("unit", token.Value);
    }
   
    [Fact]
    public void TestGetFunctionKeywordToken()
    {
        const string functionText = "fn x(a: []fn)";
        
        var token = GetSingleTokenFromLexerByText(functionText);
        
        Assert.Equal(TokenType.FUNCTION, token.Type);        
        Assert.Equal("fn", token.Value);
    }
    
    [Fact]
    public void TestGetReturnKeywordToken()
    {
        const string returnText = "return 5";
        
        var token = GetSingleTokenFromLexerByText(returnText);
        
        Assert.Equal(TokenType.RETURN, token.Type);        
        Assert.Equal("return", token.Value);
    }
    
    [Fact]
    public void TestGetIfKeywordToken()
    {
        const string ifText = "if (x > 5)";
        
        var token = GetSingleTokenFromLexerByText(ifText);
        
        Assert.Equal(TokenType.IF, token.Type);        
        Assert.Equal("if", token.Value);
    }
    
    [Fact]
    public void TestGetElseKeywordToken()
    {
        const string elseText = "else {}";
        
        var token = GetSingleTokenFromLexerByText(elseText);
        
        Assert.Equal(TokenType.ELSE, token.Type);        
        Assert.Equal("else", token.Value);
    }

    [Fact]
    public void TestGetWhileKeywordToken()
    {
        const string whileText = "while (true)";
        
        var token = GetSingleTokenFromLexerByText(whileText);
        
        Assert.Equal(TokenType.WHILE, token.Type);        
        Assert.Equal("while", token.Value);
    }
    
    // Some edge case
    [Fact]
    public void TestIffIdentifierToken()
    {
        const string iffText = "iff";
        
        var token = GetSingleTokenFromLexerByText(iffText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(iffText, token.Value);
    }
    

    [Fact]
    public void TestLettIdentifierToken()
    {
        const string lettText = "lett";
        
        var token = GetSingleTokenFromLexerByText(lettText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(lettText, token.Value);
    }

    [Fact]
    public void TestCaseSensitivityOfKeyword()
    {
        const string notKeywordText = "While";
        
        var token = GetSingleTokenFromLexerByText(notKeywordText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(notKeywordText, token.Value);
    }
    
    // TryBuildText
    
    // TODO
    
    // TryBuildNumber
    
    // TODO
    
    // TryBuildMultiCharacterOperator
    
    // TODO
    
    // TryBuildSingleCharacterOperator
    
    // TODO
    
    
    // Some more complex tests - with lots of tokens
    
    // [Fact]
    // public void TestGetTokensFromVariableAssignment()
    // {
    //     const string assignmentText = "let x: [] = 5";
    //     var streamReader = GetStreamReaderFromString(assignmentText);
    //     var lexer = new Lexer(streamReader);       
    //     
    //     var tokens = new List<Token>();
    //
    //
    //     lexer.GetNextToken();
    //     while (lexer.token.Type != TokenType.ETX)
    //     {
    //         lexer.GetNextToken();
    //         tokens.Add(lexer.token);
    //     }
    //     Assert.Equal(TokenType.LET, tokens[0].Type);
    // }
    
    // Some edge cases

    [Fact]
    public void TestDeclareLettIdentifier()
    {
        const string declareLett = "let lett";

        var tokens = GetAllTokensFromLexerByText(declareLett);

        Assert.Equal(TokenType.LET, tokens[0].Type);
        Assert.Equal("let", tokens[0].Value);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
        Assert.Equal("lett", tokens[1].Value);
    }

    private static Token GetSingleTokenFromLexerByText(string textToLexer)
    {
        var streamReader = GetStreamReaderFromString(textToLexer);
        var lexer = new Lexer(streamReader);       

        lexer.GetNextToken();
        streamReader.Close();

        return lexer.Token;
    }

    private static List<Token> GetAllTokensFromLexerByText(string textToLexer)
    {
        var streamReader = GetStreamReaderFromString(textToLexer);
        var lexer = new Lexer(streamReader);     
        
        var tokens = new List<Token>();

        lexer.GetNextToken();
        while (lexer.Token.Type != TokenType.ETX)
        {
            tokens.Add(lexer.Token);
            lexer.GetNextToken();
        }
        
        streamReader.Close();

        return tokens;
    }


    private static StreamReader GetStreamReaderFromString(string text)
    {
        var byteArray = Encoding.UTF8.GetBytes(text);
        var stream = new MemoryStream(byteArray);
        
        return new StreamReader(stream);
    }
}
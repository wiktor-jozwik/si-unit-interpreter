using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace si_unit_interpreter.spec;

public class LexerUnitTests
{
    // SINGLE TOKEN TESTS

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
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(2, token.Position.ColumnNumber);
    }
    
    [Fact]
    public void TestGetDivisionToken()
    {
        const string divisionText = "/ 5";
        
        var token = GetSingleTokenFromLexerByText(divisionText);
        
        Assert.Equal(TokenType.DIVISION_OPERATOR, token.Type);        
        Assert.Equal('/', token.Value);
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
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
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
    }
    
    [Fact]
    public void TestGetLetKeywordToken()
    {
        const string letText = "let x";
        
        var token = GetSingleTokenFromLexerByText(letText);
        
        Assert.Equal(TokenType.LET, token.Type);        
        Assert.Equal("let", token.Value);
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
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
        const string functionText = "fn x(a: [])";
        
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
    
    [Fact]
    public void TestGetStringTypeToken()
    {
        const string whileText = "string";

        var token = GetSingleTokenFromLexerByText(whileText);
        
        Assert.Equal(TokenType.STRING_TYPE, token.Type);        
        Assert.Equal("string", token.Value);
    }

    [Fact]
    public void TestGetBoolTypeToken()
    {
        const string whileText = "bool";
        
        var token = GetSingleTokenFromLexerByText(whileText);
        
        Assert.Equal(TokenType.BOOL_TYPE, token.Type);        
        Assert.Equal("bool", token.Value);
    }
    
    [Fact]
    public void TestGetVoidTypeToken()
    {
        const string whileText = "void";
        
        var token = GetSingleTokenFromLexerByText(whileText);
        
        Assert.Equal(TokenType.VOID_TYPE, token.Type);        
        Assert.Equal("void", token.Value);
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
    
    [Fact]
    public void TestTextToken()
    {
        const string s = "string in my language";
        const string stringText = $"\"{s}\"";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.STRING, token.Type);        
        Assert.Equal(s, token.Value);
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
    }
    
    [Fact]
    public void TestTextWithoutEndingQuoteToken()
    {
        const string s = "my string ";
        const string stringText = $"\"{s}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INVALID, token.Type);        
        Assert.Equal(s, token.Value);
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
    }
    
    // TryBuildNumber
    
    [Fact]
    public void TestIntToken()
    {
        const int intValue = 25;
        var stringText = $"{intValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INT, token.Type);        
        Assert.Equal(intValue, token.Value);
    }
    
    [Fact]
    public void TestIntTokenWithCharacterAfterwards()
    {
        const int intValue = 5556;
        var stringText = $"{intValue}aaa";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INT, token.Type);        
        Assert.Equal(intValue, token.Value);
    }
    
    // public void TestIntWithLeadingZerosToken()
    // {
    //     const int intValue = 25;
    //     var stringText = $"000{intValue}";
    //     
    //     var token = GetSingleTokenFromLexerByText(stringText);
    //     
    //     Assert.Equal(TokenType.INT, token.Type);        
    //     Assert.Equal(intValue, token.Value);
    // }
    
    [Fact]
    public void TestFloatToken()
    {
        const double floatValue = 25.55;
        var stringText = $"{floatValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);
        Assert.Equal(floatValue, token.Value, 5);
    }
    
    [Fact]
    public void TestFloatWithExponentToken()
    {
        const double floatValue = 0.25e5;
        var stringText = $"{floatValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(floatValue, token.Value, 5);
    }
    
    [Fact]
    public void TestFloatWithMinusExponentToken()
    {
        const double floatValue = 4e-5;
        var stringText = $"{floatValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(floatValue, token.Value, 5);
    }
    
    // TryBuildBoolean
    
    [Fact]
    public void TestTrueToken()
    {
        const bool boolValue = true;
        var stringText = $"{boolValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.BOOL, token.Type);        
        Assert.Equal(boolValue, token.Value);
    }
    
    [Fact]
    public void TestFalseToken()
    {
        const bool boolValue = false;
        var stringText = $"{boolValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.BOOL, token.Type);        
        Assert.Equal(boolValue, token.Value);
    }

    
    // TryBuildMultiCharacterOperator
    
    // TODO
    
    // TryBuildSingleCharacterOperator
    
    // TODO
    
    
    // MORE THAN ONE TOKEN STATEMENTS
    
    
    // [Fact]
    // public void TestGetTokensFromVariableAssignment()
    // {
    //     const string assignmentText = "let x: [] = 5"
    // let s: string = "my string here"
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
    
    
    // TOKEN POSITION TESTS

    [Fact]
    public void TestTokenPositionOfKeywordAndIdentifier()
    {
        const string keywordAndIdentifierText = "return myVariable";
        
        var tokens = GetAllTokensFromLexerByText(keywordAndIdentifierText);
        
        var returnToken = tokens[0];
        var identifierToken = tokens[1];
        
        Assert.Equal(TokenType.RETURN, returnToken.Type);
        Assert.Equal("return", returnToken.Value);
        Assert.Equal(1, returnToken.Position.RowNumber);
        Assert.Equal(1, returnToken.Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, identifierToken.Type);
        Assert.Equal("myVariable", identifierToken.Value);
        Assert.Equal(1, identifierToken.Position.RowNumber);
        Assert.Equal(8, identifierToken.Position.ColumnNumber);
    }


    [Fact]
    public void TestTokenPositionWithNewLineInside()
    {
        const string keywordAndIdentifierText = "return }\nlet force";
        
        var tokens = GetAllTokensFromLexerByText(keywordAndIdentifierText);
        
        // tokens: return, curlyBrace, let, identifier
        var returnToken = tokens[0];
        var letToken = tokens[2];
        var identifierToken = tokens[3];
        
        Assert.Equal(TokenType.RETURN, returnToken.Type);
        Assert.Equal("return", returnToken.Value);
        Assert.Equal(1, returnToken.Position.RowNumber);
        Assert.Equal(1, returnToken.Position.ColumnNumber);
        
        Assert.Equal(TokenType.LET, letToken.Type);
        Assert.Equal("let", letToken.Value);
        Assert.Equal(2, letToken.Position.RowNumber);
        Assert.Equal(1, letToken.Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, identifierToken.Type);
        Assert.Equal("force", identifierToken.Value);
        Assert.Equal(2, identifierToken.Position.RowNumber);
        Assert.Equal(5, identifierToken.Position.ColumnNumber);
    }
    
    
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

    
    // HELPER FUNCTIONS
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
using si_unit_interpreter.exceptions.lexer;
using si_unit_interpreter.lexer;
using Xunit;

namespace si_unit_interpreter.spec;

public class LexerUnitTests
{

    // TryBuildEtx
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "ETX")]
    public void TestGetEtxToken()
    {
        const string commentText = "";
        var token = GetSingleTokenFromLexerByText(commentText);
        
        Assert.Equal(TokenType.ETX, token.Type);
    }
    
    // TryBuildCommentOrDivideOperator
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Comment")]
    public void TestCommentToken()
    {
        const string comment = "let x";
        const string commentText = "//" + comment;
        
        var token = GetSingleTokenFromLexerByText(commentText);
        
        Assert.Equal(TokenType.COMMENT, token.Type);        
        Assert.Equal(comment, token.Value);
        Assert.Equal(1, token.Position.RowNumber);
        Assert.Equal(1, token.Position.ColumnNumber);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Comment")]
    [Trait("Category", "Error")]
    public void TestCommentLimit()
    {
        const string comment = "comment to test limit, more than 10 chars";
        const string commentText = "// " + comment;
        const int maxCommentLength = 10;

        var e = Assert.Throws<CommentExceededLengthException>(() => GetSingleTokenFromLexerByText(commentText, maxCommentLength: maxCommentLength));
        Assert.Equal($"Comment can have maximum: {maxCommentLength} chars", e.Message);
    }

    // TryBuildIdentifierOrKeyword
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    public void TestIdentifierToken()
    {
        const string identifierText = "myVariable";
        
        var token = GetSingleTokenFromLexerByText(identifierText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(identifierText, token.Value);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "Error")]

    public void TestIdentifierLimit()
    {
        const string identifierText = "myIdentifierWithMoreThan10Chars";
        const int maxIdentifierLength = 10;
        
        var e = Assert.Throws<IdentifierExceededLengthException>(() => GetSingleTokenFromLexerByText(identifierText, maxIdentifierLength: maxIdentifierLength));
        Assert.Equal($"Identifier can have maximum: {maxIdentifierLength} chars", e.Message);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    public void TestSnakeCaseIdentifierToken()
    {
        const string identifierText = "my_first_var333";
        
        var token = GetSingleTokenFromLexerByText(identifierText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(identifierText, token.Value);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestLetKeywordToken()
    {
        const string letText = "let";
        
        var token = GetSingleTokenFromLexerByText(letText);
        
        Assert.Equal(TokenType.LET, token.Type);        
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestUnitKeywordToken()
    {
        const string unitText = "unit";
        
        var token = GetSingleTokenFromLexerByText(unitText);
        
        Assert.Equal(TokenType.UNIT, token.Type);        
    }
   
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestReturnKeywordToken()
    {
        const string returnText = "return";
        
        var token = GetSingleTokenFromLexerByText(returnText);
        
        Assert.Equal(TokenType.RETURN, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestIfKeywordToken()
    {
        const string ifText = "if";
        
        var token = GetSingleTokenFromLexerByText(ifText);
        
        Assert.Equal(TokenType.IF, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestElseKeywordToken()
    {
        const string elseText = "else";
        
        var token = GetSingleTokenFromLexerByText(elseText);
        
        Assert.Equal(TokenType.ELSE, token.Type);        
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestWhileKeywordToken()
    {
        const string whileText = "while";
        
        var token = GetSingleTokenFromLexerByText(whileText);
        
        Assert.Equal(TokenType.WHILE, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestStringTypeToken()
    {
        const string stringTypeText = "string";

        var token = GetSingleTokenFromLexerByText(stringTypeText);
        
        Assert.Equal(TokenType.STRING_TYPE, token.Type);        
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestBoolTypeToken()
    {
        const string boolTypeText = "bool";
        
        var token = GetSingleTokenFromLexerByText(boolTypeText);
        
        Assert.Equal(TokenType.BOOL_TYPE, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestVoidTypeToken()
    {
        const string voidTypeText = "void";
        
        var token = GetSingleTokenFromLexerByText(voidTypeText);
        
        Assert.Equal(TokenType.VOID_TYPE, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestFalseToken()
    {
        const string boolText = "false";

        var token = GetSingleTokenFromLexerByText(boolText);
        
        Assert.Equal(TokenType.FALSE, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Keyword")]
    public void TestTrueToken()
    {
        const string boolText = "true";
        
        var token = GetSingleTokenFromLexerByText(boolText);
        
        Assert.Equal(TokenType.TRUE, token.Type);        
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "EdgeCase")]
    public void TestIffIdentifierToken()
    {
        const string iffText = "iff";
        
        var token = GetSingleTokenFromLexerByText(iffText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(iffText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "EdgeCase")]
    public void TestLettIdentifierToken()
    {
        const string lettText = "lett";
        
        var token = GetSingleTokenFromLexerByText(lettText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(lettText, token.Value);
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "EdgeCase")]
    public void TestCaseSensitivityOfKeyword()
    {
        const string notKeywordText = "While";
        
        var token = GetSingleTokenFromLexerByText(notKeywordText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(notKeywordText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "EdgeCase")]
    public void TestBooleanCaseSensitivity()
    {
        const string notBoolText = "False";

        var token = GetSingleTokenFromLexerByText(notBoolText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(notBoolText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Identifier")]
    [Trait("Category", "EdgeCase")]
    public void TestBooleanNotCompletedToken()
    {
        const string notBoolText = "fals";

        var token = GetSingleTokenFromLexerByText(notBoolText);
        
        Assert.Equal(TokenType.IDENTIFIER, token.Type);        
        Assert.Equal(notBoolText, token.Value);
    }
    
    // TryBuildText
    
    [Trait("Category", "SingleToken")]
    [Trait("Category", "String")]
    [Fact]
    public void TestTextToken()
    {
        const string s = "string in my language";
        const string stringText = $"\"{s}\"";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.STRING, token.Type);        
        Assert.Equal(s, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "String")]
    public void TestTextWithEscapingCharsToken()
    {
        const string s = "escaped \\\"string\\\" with \\\\ \\\\";
        const string stringText = $"\"{s}\"";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.STRING, token.Type);      
        Assert.Equal("escaped \"string\" with \\ \\", token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "String")]
    public void TestTextWithNotKnownEscapingChars()
    {
        const string s = "not known \\\r";
        const string stringText = $"\"{s}\"";
        
        Assert.Throws<UnknownEscapeCharException>(() =>
            GetSingleTokenFromLexerByText(stringText));
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "String")]
    [Trait("Category", "Error")]
    public void TestTextLimit()
    {
        const string s = "my string with exceeded length - more than 10";
        const string stringText = $"\"{s}\"";
        const int maxTextLength = 10;

        var e = Assert.Throws<TextExceededLengthException>(() =>
            GetSingleTokenFromLexerByText(stringText, maxTextLength: maxTextLength));
        Assert.Equal($"Text can have maximum: {maxTextLength} chars", e.Message);
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "String")]
    [Trait("Category", "Error")]
    public void TestTextWithoutEndingQuoteToken()
    {
        const string s = "my string ";
        const string stringText = $"\"{s}";
        
        Assert.Throws<TextEndingQuoteNotFoundException>(() =>
            GetSingleTokenFromLexerByText(stringText));
    }
    
    // TryBuildNumber
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Int")]
    public void TestIntToken()
    {
        const int intValue = 25;
        var stringText = $"{intValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INT, token.Type);        
        Assert.Equal(intValue, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Int")]
    public void TestIntTokenWithCharacterAfterwards()
    {
        const int intValue = 5556;
        var stringText = $"{intValue}aaa";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INT, token.Type);        
        Assert.Equal(intValue, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Int")]
    public void TestIntWithLeadingZerosToken()
    {
        const int intValue = 25;
        var stringText = $"000{intValue}";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.INVALID, token.Type);        
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Int")]
    [Trait("Category", "Error")]
    public void TestIntLimit()
    {
        const string stringText = "1500";
        const int maxIntSize = 1000;

        var e = Assert.Throws<NumberExceededSizeException>(() =>
            GetSingleTokenFromLexerByText(stringText, maxIntSize: maxIntSize));
        Assert.Equal($"Number can be up to: {maxIntSize}", e.Message);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    public void TestPositiveFloatToken()
    {
        const string stringText = "25.55";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);
        Assert.Equal(25.55, token.Value, 5);
    }

    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    public void TestFloatWithExponentToken()
    {
        const string stringText = "0.25e5";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(0.25e5, token.Value, 5);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    public void TestFloatWithMinusExponentToken()
    {
        const string stringText = "3.8e-2";
        
        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(3.8e-2, token.Value, 5);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    public void TestFloatWithoutFractionPartWithMinusExponentToken()
    {
        const string stringText = "4e-5";

        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(4e-5, token.Value, 5);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    public void TestFloatWithoutFractionPartExponentToken()
    {
        const string stringText = "2e3";

        var token = GetSingleTokenFromLexerByText(stringText);
        
        Assert.Equal(TokenType.FLOAT, token.Type);        
        Assert.Equal(2e3, token.Value, 5);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    [Trait("Category", "Error")]
    public void TestDecimalPlacesLimit()
    {
        const string stringText = "0.000001";
        const int maxDecimalPlaces = 5;

        var e = Assert.Throws<DecimalPlacesExceededAmountException>(() =>
            GetSingleTokenFromLexerByText(stringText, maxDecimalPlaces: maxDecimalPlaces));
        Assert.Equal($"Number can have up to: {maxDecimalPlaces} decimal places", e.Message);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "Float")]
    [Trait("Category", "Error")]
    public void TestExponentSizeLimit()
    {
        const string stringText = "2e21";
        const int maxExponentSize = 20;

        var e = Assert.Throws<ExponentPartExceededSizeException>(() =>
            GetSingleTokenFromLexerByText(stringText, maxExponentSize: maxExponentSize));
        Assert.Equal($"Exponent part can be up to: {maxExponentSize}", e.Message);
    }

    
    // Multi char operators
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestOrOperatorToken()
    {
        const string operatorText = "||";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.OR_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestAndOperatorToken()
    {
        const string operatorText = "&&";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.AND_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestGreaterEqualThanOperatorToken()
    {
        const string operatorText = ">=";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.GREATER_EQUAL_THAN_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestSmallerEqualThanOperatorToken()
    {
        const string operatorText = "<=";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.SMALLER_EQUAL_THAN_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestEqualOperatorToken()
    {
        const string operatorText = "==";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.EQUAL_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestNotEqualOperatorToken()
    {
        const string operatorText = "!=";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.NOT_EQUAL_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "MultiCharOperator")]
    public void TestReturnArrowToken()
    {
        const string operatorText = "->";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.RETURN_ARROW, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    // Single operators
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestAssignmentOperatorToken()
    {
        const string operatorText = "=";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestPlusOperatorToken()
    {
        const string operatorText = "+";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.PLUS_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestMinusOperatorToken()
    {
        const string operatorText = "-";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.MINUS_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestDivisionOperatorToken()
    {
        const string operatorText = "/";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.DIVISION_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestMultiplicationOperatorToken()
    {
        const string operatorText = "*";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestGreaterThanToken()
    {
        const string operatorText = ">";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.GREATER_THAN_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestSmallerThanToken()
    {
        const string operatorText = "<";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.SMALLER_THAN_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestPowerOperatorToken()
    {
        const string operatorText = "^";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.POWER_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestNegateOperatorToken()
    {
        const string operatorText = "!";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.NEGATE_OPERATOR, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }
    
    // Braces
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestLeftParenthesesToken()
    {
        const string operatorText = "(";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.LEFT_PARENTHESES, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestRightParenthesesToken()
    {
        const string operatorText = ")";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.RIGHT_PARENTHESES, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestLeftSquareBracketToken()
    {
        const string operatorText = "[";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestRightSquareBracketToken()
    {
        const string operatorText = "]";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestLeftCurlyBraceToken()
    {
        const string operatorText = "{";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestRightCurlyBraceToken()
    {
        const string operatorText = "}";     

        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestColonToken()
    {
        const string operatorText = ":";     
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.COLON, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    public void TestCommaToken()
    {
        const string operatorText = ",";     
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.COMMA, token.Type);        
        Assert.Equal(operatorText, token.Value);
    }    
    
    [Fact]
    [Trait("Category", "SingleToken")]
    [Trait("Category", "SingleCharOperator")]
    [Trait("Category", "Unknown")]
    public void TestUnknownToken()
    {
        const string operatorText = ";";
        
        var token = GetSingleTokenFromLexerByText(operatorText);
        
        Assert.Equal(TokenType.UNKNOWN, token.Type);        
        Assert.Equal(";", token.Value);        
    }
    
    // Examples from gitlab code examples

    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "TokenPosition")]
    [Trait("Category", "Core")]
    public void TestIntScalarAssignmentTokens()
    {
        const string code = "let x: [] = 5";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[4].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[5].Type);  
        Assert.Equal("=", tokens[5].Value);
        Assert.Equal(TokenType.INT, tokens[6].Type);        
        Assert.Equal(5, tokens[6].Value);
        Assert.Equal(TokenType.ETX, tokens[7].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    public void TestIntScalarAssignmentWithoutSpacesTokens()
    {
        const string code = "let x:[]=5";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[4].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[5].Type);  
        Assert.Equal("=", tokens[5].Value);
        Assert.Equal(TokenType.INT, tokens[6].Type);        
        Assert.Equal(5, tokens[6].Value);
        Assert.Equal(TokenType.ETX, tokens[7].Type);
    }

    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestFloatScalarAssignmentTokens()
    {
        const string code = "let x: [] = 5.2";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[4].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[5].Type);  
        Assert.Equal("=", tokens[5].Value);
        Assert.Equal(TokenType.FLOAT, tokens[6].Type);        
        Assert.Equal(5.2, tokens[6].Value);
        Assert.Equal(TokenType.ETX, tokens[7].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestSiUnitAssignmentTokens()
    {
        const string code = "let duration: [s] = 5.1e2";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("duration", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[4].Type);        
        Assert.Equal("s", tokens[4].Value);
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[5].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[6].Type);  
        Assert.Equal("=", tokens[6].Value);
        Assert.Equal(TokenType.FLOAT, tokens[7].Type);        
        Assert.Equal(5.1e2, tokens[7].Value, 5);
        Assert.Equal(TokenType.ETX, tokens[8].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestStringAssignmentTokens()
    {
        const string code = "let myString: string = \"my string with \\\\ \\\n \\\t escape chars\"";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);  
        Assert.Equal("myString", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.STRING_TYPE, tokens[3].Type); 
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[4].Type);        
        Assert.Equal(TokenType.STRING, tokens[5].Type);        
        Assert.Equal("my string with \\ \n \t escape chars", tokens[5].Value);
        Assert.Equal(TokenType.ETX, tokens[6].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestBoolAssignmentTokens()
    {
        const string code = "let isDigit: bool = true";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("isDigit", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.BOOL_TYPE, tokens[3].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[4].Type);        
        Assert.Equal(TokenType.TRUE, tokens[5].Type);       
        Assert.Equal(TokenType.ETX, tokens[6].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestUnitDeclarationTokens()
    {
        const string code = "unit N: [kg*m*s^-2]";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.UNIT, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("N", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[4].Type);        
        Assert.Equal("kg", tokens[4].Value);
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, tokens[5].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[6].Type);        
        Assert.Equal("m", tokens[6].Value);
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, tokens[7].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[8].Type);        
        Assert.Equal("s", tokens[8].Value);
        Assert.Equal(TokenType.POWER_OPERATOR, tokens[9].Type);        
        Assert.Equal(TokenType.MINUS_OPERATOR, tokens[10].Type);   
        Assert.Equal(TokenType.INT, tokens[11].Type);        
        Assert.Equal(2, tokens[11].Value);        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[12].Type);       
        Assert.Equal(TokenType.ETX, tokens[13].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestComplexExpressionTokens()
    {
        const string code = "let x: bool = (firstFn(y, z) || force && www)";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("x", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.BOOL_TYPE, tokens[3].Type);
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[4].Type);
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[5].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[6].Type);        
        Assert.Equal("firstFn", tokens[6].Value);
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[7].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[8].Type);        
        Assert.Equal("y", tokens[8].Value);
        Assert.Equal(TokenType.COMMA, tokens[9].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[10].Type);        
        Assert.Equal("z", tokens[10].Value);
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[11].Type);
        Assert.Equal(TokenType.OR_OPERATOR, tokens[12].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[13].Type);        
        Assert.Equal("force", tokens[13].Value);
        Assert.Equal(TokenType.AND_OPERATOR, tokens[14].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[15].Type);        
        Assert.Equal("www", tokens[15].Value);
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[16].Type);
        Assert.Equal(TokenType.ETX, tokens[17].Type);        
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestComplexLiteralExpressionTokens()
    {
        const string code = "let test: [] = -(5-2)/y*2.5";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.LET, tokens[0].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);        
        Assert.Equal("test", tokens[1].Value);
        Assert.Equal(TokenType.COLON, tokens[2].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[3].Type);        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[4].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[5].Type);  
        Assert.Equal("=", tokens[5].Value);
        Assert.Equal(TokenType.MINUS_OPERATOR, tokens[6].Type);  
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[7].Type);        
        Assert.Equal(TokenType.INT, tokens[8].Type);        
        Assert.Equal(5, tokens[8].Value);
        Assert.Equal(TokenType.MINUS_OPERATOR, tokens[9].Type);  
        Assert.Equal(TokenType.INT, tokens[10].Type);        
        Assert.Equal(2, tokens[10].Value);
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[11].Type);
        Assert.Equal(TokenType.DIVISION_OPERATOR, tokens[12].Type);  
        Assert.Equal(TokenType.IDENTIFIER, tokens[13].Type);        
        Assert.Equal("y", tokens[13].Value);
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, tokens[14].Type);  
        Assert.Equal(TokenType.FLOAT, tokens[15].Type);        
        Assert.Equal(2.5, tokens[15].Value, 5);
        Assert.Equal(TokenType.ETX, tokens[16].Type);        
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "TokenPosition")]
    [Trait("Category", "Core")]
    public void TestIfBlockTokensAndPositionOfEach()
    {
        const string code = "if (force > 5: [N]) " +
                            "{\nlet x: [] = -0.2e2 } " +
                            "else if (duration <= 0: [s]) " +
                            "{\nprint(duration) } " +
                            "else {\nreturn 5.5 }";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.IF, tokens[0].Type);        
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[1].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);  
        Assert.Equal("force", tokens[2].Value);  
        Assert.Equal(TokenType.GREATER_THAN_OPERATOR, tokens[3].Type);        
        Assert.Equal(TokenType.INT, tokens[4].Type);
        Assert.Equal(5, tokens[4].Value);
        Assert.Equal(TokenType.COLON, tokens[5].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[6].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[7].Type);
        Assert.Equal("N", tokens[7].Value);
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[8].Type);        
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[9].Type);  
        
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, tokens[10].Type);        
        Assert.Equal(TokenType.LET, tokens[11].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[12].Type);
        Assert.Equal("x", tokens[12].Value);
        Assert.Equal(2, tokens[12].Position.RowNumber);
        Assert.Equal(5, tokens[12].Position.ColumnNumber);
        
        Assert.Equal(TokenType.COLON, tokens[13].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[14].Type);
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[15].Type);        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[16].Type);        
        Assert.Equal(TokenType.MINUS_OPERATOR, tokens[17].Type);        
        Assert.Equal(TokenType.FLOAT, tokens[18].Type);
        Assert.Equal(0.2e2, tokens[18].Value, 4);
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, tokens[19].Type);
        
        Assert.Equal(TokenType.ELSE, tokens[20].Type);
        Assert.Equal(TokenType.IF, tokens[21].Type);
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[22].Type);  
        Assert.Equal(TokenType.IDENTIFIER, tokens[23].Type);  
        Assert.Equal("duration", tokens[23].Value);  
        Assert.Equal(TokenType.SMALLER_EQUAL_THAN_OPERATOR, tokens[24].Type);        
        Assert.Equal(TokenType.INT, tokens[25].Type);
        Assert.Equal(0, tokens[25].Value);
        Assert.Equal(TokenType.COLON, tokens[26].Type);        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[27].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[28].Type);
        Assert.Equal("s", tokens[28].Value);
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[29].Type);        
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[30].Type);  
        
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, tokens[31].Type);        
        Assert.Equal(TokenType.IDENTIFIER, tokens[32].Type);
        Assert.Equal("print", tokens[32].Value);
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[33].Type);  
        Assert.Equal(TokenType.IDENTIFIER, tokens[34].Type);  
        Assert.Equal("duration", tokens[34].Value);  
        Assert.Equal(3, tokens[34].Position.RowNumber);
        Assert.Equal(7, tokens[34].Position.ColumnNumber);
        
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[35].Type);  
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, tokens[36].Type);
        
        Assert.Equal(TokenType.ELSE, tokens[37].Type);
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, tokens[38].Type);        
        Assert.Equal(TokenType.RETURN, tokens[39].Type);
        Assert.Equal(4, tokens[39].Position.RowNumber);
        Assert.Equal(1, tokens[39].Position.ColumnNumber);
        
        Assert.Equal(TokenType.FLOAT, tokens[40].Type);
        Assert.Equal(5.5, tokens[40].Value, 4);
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, tokens[41].Type);

        Assert.Equal(TokenType.ETX, tokens[42].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "TokenPosition")]
    [Trait("Category", "Core")]
    public void TestWhileTokensAndPositionOfEach()
    {
        const string code = "while (i != 0) {\nlet energy: [J] = 10 * i\n}";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.WHILE, tokens[0].Type);    
        Assert.Equal(1, tokens[0].Position.RowNumber);
        Assert.Equal(1, tokens[0].Position.ColumnNumber);
        
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[1].Type);   
        Assert.Equal(1, tokens[1].Position.RowNumber);
        Assert.Equal(7, tokens[1].Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);  
        Assert.Equal("i", tokens[2].Value);  
        Assert.Equal(1, tokens[2].Position.RowNumber);
        Assert.Equal(8, tokens[2].Position.ColumnNumber);
        
        Assert.Equal(TokenType.NOT_EQUAL_OPERATOR, tokens[3].Type);  
        Assert.Equal(1, tokens[3].Position.RowNumber);
        Assert.Equal(10, tokens[3].Position.ColumnNumber);
        
        Assert.Equal(TokenType.INT, tokens[4].Type);  
        Assert.Equal(0, tokens[4].Value); 
        Assert.Equal(1, tokens[4].Position.RowNumber);
        Assert.Equal(13, tokens[4].Position.ColumnNumber);
        
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[5].Type); 
        Assert.Equal(1, tokens[5].Position.RowNumber);
        Assert.Equal(14, tokens[5].Position.ColumnNumber);
        
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, tokens[6].Type);
        Assert.Equal(1, tokens[6].Position.RowNumber);
        Assert.Equal(16, tokens[6].Position.ColumnNumber);
        
        Assert.Equal(TokenType.LET, tokens[7].Type);
        Assert.Equal(2, tokens[7].Position.RowNumber);
        Assert.Equal(1, tokens[7].Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[8].Type);  
        Assert.Equal("energy", tokens[8].Value);  
        Assert.Equal(2, tokens[8].Position.RowNumber);
        Assert.Equal(5, tokens[8].Position.ColumnNumber);
        
        Assert.Equal(TokenType.COLON, tokens[9].Type);
        Assert.Equal(2, tokens[9].Position.RowNumber);
        Assert.Equal(11, tokens[9].Position.ColumnNumber);
        
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[10].Type);
        Assert.Equal(2, tokens[10].Position.RowNumber);
        Assert.Equal(13, tokens[10].Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[11].Type);  
        Assert.Equal("J", tokens[11].Value);  
        Assert.Equal(2, tokens[11].Position.RowNumber);
        Assert.Equal(14, tokens[11].Position.ColumnNumber);
        
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[12].Type);
        Assert.Equal(2, tokens[12].Position.RowNumber);
        Assert.Equal(15, tokens[12].Position.ColumnNumber);
        
        Assert.Equal(TokenType.ASSIGNMENT_OPERATOR, tokens[13].Type);
        Assert.Equal(2, tokens[13].Position.RowNumber);
        Assert.Equal(17, tokens[13].Position.ColumnNumber);
        
        Assert.Equal(TokenType.INT, tokens[14].Type);  
        Assert.Equal(10, tokens[14].Value);
        Assert.Equal(2, tokens[14].Position.RowNumber);
        Assert.Equal(19, tokens[14].Position.ColumnNumber);
        
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, tokens[15].Type);
        Assert.Equal(2, tokens[15].Position.RowNumber);
        Assert.Equal(22, tokens[15].Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[16].Type);  
        Assert.Equal("i", tokens[16].Value);  
        Assert.Equal(2, tokens[16].Position.RowNumber);
        Assert.Equal(24, tokens[16].Position.ColumnNumber);
        
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, tokens[17].Type);
        Assert.Equal(3, tokens[17].Position.RowNumber);
        Assert.Equal(1, tokens[17].Position.ColumnNumber);

        Assert.Equal(TokenType.ETX, tokens[18].Type);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "Core")]
    public void TestFunctionTokens()
    {
        const string code = "calculateForceDelta(N1: [N], N2: [N], scalar: []) -> [N] {" +
                            "\nreturn (N2-N1) * scalar }";

        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[0].Type);  
        Assert.Equal("calculateForceDelta", tokens[0].Value);
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[1].Type);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);  
        Assert.Equal("N1", tokens[2].Value); 
        Assert.Equal(TokenType.COLON, tokens[3].Type);
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[4].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[5].Type);  
        Assert.Equal("N", tokens[5].Value);  
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[6].Type); 
        
        Assert.Equal(TokenType.COMMA, tokens[7].Type);

        Assert.Equal(TokenType.IDENTIFIER, tokens[8].Type);  
        Assert.Equal("N2", tokens[8].Value); 
        Assert.Equal(TokenType.COLON, tokens[9].Type);
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[10].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[11].Type);  
        Assert.Equal("N", tokens[11].Value);  
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[12].Type); 
        
        Assert.Equal(TokenType.COMMA, tokens[13].Type);

        Assert.Equal(TokenType.IDENTIFIER, tokens[14].Type);  
        Assert.Equal("scalar", tokens[14].Value); 
        Assert.Equal(TokenType.COLON, tokens[15].Type);
        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[16].Type);
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[17].Type); 
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[18].Type); 
        
        Assert.Equal(TokenType.RETURN_ARROW, tokens[19].Type); 

        Assert.Equal(TokenType.LEFT_SQUARE_BRACKET, tokens[20].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[21].Type);  
        Assert.Equal("N", tokens[21].Value);  
        Assert.Equal(TokenType.RIGHT_SQUARE_BRACKET, tokens[22].Type); 
        
        Assert.Equal(TokenType.LEFT_CURLY_BRACE, tokens[23].Type);
        
        Assert.Equal(TokenType.RETURN, tokens[24].Type);

        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[25].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[26].Type);  
        Assert.Equal("N2", tokens[26].Value); 
        Assert.Equal(TokenType.MINUS_OPERATOR, tokens[27].Type);
        Assert.Equal(TokenType.IDENTIFIER, tokens[28].Type);  
        Assert.Equal("N1", tokens[28].Value); 
        Assert.Equal(TokenType.RIGHT_PARENTHESES, tokens[29].Type); 
        
        Assert.Equal(TokenType.MULTIPLICATION_OPERATOR, tokens[30].Type);

        Assert.Equal(TokenType.IDENTIFIER, tokens[31].Type);  
        Assert.Equal("scalar", tokens[31].Value); 
        
        Assert.Equal(TokenType.RIGHT_CURLY_BRACE, tokens[32].Type); 

        Assert.Equal(TokenType.ETX, tokens[33].Type);
    }


    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    public void TestTokenPositionOfKeywordAndIdentifier()
    {
        const string keywordAndIdentifierText = "returnn myVariable";
        
        var tokens = GetAllTokensFromLexerByText(keywordAndIdentifierText);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[0].Type);
        Assert.Equal("returnn", tokens[0].Value);

        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
        Assert.Equal("myVariable", tokens[1].Value);
    }


    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "TokenPosition")]
    public void TestTokenPositionWithNewLineInside()
    {
        const string keywordAndIdentifierText = "return }\nlet force";
        
        var tokens = GetAllTokensFromLexerByText(keywordAndIdentifierText);
        
        // tokens: return, curlyBrace, let, identifier
        var returnToken = tokens[0];
        var letToken = tokens[2];
        var identifierToken = tokens[3];
        
        Assert.Equal(TokenType.RETURN, returnToken.Type);
        Assert.Equal(1, returnToken.Position.RowNumber);
        Assert.Equal(1, returnToken.Position.ColumnNumber);
        
        Assert.Equal(TokenType.LET, letToken.Type);
        Assert.Equal(2, letToken.Position.RowNumber);
        Assert.Equal(1, letToken.Position.ColumnNumber);
        
        Assert.Equal(TokenType.IDENTIFIER, identifierToken.Type);
        Assert.Equal("force", identifierToken.Value);
        Assert.Equal(2, identifierToken.Position.RowNumber);
        Assert.Equal(5, identifierToken.Position.ColumnNumber);
    }
    
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    public void TestDeclareLettIdentifier()
    {
        const string declareLett = "let lett";

        var tokens = GetAllTokensFromLexerByText(declareLett);

        Assert.Equal(TokenType.LET, tokens[0].Type);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[1].Type);
        Assert.Equal("lett", tokens[1].Value);
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    [Trait("Category", "TokenPosition")]
    [Trait("Category", "Comment")]
    public void TestCommentWithNewLineToken()
    {
        const string let = "let x";
        const string code = "//" + let + "\n" + let;
        
        var tokens = GetAllTokensFromLexerByText(code);
        
        Assert.Equal(TokenType.COMMENT, tokens[0].Type);        
        Assert.Equal(let, tokens[0].Value);        
        Assert.Equal(1, tokens[0].Position.RowNumber);        
        Assert.Equal(1, tokens[0].Position.ColumnNumber);      
        
        Assert.Equal(TokenType.LET, tokens[1].Type);        
        Assert.Equal(2, tokens[1].Position.RowNumber);        
        Assert.Equal(1, tokens[1].Position.ColumnNumber); 
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);        
        Assert.Equal("x", tokens[2].Value);        
        Assert.Equal(2, tokens[2].Position.RowNumber);        
        Assert.Equal(5, tokens[2].Position.ColumnNumber); 
    }
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    [Trait("Category", "TokenPosition")]
    public void TestNewlineDifferentCombinations()
    {
        const string newLines = "let x\n\r" +
                                "let y\n" +
                                "let z\r\n" +
                                "let w\r" +
                                "let q";
        
        var tokens = GetAllTokensFromLexerByText(newLines);

        var x = tokens[1];
        var y = tokens[3];
        var z = tokens[5];
        var w = tokens[7];
        var q = tokens[9];
        
        Assert.Equal(1, x.Position.RowNumber);        
        Assert.Equal(5, x.Position.ColumnNumber); 
        Assert.Equal(2, y.Position.RowNumber);        
        Assert.Equal(5, y.Position.ColumnNumber); 
        Assert.Equal(3, z.Position.RowNumber);        
        Assert.Equal(5, z.Position.ColumnNumber); 
        Assert.Equal(4, w.Position.RowNumber);        
        Assert.Equal(5, w.Position.ColumnNumber); 
        Assert.Equal(5, q.Position.RowNumber);        
        Assert.Equal(5, q.Position.ColumnNumber); 
    }
    
    
    [Fact]
    [Trait("Category", "MultiToken")]
    [Trait("Category", "EdgeCase")]
    public void TestIffIdentifier()
    {
        const string iffText = "if ( iff > 5)";

        var tokens = GetAllTokensFromLexerByText(iffText);

        Assert.Equal(TokenType.IF, tokens[0].Type);
        
        Assert.Equal(TokenType.LEFT_PARENTHESES, tokens[1].Type);
        
        Assert.Equal(TokenType.IDENTIFIER, tokens[2].Type);
        Assert.Equal("iff", tokens[2].Value);

        Assert.Equal(TokenType.GREATER_THAN_OPERATOR, tokens[3].Type);
        Assert.Equal(">", tokens[3].Value);

        Assert.Equal(TokenType.INT, tokens[4].Type);
        Assert.Equal(5, tokens[4].Value);
    }

    // HELPER FUNCTIONS
    private static Token GetSingleTokenFromLexerByText(
        string textToLexer, 
        int maxCommentLength = 1000,
        int maxIdentifierLength = 50,
        int maxTextLength = 1000,
        int maxDecimalPlaces = 10,
        int maxExponentSize = 10,
        long maxIntSize = 10000
        )
    {
        var streamReader = Helper.GetStreamReaderFromString(textToLexer);
        var lexer = new Lexer(
            streamReader, 
            maxCommentLength,
            maxIdentifierLength,
            maxTextLength,
            maxDecimalPlaces,
            maxExponentSize,
            maxIntSize
            );       

        lexer.GetNextToken();
        streamReader.Close();

        return lexer.Token;
    }

    private static List<Token> GetAllTokensFromLexerByText(string textToLexer)
    {
        var streamReader = Helper.GetStreamReaderFromString(textToLexer);
        var lexer = new Lexer(streamReader);     
        
        var tokens = new List<Token>();

        lexer.GetNextToken();
        while (lexer.Token.Type != TokenType.ETX)
        {
            tokens.Add(lexer.Token);
            lexer.GetNextToken();
        }
        // Add ETX token
        tokens.Add(lexer.Token);

        streamReader.Close();

        return tokens;
    }



}
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit.expression;
using si_unit_interpreter.parser.unit.expression.power;
using Xunit;

namespace si_unit_interpreter.spec;

public class ParserUnitTests
{
    [Fact]
    [Trait("Category", "Assignment")]
    public void TestBoolAssignment()
    {
        const string code = "let x: bool = true";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Variables);

        var assignStatement = program.Variables.First();
        Assert.Equal("x", assignStatement.Parameter.Identifier);
        Assert.IsType<BoolType>(assignStatement.Parameter.Type);
        Assert.IsType<BoolLiteral>(assignStatement.Expression);

        var boolLiteral = (BoolLiteral) assignStatement.Expression;
        Assert.True(boolLiteral.Value);
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestStringAssignment()
    {
        const string code = "let s: string = \"my string\"";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Variables);

        var assignStatement = program.Variables.First();
        Assert.Equal("s", assignStatement.Parameter.Identifier);
        Assert.IsType<StringType>(assignStatement.Parameter.Type);
        Assert.IsType<StringLiteral>(assignStatement.Expression);

        var stringLiteral = (StringLiteral) assignStatement.Expression;
        Assert.Equal("my string", stringLiteral.Value);
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestIntUnitScalarAssignment()
    {
        const string code = "let mul: [] = 5";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Variables);

        var assignStatement = program.Variables.First();
        Assert.Equal("mul", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);
        Assert.IsType<IntLiteral>(assignStatement.Expression);

        var intLiteral = (IntLiteral) assignStatement.Expression;
        Assert.Equal(5, intLiteral.Value);
        
        Assert.Null(intLiteral.UnitType);
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestFloatUnitScalarAssignment()
    {
        const string code = "let mulFloat: [] = 5e2";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Variables);

        var assignStatement = program.Variables.First();
        Assert.Equal("mulFloat", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);
        Assert.IsType<FloatLiteral>(assignStatement.Expression);

        var floatLiteral = (FloatLiteral) assignStatement.Expression;
        Assert.Equal(5e2, floatLiteral.Value);
        
        Assert.Null(floatLiteral.UnitType);
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestUnitAssignment()
    {
        const string code = "let force: [m*s^-2] = 5.23";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Variables);

        var assignStatement = program.Variables.First();
        Assert.Equal("force", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);
        Assert.IsType<FloatLiteral>(assignStatement.Expression);

        var unitType = (UnitType) assignStatement.Parameter.Type;
        var unitExpression = (UnitExpression) unitType.Expression!;
        
        var leftExpression = (UnitUnaryExpression) unitExpression.Left;
        Assert.Equal("m", leftExpression.Identifier);
        Assert.Null(leftExpression.UnitPower);
        
        var rightExpression = (UnitUnaryExpression) unitExpression.Right!;
        Assert.Equal("s", rightExpression.Identifier);
        Assert.IsType<UnitMinusPower>(rightExpression.UnitPower);
        var rightUnitPower = (UnitMinusPower) rightExpression.UnitPower!;
        Assert.Equal(2, rightUnitPower.Value);

        
        var floatLiteral = (FloatLiteral) assignStatement.Expression;
        Assert.Equal(5.23, floatLiteral.Value);
    }
    
    

    private Parser PrepareParser(string code)
    {
        var lexer = new Lexer(Helper.GetStreamReaderFromString(code));

        return new Parser(lexer);
    }
}
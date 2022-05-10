using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
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
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var assignStatement = (AssignStatement) firstStatement;
        Assert.Equal("x", assignStatement.Parameter.Identifier);
        Assert.IsType<BoolType>(assignStatement.Parameter.Type);

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
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var assignStatement = (AssignStatement) firstStatement;
        Assert.Equal("s", assignStatement.Parameter.Identifier);
        Assert.IsType<StringType>(assignStatement.Parameter.Type);

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
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var assignStatement = (AssignStatement) firstStatement;
        Assert.Equal("mul", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);

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
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var assignStatement = (AssignStatement) firstStatement;
        Assert.Equal("mulFloat", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);

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
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var assignStatement = (AssignStatement) firstStatement;
        Assert.Equal("force", assignStatement.Parameter.Identifier);
        Assert.IsType<UnitType>(assignStatement.Parameter.Type);

        var unitType = (UnitType) assignStatement.Parameter.Type;
        var unitExpression = (UnitExpression) unitType.Expression!;
        
        var leftExpression = (UnitUnaryExpression) unitExpression.Left;
        Assert.Equal("m", leftExpression.Identifier);
        Assert.Null(leftExpression.UnitPower);
        
        var rightExpression = (UnitUnaryExpression) unitExpression.Right!;
        Assert.Equal("s", rightExpression.Identifier);
        var rightUnitPower = (UnitMinusPower) rightExpression.UnitPower!;
        Assert.Equal(2, rightUnitPower.Value);

        
        var floatLiteral = (FloatLiteral) assignStatement.Expression;
        Assert.Equal(5.23, floatLiteral.Value);
    }

    [Fact]
    [Trait("Category", "FunctionCall")]
    public void TestFunctionCall()
    {
        const string code = "print(myVariable)";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);
        
        var firstStatement = program.Statements.First();

        var functionCall = (FunctionCall) firstStatement;
        
        Assert.Equal("print", functionCall.Name);
        var argument = functionCall.Arguments[0];
        var identifierArgument = (Identifier) argument;
        Assert.Equal("myVariable", identifierArgument.Name);
    }
    
    [Fact]
    [Trait("Category", "ReturnStatement")]
    public void TestVoidReturnStatement()
    {
        const string code = "return";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();
        var returnStatement = (ReturnStatement) firstStatement;

        Assert.Null(returnStatement.Expression);
    }
    
    [Fact]
    [Trait("Category", "ReturnStatement")]
    public void TestReturnStatement()
    {
        const string code = "return x + 5";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();
        var returnStatement = (ReturnStatement) firstStatement;

        var expression = returnStatement.Expression;
        var addExpression = (AddExpression) expression!;

        var left = (Identifier) addExpression.Left;
        var right = (IntLiteral) addExpression.Right!;
        
        Assert.Equal("x", left.Name);
        Assert.Equal(5, right.Value);
        Assert.Null(right.UnitType);
    }
    
    [Fact]
    [Trait("Category", "WhileStatement")]
    public void TestWhileStatement()
    {
        const string code = "while(y > 18) { print(y) }";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var firstStatement = program.Statements.First();

        var whileStatement = (WhileStatement) firstStatement;

        var gtExpression = (GreaterThanExpression) whileStatement.Condition;
        var left = (Identifier) gtExpression.Left;
        var right = (IntLiteral) gtExpression.Right!;
        
        Assert.Equal("y",left.Name);
        Assert.Equal(18, right.Value);
        Assert.Null(right.UnitType);
        
        Assert.Single(whileStatement.Statements);

        var functionCall = (FunctionCall) whileStatement.Statements[0];

        Assert.Equal("print", functionCall.Name);

        Assert.Single(functionCall.Arguments);

        var argument = (Identifier) functionCall.Arguments[0];

        Assert.Equal("y", argument.Name);
    }
    
    [Fact]
    [Trait("Category", "IfStatement")]
    public void TestIfStatement()
    {
        const string code = "if(force > 5: [N]) {\n" +
                            "force = force * 2\n" +
                            "print(force)\n" +
                            "}\n" +
                            "else if(force > 0: [N]) {\n" +
                            "print(force)\n" +
                            "}\n" +
                            "else {\n" +
                            "let message: string = \"Less than 0\"\n" +
                            "print(message)\n" +
                            "}";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);
        
    }
    
    [Fact]
    [Trait("Category", "FunctionStatement")]
    public void TestFunctionStatement()
    {
        const string code = "fn calculateVelocityData(v1: [m*s^-1], v2: [m*s^-1], scalar: []) -> [m*s^-1] {\n" +
                            "return (v2-v1) * scalar\n" +
                            "}";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Functions);
        Assert.Empty(program.Statements);

        var (name, body) = program.Functions.First();
        Assert.Equal("calculateVelocityData", name);

        Assert.Single(body);
        var returnStatement = (ReturnStatement) body.First();

        var multiplicateExpression = (MultiplicateExpression) returnStatement.Expression!;
        var leftMult = (SubtractExpression) multiplicateExpression.Left;
        var rightMult = (Identifier) multiplicateExpression.Right!;

        var leftSubtract = (Identifier) leftMult.Left;
        var rightSubtract = (Identifier) leftMult.Right!;
        
        Assert.Equal("v2", leftSubtract.Name);        
        Assert.Equal("v1", rightSubtract.Name);        
        Assert.Equal("scalar", rightMult.Name);        
    }
    
    [Fact]
    [Trait("Category", "UnitDeclaration")]
    public void TestUnitDeclaration()
    {
        const string code = "unit N: [kg*m*s^-2]";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Empty(program.Statements);
        Assert.Empty(program.Functions);
        Assert.Single(program.Units);
        
        var (identifier, unitBody) = program.Units.First();
        
        Assert.Equal("N", identifier);

        var unitExpression = (UnitExpression) unitBody.Expression!;
        var left = (UnitExpression) unitExpression.Left;

        var kg = (UnitUnaryExpression) left.Left;
        var m = (UnitUnaryExpression) left.Right!;
        
        var s = (UnitUnaryExpression) unitExpression.Right!;

        Assert.Equal("kg", kg.Identifier);
        var kgUnitPower = (UnitPower) kg.UnitPower!;
        Assert.Null(kgUnitPower);
        
        Assert.Equal("m", m.Identifier);
        var mUnitPower = (UnitPower) m.UnitPower!;
        Assert.Null(mUnitPower);
        
        Assert.Equal("s", s.Identifier);
        var sUnitPower = (UnitMinusPower) s.UnitPower!;
        Assert.Equal(2, sUnitPower.Value);
    }
    
    [Fact]
    [Trait("Category", "FunctionStatement")]
    [Trait("Category", "MultiStatement")]
    public void TestFunctionStatementsWithCalls()
    {
        const string code = "fn printCustomMessage(m: string) -> void {\n" +
                            "print(\"Custom: \")\n" +
                            "print(m)\n" +
                            "}\n" +
                            "fn printCustomMessage2(m: string) -> void {\n" +
                            "print(\"Custom2: \")\n" +
                            "print(m)\n" +
                            "}\n" +
                            "let m: string = \"My message\"\n" +
                            "printCustomMessage(m)\n" +
                            "printCustomMessage2(m)";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);
    }

    [Fact]
    [Trait("Category", "Expression")]
    public void TestLogicExpression()
    {
        const string code = "let x: bool = (firstVariable || www && xyz) == false";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var assignStatement = (AssignStatement) program.Statements.First();
        var equalExpression = (EqualExpression) assignStatement.Expression;

        var leftEq = (Expression) equalExpression.Left;
        var rightEq = (BoolLiteral) equalExpression.Right!;
        Assert.False(rightEq.Value);

        var leftExpr = (Identifier) leftEq.Left;
        Assert.Equal("firstVariable", leftExpr.Name);

        var rightExpr = (LogicFactor) leftEq.Right!;

        var leftLogic = (Identifier) rightExpr.Left;
        var rightLogic = (Identifier) rightExpr.Right!;
        
        Assert.Equal("www", leftLogic.Name);
        Assert.Equal("xyz", rightLogic.Name);
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestComparisonExpression()
    {
        const string code = "let x: bool = x > 5 && x <= 40";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var assignStatement = (AssignStatement) program.Statements.First();
        var logicFactor = (LogicFactor) assignStatement.Expression;

        var greaterThanExpression = (GreaterThanExpression) logicFactor.Left;
        var smallerEqualThanExpression = (SmallerEqualThanExpression) logicFactor.Right!;

        var gtIdentifier = (Identifier) greaterThanExpression.Left;
        var gtIntLiteral = (IntLiteral) greaterThanExpression.Right!;
        
        Assert.Equal("x", gtIdentifier.Name);
        Assert.Equal(5, gtIntLiteral.Value);
        
        var stIdentifier = (Identifier) smallerEqualThanExpression.Left;
        var stIntLiteral = (IntLiteral) smallerEqualThanExpression.Right!;
        
        Assert.Equal("x", stIdentifier.Name);
        Assert.Equal(40, stIntLiteral.Value);
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestAdditiveAndMultiplicativeExpression()
    {
        const string code = "let x: [] = 2 + 3 * 4.2 - 2e1 / 8";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);
        
        var assignStatement = (AssignStatement) program.Statements.First();
        var subtractExpression = (SubtractExpression) assignStatement.Expression;

        var addExpression = (AddExpression) subtractExpression.Left;

        var leftLiteral = (IntLiteral) addExpression.Left;
        Assert.Equal(2, leftLiteral.Value);
        var multiplicateExpression = (MultiplicateExpression) addExpression.Right!;

        var leftMultiplicateLiteral = (IntLiteral) multiplicateExpression.Left;
        var rightMultiplicateLiteral = (FloatLiteral) multiplicateExpression.Right!;
        
        Assert.Equal(3, leftMultiplicateLiteral.Value);
        Assert.Equal(4.2, rightMultiplicateLiteral.Value);
        
        var divideExpression = (DivideExpression) subtractExpression.Right!;

        var leftDivideLiteral = (FloatLiteral) divideExpression.Left;
        var rightDivideLiteral = (IntLiteral) divideExpression.Right!;
        
        Assert.Equal(2e1, leftDivideLiteral.Value);
        Assert.Equal(8, rightDivideLiteral.Value);
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestExpressionWithParentheses()
    {
        const string code = "let x: [] = (2 + 3) * 4";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);
        
        var assignStatement = (AssignStatement) program.Statements.First();
        var multiplicateExpression = (MultiplicateExpression) assignStatement.Expression;

        var leftMultiplicateExpression = (AddExpression) multiplicateExpression.Left;
        var rightLiteral = (IntLiteral) multiplicateExpression.Right!;
        
        Assert.Equal(4, rightLiteral.Value);

        var leftAddLiteral = (IntLiteral) leftMultiplicateExpression.Left;
        var rightAddLiteral = (IntLiteral) leftMultiplicateExpression.Right!;
        
        Assert.Equal(2, leftAddLiteral.Value);
        Assert.Equal(3, rightAddLiteral.Value);
    }
    

    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestNegateExpression()
    {
        const string code = "let x: bool = -5.5 < -2 && !isEqual != 7 >= y";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

        var assignStatement = (AssignStatement) program.Statements.First();
        var logicFactor = (LogicFactor) assignStatement.Expression;

        var smallerThanExpression = (SmallerThanExpression) logicFactor.Left;

        var leftMinusExpression = (MinusExpression) smallerThanExpression.Left;
        var leftMinusLiteral = (FloatLiteral) leftMinusExpression.Child;
        Assert.Equal(5.5, leftMinusLiteral.Value);
        
        var rightMinusExpression = (MinusExpression) smallerThanExpression.Right!;
        var rightMinusLiteral = (IntLiteral) rightMinusExpression.Child;
        Assert.Equal(2, rightMinusLiteral.Value);

        var greaterEqualThanExpression = (GreaterEqualThanExpression) logicFactor.Right!;

        var notEqualExpression = (NotEqualExpression) greaterEqualThanExpression.Left;

        var notExpression = (NotExpression) notEqualExpression.Left;
        var notExpressionIdentifier = (Identifier) notExpression.Child;
        Assert.Equal("isEqual", notExpressionIdentifier.Name);

        var notEqualLiteral = (IntLiteral) notEqualExpression.Right!;
        Assert.Equal(7, notEqualLiteral.Value);

        var rightIdentifier = (Identifier) greaterEqualThanExpression.Right!;
        Assert.Equal("y", rightIdentifier.Name);
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestExpressionWithFunctionCalls()
    {
        const string code = "let x: [N] = getFirstForce(x, y) - getSecondForce()";

        var parser = PrepareParser(code);
        var program = parser.Parse();
        
        Assert.Single(program.Statements);

    }

    [Fact]
    [Trait("Category", "Core")]
    public void TestCoreFunctionalityWithCalculatingGForce()
    {
        const string code = "unit N: [kg*m*s^-2]\n" +
                            "" +
                            "fn calculateGForce(m1: [kg], m2: [kg], distance: [m]) -> [N] {\n" +
                            "let G: [N*m^2*kg^-2] = 6.6732e-11\n" +
                            "return G * earthMass * sunMass / (earthSunDistance * earthSunDistance)\n" +
                            "}\n" +
                            "" +
                            "let earthMass: [kg] = 5.9722e24\n" +
                            "let sunMass: [kg] = 1.989e30\n" +
                            "let earthSunDistance: [m] = 149.24e9\n" +
                            "let gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)\n" +
                            "" +
                            "print(gForce)";
    }
    

    private static Parser PrepareParser(string code)
    {
        var lexer = new Lexer(Helper.GetStreamReaderFromString(code));

        return new Parser(lexer);
    }
}
using Newtonsoft.Json;
using Shouldly;
using si_unit_interpreter.lexer;
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
        const string code = @"
                            main() -> void {
                                let x: bool = true
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration = 
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new BoolType()
                ), 
                new BoolLiteral(true)
                );

        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestStringAssignment()
    {
        const string code = @"
                            main() -> void {
                                let s: string = ""my string""
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration = 
            new VariableDeclaration(
                new Parameter(
                    "s", 
                    new StringType()
                ), 
                new StringLiteral("my string")
                );

        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestIntUnitScalarAssignment()
    {
        const string code = @"
                            main() -> void {
                                let mul: [] = 5
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration = 
            new VariableDeclaration(
                new Parameter(
                    "mul", 
                    new UnitType(null)
                ),
                new IntLiteral(5, null));

        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestFloatUnitScalarAssignment()
    {
        const string code = @"
                            main() -> void {
                                let mulFloat: [] = 5e2
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration = 
            new VariableDeclaration(
                new Parameter(
                    "mulFloat",
                    new UnitType(null)
                ),
                new FloatLiteral(5e2, null)
                );
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }

    [Fact]
    [Trait("Category", "Assignment")]
    public void TestUnitAssignment()
    {
        const string code = @"
                            main() -> void {
                                let speed: [m*s^-2] = 5.23
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration = 
            new VariableDeclaration(
                new Parameter(
                    "speed", 
                    new UnitType(
                        new UnitExpression(
                            new UnitUnaryExpression(
                                "m",
                                null
                            ),
                            new UnitUnaryExpression(
                                "s", 
                                new UnitMinusPower(2)
                                )
                            )
                        )
                ), 
                new FloatLiteral(5.23, null)
                );

        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }

    [Fact]
    [Trait("Category", "FunctionCall")]
    public void TestFunctionCall()
    {
        const string code = @"
                            main() -> void {
                                print(myVariable)
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var functionCall = 
            new FunctionCall(
                "print",
                new List<IExpression>
                {
                    new Identifier("myVariable")
                }
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(functionCall));
    }
    
    [Fact]
    [Trait("Category", "ReturnStatement")]
    public void TestVoidReturnStatement()
    {
        const string code = @"
                            main() -> void {
                                return
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var returnStatement =
            new ReturnStatement(null);
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(returnStatement));
    }
    
    [Fact]
    [Trait("Category", "ReturnStatement")]
    public void TestReturnStatement()
    {
        const string code = @"
                            main() -> void {
                                return x + 5
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var returnStatement =
            new ReturnStatement(
                new AddExpression(
                    new Identifier(
                        "x"
                        ),
                    new IntLiteral(
                        5,
                        null
                        )
                    )
                );
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(returnStatement));
    }
    
    [Fact]
    [Trait("Category", "WhileStatement")]
    public void TestWhileStatement()
    {
        const string code = @"
                            main() -> void {
                                while(y > 18) { 
                                    print(y) 
                                    y = y + 1 
                                }
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var whileStatement =
            new WhileStatement(
                new GreaterThanExpression(
                    new Identifier(
                        "y"
                    ),
                    new IntLiteral(
                        18,
                        null
                    )
                ),
                new Block(
                    new List<IStatement>
                    {
                        new FunctionCall(
                            "print",
                            new List<IExpression>
                            {
                                new Identifier(
                                    "y"
                                )
                            }
                        ),
                        new AssignStatement(
                            "y",
                            new AddExpression(
                                new Identifier(
                                    "y"
                                ),
                                new IntLiteral(
                                    1,
                                    null
                                )
                            )
                        )
                    }
                )
            );
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(whileStatement));
    }
    //
    // [Fact]
    // [Trait("Category", "IfStatement")]
    // public void TestIfStatement()
    // {
    //     const string code = @"
    //                         if(force > 12 [N]) {
    //                             print(force + f(1))
    //                         }
    //                         else if(g(force) < 3.5 [N]) {
    //                             printCustom1(force, g(2 * x))
    //                         }
    //                         else if(force >= 0 [N]) {
    //                             printCustom2(force)
    //                             print(force)
    //                         }
    //                         else {
    //                             print(""Less than 0"")
    //                         }";
    //
    //     var parser = PrepareParser(code);
    //     var program = parser.Parse();
    //     
    //     Assert.Single(program.Statements);
    //
    //     var ifStatement = (IfStatement) program.Statements.First();
    //     
    //     Assert.Equal(1, ifStatement.Statements.Count);
    //     Assert.Equal(2, ifStatement.ElseIfStatements!.Count);
    //     Assert.Equal(1, ifStatement.ElseStatement!.Count);
    //     
    //     var firstElseIfStatement = ifStatement.ElseIfStatements[0];
    //     Assert.Equal(1, firstElseIfStatement.Statements.Count);
    //
    //     var secondElseIfStatement = ifStatement.ElseIfStatements[1];
    //     Assert.Equal(2, secondElseIfStatement.Statements.Count);
    //     
    //     var ifCondition = (GreaterThanExpression) ifStatement.Condition;
    //
    //     var leftIfCondition = (Identifier) ifCondition.Left;
    //     var rightIfCondition = (IntLiteral) ifCondition.Right!;
    //     
    //     Assert.Equal("force", leftIfCondition.Name);
    //
    //     var ifConditionUnitExpression = (UnitUnaryExpression) rightIfCondition.UnitType!.Expression!;
    //     
    //     Assert.Equal(12, rightIfCondition.Value);
    //     Assert.Equal("N", ifConditionUnitExpression.Name);
    //
    //     var ifFunctionCall = (FunctionCall) ifStatement.Statements[0];
    //     Assert.Equal("print",ifFunctionCall.Name);
    //     
    //     var ifFunctionCallArgument = (AddExpression) ifFunctionCall.Arguments[0];
    //
    //     var leftIfFunctionCallArgument = (Identifier) ifFunctionCallArgument.Left;
    //     Assert.Equal("force", leftIfFunctionCallArgument.Name);
    //     
    //     var rightIfFunctionCallArgument = (FunctionCall) ifFunctionCallArgument.Right!;
    //     Assert.Equal("f", rightIfFunctionCallArgument.Name);
    //
    //     var intLiteralRightIfFunctionCallArgument = (IntLiteral) rightIfFunctionCallArgument.Arguments[0];
    //     Assert.Equal(1, intLiteralRightIfFunctionCallArgument.Value);
    //
    //     var firstElseIfCondition = (SmallerThanExpression) firstElseIfStatement.Condition;
    //
    //     var leftFirstElseIfCondition = (FunctionCall) firstElseIfCondition.Left;
    //     Assert.Equal("g", leftFirstElseIfCondition.Name);
    //     var argumentLeftFirstElseIfCondition = (Identifier) leftFirstElseIfCondition.Arguments[0];
    //     Assert.Equal("force",argumentLeftFirstElseIfCondition.Name);
    //
    //     var rightFirstElseIfCondition = (FloatLiteral) firstElseIfCondition.Right!;
    //     var rightFirstElseIfConditionUnitExpression = (UnitUnaryExpression) rightFirstElseIfCondition.UnitType!.Expression!;
    //     
    //     Assert.Equal(3.5, rightFirstElseIfCondition.Value);
    //     Assert.Equal("N", rightFirstElseIfConditionUnitExpression.Name);
    //
    //     var firstElseIfFunctionCall = (FunctionCall) firstElseIfStatement.Statements[0];
    //     Assert.Equal("printCustom1", firstElseIfFunctionCall.Name);
    //
    //     var firstArgumentFirstElseIfFunctionCall = (Identifier) firstElseIfFunctionCall.Arguments[0];
    //     Assert.Equal("force",firstArgumentFirstElseIfFunctionCall.Name);
    //     
    //     var secondArgumentFirstElseIfFunctionCall = (FunctionCall) firstElseIfFunctionCall.Arguments[1];
    //
    //     var multExpressionFirstElseIfFunctionCall =
    //         (MultiplicateExpression) secondArgumentFirstElseIfFunctionCall.Arguments[0];
    //
    //     var leftMultExpressionFirstElseIfFunctionCall = (IntLiteral) multExpressionFirstElseIfFunctionCall.Left;
    //     var rightMultExpressionFirstElseIfFunctionCall = (Identifier) multExpressionFirstElseIfFunctionCall.Right!;
    //     
    //     Assert.Equal(2, leftMultExpressionFirstElseIfFunctionCall.Value);
    //     Assert.Null(leftMultExpressionFirstElseIfFunctionCall.UnitType);
    //     
    //     Assert.Equal("x",rightMultExpressionFirstElseIfFunctionCall.Name);
    //
    //     var secondElseIfCondition = (GreaterEqualThanExpression) secondElseIfStatement.Condition;
    //
    //     var leftSecondElseIfCondition = (Identifier) secondElseIfCondition.Left;
    //     Assert.Equal("force", leftSecondElseIfCondition.Name);
    //
    //     var rightSecondElseIfCondition = (IntLiteral) secondElseIfCondition.Right!;
    //     var rightSecondElseIfConditionUnitExpression = (UnitUnaryExpression) rightSecondElseIfCondition.UnitType!.Expression!;
    //     
    //     Assert.Equal(0, rightSecondElseIfCondition.Value);
    //     Assert.Equal("N", rightSecondElseIfConditionUnitExpression.Name);
    //
    //     var secondElseIfFirstFunctionCall = (FunctionCall) secondElseIfStatement.Statements[0];
    //     Assert.Equal("printCustom2", secondElseIfFirstFunctionCall.Name);
    //     var argumentSecondElseIfFirstFunctionCall = (Identifier) secondElseIfFirstFunctionCall.Arguments[0];
    //     Assert.Equal("force",argumentSecondElseIfFirstFunctionCall.Name);
    //     
    //     var secondElseIfSecondFunctionCall = (FunctionCall) secondElseIfStatement.Statements[1];
    //     Assert.Equal("print", secondElseIfSecondFunctionCall.Name);
    //     var argumentSecondElseIfSecondFunctionCall = (Identifier) secondElseIfSecondFunctionCall.Arguments[0];
    //     Assert.Equal("force",argumentSecondElseIfSecondFunctionCall.Name);
    //
    //     var elseStatement = (FunctionCall) ifStatement.ElseStatement[0];
    //     
    //     var argumentElseStatement = (StringLiteral) elseStatement.Arguments[0];
    //     Assert.Equal("Less than 0",argumentElseStatement.Value);
    // }
    //
    [Fact]
    [Trait("Category", "FunctionStatement")]
    public void TestFunctionStatement()
    {
        const string code = @"
                            calculateKEnergy(mass: [kg], speed: [m*s^-1], scalar: []) -> [J] {
                                return mass * speed * speed / scalar
                            }";
        
        var parser = PrepareParser(code);
        var program = parser.Parse();

        var functionStatement = program.Functions["calculateKEnergy"];

        var parameters = functionStatement.Parameters;
        var returnType = functionStatement.ReturnType;
        var block = functionStatement.Statements;
        
        parameters.Count.ShouldBe(3);
        var firstParameter = parameters[0];
        var secondParameter = parameters[1];
        var thirdParameter = parameters[2];

        var firstParameterExpected =
            new Parameter(
                "mass",
                new UnitType(
                    new UnitUnaryExpression(
                        "kg",
                        null
                    )
                )
            );

        var secondParameterExpected =
            new Parameter(
                "speed",
                new UnitType(
                    new UnitExpression(
                        new UnitUnaryExpression(
                            "m",
                            null
                        ),
                        new UnitUnaryExpression(
                            "s",
                            new UnitMinusPower(1)
                        )
                    )
                )
            );
        
        var thirdParameterExpected =
            new Parameter(
                "scalar",
                new UnitType(
                    null
                    )
            );

        var returnTypeExpected =
            new UnitType(
                new UnitUnaryExpression(
                    "J",
                    null
                )
            );

        block.Statements.Count.ShouldBe(1);
        var returnStatement = (ReturnStatement) block.Statements.First();
        var returnExpression = returnStatement.Expression;

        var returnExpressionExpected =
            new DivideExpression(
                new MultiplicateExpression(
                    new MultiplicateExpression(
                        new Identifier("mass"),
                        new Identifier("speed")
                    ),
                    new Identifier("speed")
                ),
                new Identifier("scalar")
            );

        JsonConvert.SerializeObject(firstParameter)
            .ShouldBe(JsonConvert.SerializeObject(firstParameterExpected));
        
        JsonConvert.SerializeObject(secondParameter)
            .ShouldBe(JsonConvert.SerializeObject(secondParameterExpected));
        
        JsonConvert.SerializeObject(thirdParameter)
            .ShouldBe(JsonConvert.SerializeObject(thirdParameterExpected));
        
        JsonConvert.SerializeObject(returnType)
            .ShouldBe(JsonConvert.SerializeObject(returnTypeExpected));
        
        JsonConvert.SerializeObject(returnExpression)
            .ShouldBe(JsonConvert.SerializeObject(returnExpressionExpected));
    }
    
    [Fact]
    [Trait("Category", "UnitDeclaration")]
    public void TestUnitDeclaration()
    {
        const string code = "unit N: [kg*m*s^-2]";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Units.Keys.ShouldHaveSingleItem();

        var unitExpression = program.Units["N"].Expression;

        var unitDeclaration =
            new UnitExpression(
                new UnitExpression(
                    new UnitUnaryExpression(
                        "kg",
                        null
                    ),
                    new UnitUnaryExpression(
                        "m",
                        null
                    )
                ),
                new UnitUnaryExpression(
                    "s",
                    new UnitMinusPower(2)
                )
            );

        JsonConvert.SerializeObject(unitExpression)
            .ShouldBe(JsonConvert.SerializeObject(unitDeclaration));
    }
    
    [Fact]
    [Trait("Category", "FunctionStatement")]
    [Trait("Category", "MultiStatement")]
    public void TestFunctionStatementsWithCalls()
    {
        const string code = @"
                            printCustomMessage(m: string) -> void {
                                print(""Custom: "")
                            }
                            printCustomMessage2(message: string) -> void {
                                print(""Custom2: "")
                            }
                            main() -> void {
                                let m: string = ""My message""
                                printCustomMessage(m)
                                printCustomMessage2(m)
                            }";
    
        var parser = PrepareParser(code);
        var program = parser.Parse();

        var functions = program.Functions;
        functions.Keys.Count.ShouldBe(3);
        
        var mainBlock = GetStatementsFromMain(program);
        mainBlock.Statements.Count.ShouldBe(3);

        var printCustomMessage = functions["printCustomMessage"];
        var printCustomMessage2 = functions["printCustomMessage2"];

        var printCustomMessageParameters = printCustomMessage.Parameters;
        var printCustomMessage2Parameters = printCustomMessage2.Parameters;

        var printCustomMessageBlock = printCustomMessage.Statements;
        var printCustomMessage2Block = printCustomMessage2.Statements;

        var printCustomMessageReturnType = printCustomMessage.ReturnType;
        var printCustomMessage2ReturnType = printCustomMessage2.ReturnType;

        printCustomMessageParameters.ShouldHaveSingleItem();
        printCustomMessage2Parameters.ShouldHaveSingleItem();
        
        printCustomMessageBlock.Statements.ShouldHaveSingleItem();
        printCustomMessage2Block.Statements.ShouldHaveSingleItem();

        var printCustomMessageParametersExpected =
            new List<Parameter>
            {
                new(
                    "m",
                    new StringType()
                )
            };
        
        var printCustomMessage2ParametersExpected =
            new List<Parameter>
            {
                new(
                    "message",
                    new StringType()
                )
            };

        var voidTypeExpected =
            new VoidType();

        var printCustomMessageBlockExpected =
            new Block(

                new List<IStatement>
                {
                    new FunctionCall(
                        "print",
                        new List<IExpression>
                        {
                            new StringLiteral("Custom: ")
                        }
                    )
                }
            );

        var printCustomMessage2BlockExpected =
            new Block(
                new List<IStatement>
                {
                    new FunctionCall(
                        "print",
                        new List<IExpression>
                        {
                            new StringLiteral("Custom2: ")
                        }
                    )
                }
            );

        var mainBlockExpected =
            new Block(
                new List<IStatement>
                {
                    new VariableDeclaration(
                        new Parameter(
                            "m",
                            new StringType()
                        ),
                        new StringLiteral("My message")
                    ),
                    new FunctionCall(
                        "printCustomMessage",
                        new List<IExpression>
                        {
                            new Identifier("m")
                        }
                    ),
                    new FunctionCall(
                        "printCustomMessage2",
                        new List<IExpression>
                        {
                            new Identifier("m")
                        }
                    )
                }
            );
        
        JsonConvert.SerializeObject(printCustomMessageParameters)
            .ShouldBe(JsonConvert.SerializeObject(printCustomMessageParametersExpected));
        
        JsonConvert.SerializeObject(printCustomMessage2Parameters)
            .ShouldBe(JsonConvert.SerializeObject(printCustomMessage2ParametersExpected));
        
        JsonConvert.SerializeObject(printCustomMessageReturnType)
            .ShouldBe(JsonConvert.SerializeObject(voidTypeExpected));
        
        JsonConvert.SerializeObject(printCustomMessage2ReturnType)
            .ShouldBe(JsonConvert.SerializeObject(voidTypeExpected));
        
        JsonConvert.SerializeObject(printCustomMessageBlock)
            .ShouldBe(JsonConvert.SerializeObject(printCustomMessageBlockExpected));

        JsonConvert.SerializeObject(printCustomMessage2Block)
            .ShouldBe(JsonConvert.SerializeObject(printCustomMessage2BlockExpected));
        
        JsonConvert.SerializeObject(mainBlock)
            .ShouldBe(JsonConvert.SerializeObject(mainBlockExpected));
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestLogicExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = firstVariable != (www && xyz || w == false && !ll || po)
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new BoolType()
                ),
                new NotEqualExpression(
                    new Identifier("firstVariable"),
                    new Expression(
                        new Expression(
                            new LogicFactor(
                                new Identifier("www"),
                                new Identifier("xyz")
                            ),
                            new LogicFactor(
                                new EqualExpression(
                                    new Identifier("w"),
                                    new BoolLiteral(false)
                                ),
                                new NotExpression(
                                    new Identifier("ll")
                                )
                            )
                        ),
                        new Identifier("po")
                    )
                )
            );
            
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestComparisonExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = x > 5 && x <= 40
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new BoolType()
                ),
                new LogicFactor(
                    new GreaterThanExpression(
                        new Identifier(
                            "x"
                        ),
                        new IntLiteral(
                            5,
                            null
                        )
                    ),
                    new SmallerEqualThanExpression(
                        new Identifier(
                            "x"
                        ),
                        new IntLiteral(
                            40,
                            null
                        )
                    )
                )
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestAdditiveAndMultiplicativeExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 2 + 3 * 4.2 - 2e1 / 8
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new UnitType(
                        null
                    )
                ),
                new SubtractExpression(
                    new AddExpression(
                        new IntLiteral(
                            2,
                            null
                        ),
                        new MultiplicateExpression(
                            new IntLiteral(
                                3,
                                null
                            ),
                            new FloatLiteral(
                                4.2,
                                null
                            )
                        )
                    ),
                    new DivideExpression(
                        new FloatLiteral(
                            2e1,
                            null
                        ),
                        new IntLiteral(
                            8,
                            null
                        )
                    )
                )
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestExpressionWithParentheses()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = (2 + 3) * 4
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new UnitType(
                        null
                    )
                ),
                new MultiplicateExpression(
                    new AddExpression(
                        new IntLiteral(
                            2,
                            null
                        ),
                        new IntLiteral(
                            3,
                            null
                        )
                    ),
                    new IntLiteral(
                        4,
                        null
                    )
                )
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestNegateExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = -5.5 < -2 && !isEqual != y
                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new BoolType()
                ),
                new LogicFactor(
                    new SmallerThanExpression(
                        new MinusExpression(
                            new FloatLiteral(
                                5.5,
                                null
                            )
                        ),
                        new MinusExpression(
                            new IntLiteral(
                                2,
                                null
                            )
                        )
                    ),
                    new NotEqualExpression(
                        new NotExpression(
                            new Identifier(
                                "isEqual"
                            )
                        ),
                        new Identifier(
                            "y"
                        )
                    )
                )
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    
    [Fact]
    [Trait("Category", "Expression")]
    public void TestExpressionWithFunctionCalls()
    {
        const string code = @"
                            main() -> void {
                                let secs: [s^2] = getFirstSec(x + 2, y) - getSecondSec()

                            }";

        var parser = PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "secs",
                    new UnitType(
                        new UnitUnaryExpression(
                            "s",
                            new UnitPower(2)
                        )
                    )
                ),
                new SubtractExpression(
                    new FunctionCall(
                        "getFirstSec",
                        new List<IExpression>
                        {
                            new AddExpression(
                                new Identifier("x"),
                                new IntLiteral(
                                    2,
                                    null
                                )
                            ),
                            new Identifier("y")
                        }
                    ),
                    new FunctionCall(
                        "getSecondSec",
                        new List<IExpression>()
                    )
                )
            );
        
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(variableDeclaration));
    }
    //
    // [Fact]
    // [Trait("Category", "Core")]
    // public void TestCoreFunctionalityWithCalculatingGForce()
    // {
    //     const string code = @"
    //                         unit N: [kg*m*s^-2]
    //                         unit G: [N*m^2*kg^-2]
    //                         
    //                         calculateGForce(m1: [kg], m2: [kg], distance: [m]) -> [N] {
    //                             let G: [G] = 6.6732e-11
    //                             return G * earthMass * sunMass / (earthSunDistance * earthSunDistance)
    //                         }
    //                         
    //                         let earthMass: [kg] = 5.9722e24
    //                         let sunMass: [kg] = 1.989e30
    //                         let earthSunDistance: [m] = 149.24e9
    //                         let gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)
    //                         
    //                         print(gForce)";
    //     
    //     var parser = PrepareParser(code);
    //     var program = parser.Parse();
    //     
    //     Assert.Equal(2, program.Units.Count);
    //     Assert.Equal(1, program.Functions.Values.Count);
    //     Assert.Equal(5, program.Statements.Count);
    //
    //     var nUnit = program.Units["N"];
    //     var gUnit = program.Units["G"];
    //
    //     var nUnitExpression = (UnitExpression) nUnit.Expression!;
    //
    //     var leftNUnitExpression = (UnitExpression) nUnitExpression.Left;
    //     var sN = (UnitUnaryExpression) nUnitExpression.Right!;
    //
    //     var kgN = (UnitUnaryExpression) leftNUnitExpression.Left;
    //     var mN = (UnitUnaryExpression) leftNUnitExpression.Right!;
    //
    //     Assert.Equal("kg", kgN.Name);
    //     Assert.Null(kgN.UnitPower);
    //     Assert.Equal("m", mN.Name);
    //     Assert.Null(mN.UnitPower);
    //     Assert.Equal("s", sN.Name);
    //
    //     var sNUnitPower = (UnitMinusPower) sN.UnitPower!;
    //     Assert.Equal(2, sNUnitPower.Value);
    //     
    //     var gUnitExpression = (UnitExpression) gUnit.Expression!;
    //
    //     var leftGUnitExpression = (UnitExpression) gUnitExpression.Left;
    //     var kgG = (UnitUnaryExpression) gUnitExpression.Right!;
    //
    //     var nG = (UnitUnaryExpression) leftGUnitExpression.Left;
    //     var mG = (UnitUnaryExpression) leftGUnitExpression.Right!;
    //
    //     Assert.Equal("N", nG.Name);
    //     Assert.Null(nG.UnitPower);
    //     
    //     Assert.Equal("m", mG.Name);
    //     var mGUnitPower = (UnitPower) mG.UnitPower!;
    //     Assert.Equal(2, mGUnitPower.Value);
    //     
    //     Assert.Equal("kg", kgG.Name);
    //
    //     var kgGUnitPower = (UnitMinusPower) kgG.UnitPower!;
    //     Assert.Equal(2, kgGUnitPower.Value);
    //
    //     var functionStatement = program.Functions["calculateGForce"];
    //     
    //     var firstParameter = functionStatement.Parameters[0];
    //     var secondParameter = functionStatement.Parameters[1];
    //     var thirdParameter = functionStatement.Parameters[2];
    //     
    //     Assert.Equal("m1",firstParameter.Name);
    //     var firstParameterType = (UnitType) firstParameter.Type;
    //     var firstParameterUnit = (UnitUnaryExpression) firstParameterType.Expression!;
    //     Assert.Equal("kg", firstParameterUnit.Name);
    //     Assert.Null(firstParameterUnit.UnitPower);
    //
    //     Assert.Equal("m2",secondParameter.Name);
    //     var secondParameterType = (UnitType) secondParameter.Type;
    //     var secondParameterUnit = (UnitUnaryExpression) secondParameterType.Expression!;
    //     Assert.Equal("kg", secondParameterUnit.Name);
    //     Assert.Null(secondParameterUnit.UnitPower);
    //     
    //     Assert.Equal("distance",thirdParameter.Name);
    //     var thirdParameterType = (UnitType) thirdParameter.Type;
    //     var thirdParameterUnit = (UnitUnaryExpression) thirdParameterType.Expression!;
    //     Assert.Equal("m", thirdParameterUnit.Name);
    //     Assert.Null(thirdParameterUnit.UnitPower);
    //
    //     var returnType = (UnitType) functionStatement.ReturnType;
    //     var returnUnit = (UnitUnaryExpression) returnType.Expression!;
    //     Assert.Equal("N", returnUnit.Name);
    //     Assert.Null(returnUnit.UnitPower);
    // }
    //

    private static Parser PrepareParser(string code)
    {
        var lexer = new Lexer(Helper.GetStreamReaderFromString(code));

        return new Parser(lexer);
    }

    private static Block GetStatementsFromMain(TopLevel topLevel)
    {
        return topLevel.Functions["main"].Statements;
    }
}
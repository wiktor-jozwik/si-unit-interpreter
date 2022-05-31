using Newtonsoft.Json;
using Shouldly;
using si_unit_interpreter.exceptions.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "mul",
                    new UnitType(
                        new List<Unit>()
                    )
                ),
                new IntLiteral(5, new UnitType(
                    new List<Unit>()
                )));

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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "mulFloat",
                    new UnitType(
                        new List<Unit>()
                    )
                ),
                new FloatLiteral(
                    5e2, new UnitType(
                        new List<Unit>()
                    )
                )
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
                                 let speed: [m*s^-2] = 5.23 [m*s^-2]
                             }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "speed",
                    new UnitType(
                        new List<Unit>
                        {
                            new("m"),
                            new("s", -2)
                        })
                ),
                new FloatLiteral(5.23, new UnitType(
                    new List<Unit>
                    {
                        new("m"),
                        new("s", -2)
                    })
                )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var returnStatement =
            new ReturnStatement(
                new AddExpression(
                    new Identifier(
                        "x"
                    ),
                    new IntLiteral(
                        5,
                        new UnitType(
                            new List<Unit>()
                        )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var whileStatement =
            new WhileStatement(
                new GreaterThanExpression(
                    new Identifier(
                        "y"
                    ),
                    new IntLiteral(
                        18,
                        new UnitType(
                            new List<Unit>()
                        )
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
                            new Identifier("y"),
                            new AddExpression(
                                new Identifier(
                                    "y"
                                ),
                                new IntLiteral(
                                    1,
                                    new UnitType(
                                        new List<Unit>()
                                    )
                                )
                            )
                        )
                    }
                )
            );
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(whileStatement));
    }

    [Fact]
    public void TestCodeWithComments()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 5
                                // let y: [] = 12
                                // x = x + 5
                                //x = x + 7
                                x = x + 3
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var mainBlock = Helper.GetStatementsFromMain(program);
        mainBlock.Statements.Count.ShouldBe(2);
    }

    [Fact]
    [Trait("Category", "IfStatement")]
    public void TestIfStatement()
    {
        const string code = @"
                            main() -> void {
                                if(force > 12 [N]) {
                                    print(force + f(1))
                                }
                                else if(g(force) < 3.5 [N]) {
                                    printCustom1(force, g(2 * x))
                                }
                                else if(force >= 0 [N]) {
                                    printCustom2(force)
                                    print(force)
                                }
                                else {
                                    print(""Less than 0"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var ifStatement =
            new IfStatement(
                new GreaterThanExpression(
                    new Identifier("force"),
                    new IntLiteral(
                        12,
                        new UnitType(new List<Unit>
                        {
                            new("N"),
                        })
                    )
                ),
                new Block(
                    new List<IStatement>
                    {
                        new FunctionCall(
                            "print",
                            new List<IExpression>
                            {
                                new AddExpression(
                                    new Identifier("force"),
                                    new FunctionCall(
                                        "f",
                                        new List<IExpression>
                                        {
                                            new IntLiteral(
                                                1,
                                                new UnitType(
                                                    new List<Unit>()
                                                )
                                            )
                                        }
                                    )
                                )
                            }
                        )
                    }
                ),
                new List<ElseIfStatement>
                {
                    new(
                        new SmallerThanExpression(
                            new FunctionCall(
                                "g",
                                new List<IExpression>
                                {
                                    new Identifier("force")
                                }
                            ),
                            new FloatLiteral(
                                3.5,
                                new UnitType(
                                    new List<Unit>
                                    {
                                        new("N"),
                                    })
                            )
                        ),
                        new Block(
                            new List<IStatement>
                            {
                                new FunctionCall(
                                    "printCustom1",
                                    new List<IExpression>
                                    {
                                        new Identifier("force"),
                                        new FunctionCall(
                                            "g",
                                            new List<IExpression>
                                            {
                                                new MultiplicateExpression(
                                                    new IntLiteral(
                                                        2,
                                                        new UnitType(
                                                            new List<Unit>()
                                                        )
                                                    ),
                                                    new Identifier("x")
                                                )
                                            }
                                        )
                                    }
                                )
                            }
                        )
                    ),
                    new(
                        new GreaterEqualThanExpression(
                            new Identifier("force"),
                            new IntLiteral(
                                0,
                                new UnitType(
                                    new List<Unit>
                                    {
                                        new("N"),
                                    })
                            )
                        ),
                        new Block(
                            new List<IStatement>
                            {
                                new FunctionCall(
                                    "printCustom2",
                                    new List<IExpression>
                                    {
                                        new Identifier("force")
                                    }
                                ),
                                new FunctionCall(
                                    "print",
                                    new List<IExpression>
                                    {
                                        new Identifier("force")
                                    }
                                )
                            }
                        )
                    )
                },
                new Block(
                    new List<IStatement>
                    {
                        new FunctionCall(
                            "print",
                            new List<IExpression>
                            {
                                new StringLiteral("Less than 0")
                            }
                        )
                    }
                )
            );
        JsonConvert.SerializeObject(block.Statements.First())
            .ShouldBe(JsonConvert.SerializeObject(ifStatement));
    }

    [Fact]
    [Trait("Category", "FunctionStatement")]
    public void TestFunctionStatement()
    {
        const string code = @"
                            calculateKEnergy(mass: [kg], speed: [m*s^-1], scalar: []) -> [J] {
                                return mass * speed * speed / scalar
                            }";

        var parser = Helper.PrepareParser(code);
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
                    new List<Unit>
                    {
                        new("kg"),
                    })
            );

        var secondParameterExpected =
            new Parameter(
                "speed",
                new UnitType(
                    new List<Unit>
                    {
                        new("m"),
                        new("s", -1)
                    })
            );

        var thirdParameterExpected =
            new Parameter(
                "scalar",
                new UnitType(
                    new List<Unit>()
                )
            );

        var returnTypeExpected =
            new UnitType(
                new List<Unit>
                {
                    new("J"),
                });

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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Units.Keys.ShouldHaveSingleItem();

        var units = program.Units["N"];

        var unitsExpected =
            new UnitType(
                new List<Unit>
                {
                    new("kg"),
                    new("m"),
                    new("s", -2)
                });

        JsonConvert.SerializeObject(units)
            .ShouldBe(JsonConvert.SerializeObject(unitsExpected));
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var functions = program.Functions;
        functions.Keys.Count.ShouldBe(3);

        var mainBlock = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),
                    new SmallerEqualThanExpression(
                        new Identifier(
                            "x"
                        ),
                        new IntLiteral(
                            40,
                            new UnitType(
                                new List<Unit>()
                            )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new UnitType(
                        new List<Unit>()
                    )
                ),
                new SubtractExpression(
                    new AddExpression(
                        new IntLiteral(
                            2,
                            new UnitType(
                                new List<Unit>()
                            )
                        ),
                        new MultiplicateExpression(
                            new IntLiteral(
                                3,
                                new UnitType(
                                    new List<Unit>()
                                )
                            ),
                            new FloatLiteral(
                                4.2,
                                new UnitType(
                                    new List<Unit>()
                                )
                            )
                        )
                    ),
                    new DivideExpression(
                        new FloatLiteral(
                            2e1,
                            new UnitType(
                                new List<Unit>()
                            )
                        ),
                        new IntLiteral(
                            8,
                            new UnitType(
                                new List<Unit>()
                            )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "x",
                    new UnitType(
                        new List<Unit>()
                    )
                ),
                new MultiplicateExpression(
                    new AddExpression(
                        new IntLiteral(
                            2,
                            new UnitType(
                                new List<Unit>()
                            )
                        ),
                        new IntLiteral(
                            3,
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),
                    new IntLiteral(
                        4,
                        new UnitType(
                            new List<Unit>()
                        )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
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
                                new UnitType(
                                    new List<Unit>()
                                )
                            )
                        ),
                        new MinusExpression(
                            new IntLiteral(
                                2,
                                new UnitType(
                                    new List<Unit>()
                                )
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

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.ShouldHaveSingleItem();
        var block = Helper.GetStatementsFromMain(program);
        block.Statements.ShouldHaveSingleItem();

        var variableDeclaration =
            new VariableDeclaration(
                new Parameter(
                    "secs",
                    new UnitType(
                        new List<Unit>
                        {
                            new("s", 2),
                        })
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
                                    new UnitType(
                                        new List<Unit>()
                                    )
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

    [Fact]
    [Trait("Category", "Core")]
    public void TestCoreFunctionalityWithCalculatingGForce()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            unit G: [N*m^2*kg^-2]
                            
                            calculateGForce(earthMass: [kg], sunMass: [kg], earthSunDistance: [m]) -> [N] {
                                let G: [G] = 6.6732e-11
                                return G * earthMass * sunMass / (earthSunDistance * earthSunDistance)
                            }
                            main() -> void {
                                let earthMass: [kg] = 5.9722e24
                                let sunMass: [kg] = 1.989e30
                                let earthSunDistance: [m] = 149.24e9
                                let gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)
                                
                                print(gForce)
                            }
                            ";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        program.Functions.Keys.Count.ShouldBe(2);
        program.Units.Keys.Count.ShouldBe(2);

        var block = Helper.GetStatementsFromMain(program);
        block.Statements.Count.ShouldBe(5);

        var nUnits = program.Units["N"];
        var nUnitsExpected =
            new UnitType(
                new List<Unit>
                {
                    new("kg"),
                    new("m"),
                    new("s", -2)
                });

        var gUnits = program.Units["G"];
        var gUnitsExpected =
            new UnitType(
                new List<Unit>
                {
                    new("N"),
                    new("m", 2),
                    new("kg", -2)
                });

        JsonConvert.SerializeObject(nUnits)
            .ShouldBe(JsonConvert.SerializeObject(nUnitsExpected));

        JsonConvert.SerializeObject(gUnits)
            .ShouldBe(JsonConvert.SerializeObject(gUnitsExpected));


        var calculateGForce = program.Functions["calculateGForce"];

        var parameters = calculateGForce.Parameters;
        var parametersExpected =
            new List<Parameter>
            {
                new(
                    "earthMass",
                    new UnitType(
                        new List<Unit>
                        {
                            new("kg")
                        })
                ),
                new(
                    "sunMass",
                    new UnitType(
                        new List<Unit>
                        {
                            new("kg")
                        })
                ),
                new(
                    "earthSunDistance",
                    new UnitType(
                        new List<Unit>
                        {
                            new("m")
                        })
                )
            };

        var returnType = calculateGForce.ReturnType;
        var returnTypeExpected =
            new UnitType(
                new List<Unit>
                {
                    new("N")
                });

        JsonConvert.SerializeObject(parameters)
            .ShouldBe(JsonConvert.SerializeObject(parametersExpected));

        JsonConvert.SerializeObject(returnType)
            .ShouldBe(JsonConvert.SerializeObject(returnTypeExpected));

        var statements = calculateGForce.Statements;

        var statementsExpected =
            new Block(
                new List<IStatement>
                {
                    new VariableDeclaration(
                        new Parameter(
                            "G",
                            new UnitType(
                                new List<Unit>
                                {
                                    new("G")
                                })
                        ),
                        new FloatLiteral(
                            6.673199999999999e-11,
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),
                    new ReturnStatement(
                        new DivideExpression(
                            new MultiplicateExpression(
                                new MultiplicateExpression(
                                    new Identifier("G"),
                                    new Identifier("earthMass")
                                ),
                                new Identifier("sunMass")
                            ),
                            new MultiplicateExpression(
                                new Identifier("earthSunDistance"),
                                new Identifier("earthSunDistance")
                            )
                        )
                    )
                }
            );

        JsonConvert.SerializeObject(statements)
            .ShouldBe(JsonConvert.SerializeObject(statementsExpected));

        var mainStatements = Helper.GetStatementsFromMain(program);

        var mainStatementsExpected =
            new Block(
                new List<IStatement>
                {
                    new VariableDeclaration(
                        new Parameter(
                            "earthMass",
                            new UnitType(
                                new List<Unit>
                                {
                                    new("kg")
                                })
                        ),
                        new FloatLiteral(
                            5.9722e24,
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),

                    new VariableDeclaration(
                        new Parameter(
                            "sunMass",
                            new UnitType(
                                new List<Unit>
                                {
                                    new("kg")
                                })
                        ),
                        new FloatLiteral(
                            1.989e30,
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),

                    new VariableDeclaration(
                        new Parameter(
                            "earthSunDistance",
                            new UnitType(
                                new List<Unit>
                                {
                                    new("m")
                                })
                        ),
                        new FloatLiteral(
                            149.24e9,
                            new UnitType(
                                new List<Unit>()
                            )
                        )
                    ),

                    new VariableDeclaration(
                        new Parameter(
                            "gForce",
                            new UnitType(
                                new List<Unit>
                                {
                                    new("N")
                                })
                        ),
                        new FunctionCall(
                            "calculateGForce",
                            new List<IExpression>
                            {
                                new Identifier("earthMass"),
                                new Identifier("sunMass"),
                                new Identifier("earthSunDistance")
                            }
                        )
                    ),
                    new FunctionCall(
                        "print",
                        new List<IExpression>
                        {
                            new Identifier("gForce")
                        }
                    )
                }
            );

        JsonConvert.SerializeObject(mainStatements)
            .ShouldBe(JsonConvert.SerializeObject(mainStatementsExpected));
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightLogicFactor()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = y ||
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_CURLY_BRACE on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = y &&
                                let
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received LET on row 4 and column 33", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfExpressionComparison()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = y <=
                                if
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received IF on row 4 and column 33", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfMultiplicativeExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = y /
                                if
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received IF on row 4 and column 33", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfAdditiveComparison()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = y -
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_CURLY_BRACE on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackRightParenthesesInExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = y - (w + 5]
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_PARENTHESES token" +
                     " but received RIGHT_SQUARE_BRACKET on row 3 and column 57", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackNumInIdentifierExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = !
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_CURLY_BRACE on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfLeftParenthesesInFunctionStatement()
    {
        const string code = @"
                            main) -> void {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 2 and column 33", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightParenthesesInFunctionStatement()
    {
        const string code = @"
                            main( -> void {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_PARENTHESES token" +
                     " but received RETURN_ARROW on row 2 and column 35", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfReturnArrowInFunctionStatement()
    {
        const string code = @"
                            main() > void {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RETURN_ARROW token" +
                     " but received GREATER_THAN_OPERATOR on row 2 and column 36", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfReturnTypeInFunctionStatement()
    {
        const string code = @"
                            main() -> {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected STRING_TYPE or BOOL_TYPE or VOID_TYPE or LEFT_SQUARE_BRACKET token" +
                     " but received LEFT_CURLY_BRACE on row 2 and column 39", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfStartingCurlyBraceInFunctionStatement()
    {
        const string code = @"
                            main() -> void
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_CURLY_BRACE token" +
                     " but received RIGHT_CURLY_BRACE on row 3 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestWrongParameterIdentifierInFunctionStatement()
    {
        const string code = @"
                            myFn(x: [], 5: []) -> void {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected IDENTIFIER token" +
                     " but received INT on row 2 and column 41", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfParameterIdentifierInFunctionStatement()
    {
        const string code = @"
                            myFn(x: [], ) -> void {
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected IDENTIFIER token" +
                     " but received RIGHT_PARENTHESES on row 2 and column 41", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightSquareBracketInUnitType()
    {
        const string code = @"
                            unit force: [N
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_SQUARE_BRACKET token" +
                     " but received ETX on row 3 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfIdentifierInUnitType()
    {
        const string code = @"
                            unit force: [N*]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected IDENTIFIER token" +
                     " but received RIGHT_SQUARE_BRACKET on row 2 and column 44", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackIntInUnitPower()
    {
        const string code = @"
                            unit force: [N*m^]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected INT token" +
                     " but received RIGHT_SQUARE_BRACKET on row 2 and column 46", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestStringInUnitPower()
    {
        const string code = @"
                            unit force: [N*m^s]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected INT token" +
                     " but received IDENTIFIER on row 2 and column 46", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackIntInMinusUnitPower()
    {
        const string code = @"
                            unit force: [N*m^-true]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected INT token" +
                     " but received TRUE on row 2 and column 47", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestStringInMinusUnitPower()
    {
        const string code = @"
                            unit force: [N*m^-s]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected INT token" +
                     " but received IDENTIFIER on row 2 and column 47", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightCurlyBraceInBlock()
    {
        const string code = @"
                            main() -> void {
                            print(a)
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_CURLY_BRACE token" +
                     " but received ETX on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfIdentifierInUnitDeclaration()
    {
        const string code = @"
                            unit 5: []
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected IDENTIFIER token" +
                     " but received INT on row 2 and column 34", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfColonInUnitDeclaration()
    {
        const string code = @"
                            unit x []
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected COLON token" +
                     " but received LEFT_SQUARE_BRACKET on row 2 and column 36", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfLeftSquareInUnitDeclaration()
    {
        const string code = @"
                            unit x: ]
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_SQUARE_BRACKET token" +
                     " but received RIGHT_SQUARE_BRACKET on row 2 and column 37", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfLeftParenthesesInFunctionCall()
    {
        const string code = @"
                            main() -> void {
                                myFn(x
                                mySecondFn()
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_PARENTHESES token" +
                     " but received IDENTIFIER on row 4 and column 33", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfExpressionInAssignment()
    {
        const string code = @"
                            main() -> void {
                                force = 
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_CURLY_BRACE on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfNextArgumentInFunctionCall()
    {
        const string code = @"
                            main() -> void {
                                f(x,)
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 3 and column 37", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfParameterInVariableDeclaration()
    {
        const string code = @"
                            main() -> void {
                               let = 5
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected IDENTIFIER token" +
                     " but received ASSIGNMENT_OPERATOR on row 3 and column 36", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfColonIbParameter()
    {
        const string code = @"
                            main() -> void {
                               let x [] = 5
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected COLON token" +
                     " but received LEFT_SQUARE_BRACKET on row 3 and column 38", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfAssignOperatorInVariableDeclaration()
    {
        const string code = @"
                            main() -> void {
                               let x: [] 5
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected ASSIGNMENT_OPERATOR token" +
                     " but received INT on row 3 and column 42", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfExpressionInVariableDeclaration()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 
                                let y: [] = 5
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received LET on row 4 and column 33", e.Message);
    }


    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfLeftParenthesesInIf()
    {
        const string code = @"
                            main() -> void {
                                if) {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 3 and column 35", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfConditionInIf()
    {
        const string code = @"
                            main() -> void {
                                if() {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 3 and column 36", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightParenthesesInIf()
    {
        const string code = @"
                            main() -> void {
                                if(x == true {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_PARENTHESES token" +
                     " but received LEFT_CURLY_BRACE on row 3 and column 46", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfStatementsInIf()
    {
        const string code = @"
                            main() -> void {
                                if(x == true)
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_CURLY_BRACE token" +
                     " but received RIGHT_CURLY_BRACE on row 4 and column 29", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfLeftParenthesesInWhile()
    {
        const string code = @"
                            main() -> void {
                                while) {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 3 and column 38", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfConditionInWhile()
    {
        const string code = @"
                            main() -> void {
                                while() {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected TRUE or FALSE or INT or FLOAT or STRING or IDENTIFIER or LEFT_PARENTHESES token" +
                     " but received RIGHT_PARENTHESES on row 3 and column 39", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestLackOfRightParenthesesInWhile()
    {
        const string code = @"
                            main() -> void {
                                while(x == true {
                                    print()
                                }
                            }
                            ";

        var parser = Helper.PrepareParser(code);

        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_PARENTHESES token" +
                     " but received LEFT_CURLY_BRACE on row 3 and column 49", e.Message);
    }

    [Fact]
    [Trait("Category", "Error")]
    public void TestUnit()
    {
        const string code = @"
                            unit v: [m/s]
                            ";

        var parser = Helper.PrepareParser(code);
        var e = Assert.Throws<ParserException>(() =>
            parser.Parse());
        Assert.Equal("Expected RIGHT_SQUARE_BRACKET token" +
                     " but received DIVISION_OPERATOR on row 2 and column 39", e.Message);
    }
}
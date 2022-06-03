using Shouldly;
using si_unit_interpreter.exceptions.interpreter;
using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using Xunit;

namespace si_unit_interpreter.spec;

public class InterpreterUnitTests
{
    [Fact]
    public void TestUnitVelocityExpression()
    {
        const string code = @"
                            main() -> void {
                                // s1 = 10
                                let s1: [m] = 4 [m] + 2 [m] * 3
                                // s2 = 1/2                                
                                let s2: [m] = (3 [m] - 1 [m]) / 4
                                // t = 30.4
                                let t: [s] = (5 [s] + 2 [s]) * 4 - 12 / 5 * (2 [s] - 3 [s])
                                // v = -0.3125
                                let v: [m*s^-1] = (s2 - s1) / t
                                print(v)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("-0.3125 [m*s^-1]\n");
    }

    [Fact]
    public void TestBigValueScalarExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 5.9722e24 * 1.989e30 / (149.24e9 * 149.24e9)
                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("5.333332321625854E+32\n");
    }
    
    [Fact]
    public void TestPrint()
    {
        const string code = @"
                            main() -> void {
                                let x: [m*s^-1] = 2 [m*s^-1]
                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("2 [m*s^-1]\n");
    }
    
    [Fact]
    public void TestNotPrintUnit()
    {
        const string code = @"
                            main() -> void {
                                let x: [m] = 2 [m]
                                let y: [s] = 5 [s]
                                print(x / y)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("0.4\n");
    }

    [Fact]
    public void TestOrExpression()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = true || false
                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("True\n");
    }

    [Fact]
    public void TestAndExpression()
    {
        const string code = @"
                            main() -> void {
                                let y: bool = true 
                                let z: bool = false
                                let x: bool = y && z

                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("False\n");
    }

    [Fact]
    public void TestCodeExecutionInIf()
    {
        const string code = @"
                            main() -> void {
                                if (5 > 2) {
                                    print(""from if"")
                                } 
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from if\n");
    }

    [Fact]
    public void TestCodeExecutionInIfWhenFalse()
    {
        const string code = @"
                            main() -> void {
                                if (5 < 2) {
                                    print(""from if"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("");
    }

    [Fact]
    public void TestCodeExecutionInElseIf()
    {
        const string code = @"
                            main() -> void {
                                let y: bool = false 
                                let z: bool = true
                                if (y) {
                                    print(""from if"")
                                } else if (z) {
                                    print(""from else if"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from else if\n");
    }


    [Fact]
    public void TestCodeExecutionInSecondElseIf()
    {
        const string code = @"
                            main() -> void {
                                if (2 [m] > 5 [m]) {
                                    print(""from if"")
                                } else if (3 < (-3 * 8)) {
                                    print(""from else if"")
                                } else if (""str"" == ""str"") {
                                    print(""from second else if"")
                                } else if (""str"" != ""str"") {
                                    print(""from third else if"")
                                } else {
                                    print(""from else"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from second else if\n");
    }

    [Fact]
    public void TestCodeExecutionInElse()
    {
        const string code = @"
                            main() -> void {
                                if (5 [K] <= (2 [K] - 7 [K])) {
                                    print(""from if"")
                                } else if (5 [kg] != 5 [kg]) {
                                    print(""from else if"")
                                } else {
                                    print(""from else"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from else\n");
    }

    [Fact]
    public void TestNotCondition()
    {
        const string code = @"
                            main() -> void {
                                let a: [m] = 5 [m]
                                let b: [m] = 12.5 [m]
                                let bbb: bool = a > b
                                if (!bbb) {
                                    print(""not bool"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("not bool\n");
    }

    [Fact]
    public void TestCodeExecutionInWhile()
    {
        const string code = @"
                            main() -> void {
                                let i: [] = 10
                                while(i > 0) {
                                    print(i)
                                    i = i - 1
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("10\n9\n8\n7\n6\n5\n4\n3\n2\n1\n");
    }

    [Fact]
    public void TestCodeExecutionWithFunction()
    {
        const string code = @"
                            getAcetylocholinoesterazaValue() -> [mol] {
                                return 5 [mol] * 12
                            }

                            main() -> void {
                                let x: [mol] = getAcetylocholinoesterazaValue() / 6
                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("10 [mol]\n");
    }

    [Fact]
    public void TestCodeExecutionWithFunctionAndParameters()
    {
        const string code = @"
                            getAcetylocholinoesterazaValue(x: [mol]) -> [mol] {
                                return x * 12e23
                            }

                            main() -> void {
                                let acetylo: [mol] = 43e-23 [mol]
                                let value: [] = getAcetylocholinoesterazaValue(acetylo) / 7 [mol]
                                print(value)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("73.7142857142857\n");
    }

    [Fact]
    public void TestCallFunctionWithParameter()
    {
        const string code = @"
                            printValue(x: [m]) -> void {
                                print(x)
                            }

                            main() -> void {
                                let distance: [m] = 2.1e2 [m]
                                printValue(distance)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("210 [m]\n");
    }

    [Fact]
    public void TestIfInFunctionWithParameterWhenConditionIsTrue()
    {
        const string code = @"
                            printMeters(x: [m]) -> void {
                                if (x > 10 [m]) {
                                    print(""x is more than 10 meters"")
                                    print(x)
                                }
                            }

                            main() -> void {
                                let distance: [m] = 10.01 [m]
                                printMeters(distance)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("x is more than 10 meters\n10.01 [m]\n");
    }

    [Fact]
    public void TestIfInFunctionWithParameterWhenConditionIsFalse()
    {
        const string code = @"
                            printMeters(x: [m]) -> void {
                                if (x >= 10 [m]) {
                                    print(""x is more or equal than 10 meters"")
                                    print(x)
                                } else {
                                    print(""x is less than 10 meters"")
                                    print(x)
                                }
                            }

                            main() -> void {
                                let distance: [m] = 9.99 [m]
                                printMeters(distance)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("x is less than 10 meters\n9.99 [m]\n");
    }

    [Fact]
    public void TestWhileInFunctionWhichTakeVariableFromParameter()
    {
        const string code = @"
                            countTo(to: []) -> void {
                                let x: [] = 0
                                while (x <= to) {
                                    print(x)
                                    x = x + 1
                                }
                            }

                            main() -> void {
                                countTo(15)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n");
    }
    
    
    [Fact]
    public void TestWhileInFunctionWhichTakeVariableFromParameter2()
    {
        const string code = @"
                            countFrom(from: []) -> void {
                                let x: [] = 0
                                if (true) {
                                    while (x <= from) {
                                        print(from)
                                        from = from - 1
                                    }
                                }
                            }

                            main() -> void {
                                countFrom(15)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n");
    }

    [Fact]
    public void TestRecursiveFunction()
    {
        const string code = @"
                            fibonacci(n: []) -> [] {
                                if (n <= 1) {
                                    return n
                                } else {
                                    return fibonacci(n-1) + fibonacci(n-2)
                                }
                            }

                            main() -> void {
                                let fibonacciValue: [] = fibonacci(12)
                                print(fibonacciValue)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("144\n");
    }

    [Fact]
    public void TestPassingVariableByCopyToFunction()
    {
        const string code = @"
                            myFn(mass: [kg]) -> void {
                                mass = mass + 10 [kg]
                                print(mass)
                            }

                            main() -> void {
                                let m: [kg] = 50 [kg]
                                myFn(m)
                                print(m)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("60 [kg]\n50 [kg]\n");
    }

    [Fact]
    public void TestRunningFunctionFromFunction()
    {
        const string code = @"
                            getX(var: [K]) -> [K] {
                                return 20 [K] - var
                            }
                            getY(w: [K]) -> [K] {
                                let z: [K] = 270 [K] + w
                                return getX(z)
                            }
                            main() -> void {
                                let x: [K] = getY(25 [K])

                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("-275 [K]\n");
    }

    [Fact]
    public void TestDifferentFunctionsOrder()
    {
        const string code = @"
                            getX(var: [s]) -> [s] {
                                return 20 [s] - var
                            }
                            main() -> void {
                                let x: [s] = getY(25 [s])

                                print(x)
                            }
                            getY(w: [s]) -> [s] {
                                return getX(w)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("-5 [s]\n");
    }

    [Fact]
    public void TestInstructionsAfterReturn()
    {
        const string code = @"
                            getX(var: [s]) -> [s] {
                                return 20 [s] - var
                                print(var) // should not be triggered and getX returned 20 - var
                            }
                            main() -> void {
                                let x: [s] = getX(-40 [s])

                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("60 [s]\n");
    }

    [Fact]
    public void TestInstructionsAfterReturnInIfs()
    {
        const string code = @"
                            main() -> void {
                                let x: [kg] = 20 [kg]
                                let y: [kg] = 18.5 [kg]
                                if (x != y) {
                                    print(""x != y"")
                                    return
                                } else {
                                    print(""x == y"")
                                }
                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("x != y\n");
    }

    [Fact]
    public void TestLackOfReturn()
    {
        const string code = @"
                            getX(var: [s]) -> [s] {
                                print(var)
                            }
                            main() -> void {
                                let x: [s] = getX(25 [s])

                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        Assert.Throws<LackOfValidReturnException>(() => interpreter.Visit(program));
    }

    [Fact]
    public void TestReturnInWhileFromVoidFunction()
    {
        const string code = @"
                            main() -> void {
                                let x: [kg] = 16 [kg]
                                let y: [kg] = 10 [kg]
                                while (x >= 0 [kg]) {
                                    if (x - y <= 0 [kg]) {
                                        print(""returning"")
                                        return
                                    } 
                                    x = x - 1 [kg]
                                    print(x)
                                }

                                print(y)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("15 [kg]\n14 [kg]\n13 [kg]\n12 [kg]\n11 [kg]\n10 [kg]\nreturning\n");
    }

    [Fact]
    public void TestReturnInWhileFromUnitFunction()
    {
        const string code = @"
                            getX() -> [s] {
                                let x: [s] = 14 [s]
                                let y: [s] = 10 [s]
                                while (x >= 0 [s]) {
                                    if (x - y <= 0 [s]) {
                                        print(""returning"")
                                        return x
                                    } 
                                    x = x - 1 [s]
                                    print(x)
                                }

                                print(y)
                            }
                            main() -> void {
                                let x: [s] = getX()

                                print(x)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("13 [s]\n12 [s]\n11 [s]\n10 [s]\nreturning\n10 [s]\n");
    }

    [Fact]
    public void TestReturnInMiddleElseIf()
    {
        const string code = @"
                            main() -> void {
                                if (2 [m] > 5 [m]) {
                                    print(""from if"")
                                } else if (3 < (-3 * 8)) {
                                    print(""from else if"")
                                } else if (""str"" != ""str"") {
                                    print(""from second else if"")
                                } else if (""str"" == ""str"") {
                                    print(""from third else if"")
                                    print(""returning"")
                                    return
                                } else {
                                    print(""from else"")
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from third else if\nreturning\n");
    }

    [Fact]
    public void TestReturnFromElse()
    {
        const string code = @"
                            main() -> void {
                                if (2 [m] > 5 [m]) {
                                    print(""from if"")
                                } else {
                                    print(""from else"")
                                    return
                                }
                                print(""should not be printed"")
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from else\n");
    }


    [Fact]
    public void TestElseWithoutReturn()
    {
        const string code = @"
                            main() -> void {
                                if (2 [m] > 5 [m]) {
                                    print(""from if"")
                                } else {
                                    print(""from else"")
                                }
                                print(""should be printed"")
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        using var consoleOutput = new ConsoleOutput();
        semanticAnalyzer.Visit(program);
        interpreter.Visit(program);
        consoleOutput.GetOutput().ShouldBe("from else\nshould be printed\n");
    }

    [Fact]
    public void TestLackOfMainFunction()
    {
        const string code = @"
                            getX(var: [s]) -> [s] {
                                return 20 [s] - var
                            }
                            ";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);

        Assert.Throws<LackOfMainFunctionException>(() => interpreter.Visit(program));
    }

    [Fact]
    public void TestWhileInf()
    {
        const string code = @"
                            main() -> void {
                                while(6 >= 3) {
                                    print(5)
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);

        Assert.Throws<MaxNumberIterationReachedException>(() => interpreter.Visit(program));
    }
    
    [Fact]
    public void TestDivideBy0()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 5 / 0
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);

        Assert.Throws<ZeroDivisionError>(() => interpreter.Visit(program));
    }
    
    [Fact]
    public void TestDivideBy0FromExpression()
    {
        const string code = @"
                            getZ() -> [] {
                                return 5 - 5
                            }
                            main() -> void {
                                let y: [] = 10
                                let z: [] = getZ()
                                let x: [] = y / z
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();


        var builtinFunctionsProvider = new BuiltInFunctionsProvider();

        var interpreter = new InterpreterVisitor("main", builtinFunctionsProvider);
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);

        Assert.Throws<ZeroDivisionError>(() => interpreter.Visit(program));
    }
}
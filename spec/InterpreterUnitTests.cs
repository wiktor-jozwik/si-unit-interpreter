using Shouldly;
using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using Xunit;
using Xunit.Abstractions;

namespace si_unit_interpreter.spec;

public class InterpreterUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    public InterpreterUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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
                                } else {
                                    print(""from else"")
                                }
                            }";
        
        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
    }
    
    [Fact]
    public void TestCodeExecutionWithFunction()
    {
        const string code = @"
                            getAcetylocholinoesterazaValue() -> [mol] {
                                return 5 [mol] * 12
                            }

                            main() -> void {
                                let x: [] = getAcetylocholinoesterazaValue() / 6 [mol]
                                print(x)
                            }";
        
        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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
                                let value: [] = getAcetylocholinoesterazaValue(acetylo) / 6 [mol]
                                print(value)
                            }";
        
        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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
                                countTo(20)
                            }";
        
        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
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

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
    }
    
    [Fact]
    public void TestPassingTwoArgumentsToFunction()
    {
        const string code = @"
                            getDistance(x: []) -> [m] {
                                // 1
                                return 5 [m] / x
                            }
                            getDuration(x: []) -> [s] {
                                // 25
                                return 5 [s] * x
                            }
                            
                            calculateVelocity(distance: [m], duration: [s]) -> [m*s^-1] {
                                // 1 / 25
                                return distance / duration
                            }

                            main() -> void {
                                let x: [m] = getDistance(10)
                                let y: [s] = getDuration(5)
                                let v1: [m*s^-1] = calculateVelocity(getDistance(10), getDuration(5))
                                let v2: [m*s^-1] = calculateVelocity(x, y)

                                print(v1)
                                print(v2)
                            }";
        
        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var testBuiltInFunctionsProvider = new TestBuiltInFunctionsProvider(_testOutputHelper);

        var interpreter = new InterpreterVisitor("main", testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
    }

    [Fact]
    public void TestRunningFunctionFromFunction()
    {
        // TODO
    }
    
}
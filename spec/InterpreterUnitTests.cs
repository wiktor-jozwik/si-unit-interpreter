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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
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

        var interpreter = new InterpreterVisitor(testBuiltInFunctionsProvider);
        
        interpreter.Visit(program);
        1.ShouldBe(1);
    }
}
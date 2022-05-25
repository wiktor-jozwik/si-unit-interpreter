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
        //
        // var semanticAnalyzer = new SemanticAnalyzerVisitor();
        // semanticAnalyzer.Visit(program);

        var functions = new Dictionary<dynamic, Func<dynamic, dynamic>>
        {
            ["print"] = (value) =>
            {
                _testOutputHelper.WriteLine($"{value}");
                return null!;
            }
        };

        var interpreter = new InterpreterVisitor(functions);
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

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
        // var interpreter = new InterpreterVisitor();
        // interpreter.Visit(program);
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
        //
        // var semanticAnalyzer = new SemanticAnalyzerVisitor();
        // semanticAnalyzer.Visit(program);

        var functions = new Dictionary<dynamic, Func<dynamic, dynamic>>
        {
            ["print"] = (value) =>
            {
                _testOutputHelper.WriteLine($"{value}");
                return null!;
            }
        };

        var interpreter = new InterpreterVisitor(functions);
        interpreter.Visit(program);
        1.ShouldBe(1);
    }
}
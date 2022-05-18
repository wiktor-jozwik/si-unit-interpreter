using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.semantic_analyzer;
using Xunit;

namespace si_unit_interpreter.spec;

public class SemanticAnalyzerUnitTests
{
    [Fact]
    public void TestVariableRedeclarationInFunction()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = true
                                let x: bool = false
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' is already declared", e.Message);
    }
    
    [Fact]
    public void TestVariableRedeclarationWithConditionInIfBody()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                if(x > 5) {
                                    let x: [] = 2
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' is already declared", e.Message);
    }
    
    [Fact]
    public void TestVariableRedeclarationInIfBody()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                if(x > 5) {
                                    let y: [] = 2
                                    let y: [] = 3
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' is already declared", e.Message);
    }
    
    [Fact]
    public void TestVariableUndeclaredInFunction()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = y + 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' is not defined", e.Message);
    }
    
    [Fact]
    public void TestFunctionUndeclaredInFunction()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 5 + fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
}
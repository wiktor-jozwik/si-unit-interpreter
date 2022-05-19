using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.semantic_analyzer;
using Xunit;

namespace si_unit_interpreter.spec;

public class SemanticAnalyzerUnitTests
{
    [Fact]
    [Trait("Category", "Invalid")]
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
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInIf()
    {
        const string code = @"
                            main() -> void {
                                let w: [] = 3
                                if(w > 5) {
                                    let w: [] = 2
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'w' is already declared", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInElseIf()
    {
        const string code = @"
                            main() -> void {
                                let o: [] = 3
                                if(o > 5) {
                                    let y: [] = 2
                                } else if(o < 3) {
                                    let o: [] = 5
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'o' is already declared", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInElse()
    {
        const string code = @"
                            main() -> void {
                                let e: [] = 3
                                if(e > 5) {
                                    let y: [] = 2
                                } else if(e < 3) {
                                    let o: [] = 5
                                } else {
                                    let e: [] = 8
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'e' is already declared", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
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
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInNestedIf()
    {
        const string code = @"
                            main() -> void {
                                let nested: [] = 3
                                if(nested > 5) {
                                    if (nested < 14) {
                                        let nested: [m] = 18
                                    }
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'nested' is already declared", e.Message);
    }
    
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInWhile()
    {
        const string code = @"
                            main() -> void {
                                let w: [] = 3
                                while(w > 5) {
                                    let w: [] = 2
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'w' is already declared", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInWhileBody()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                while(x > 5) {
                                    let z: [] = 2
                                    let z: [] = 3
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'z' is already declared", e.Message);
    }
    
        
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableRedeclarationInNestedWhile()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                while(x > 5) {
                                    let y: [] = -2
                                    while(y < 2) {
                                        let z: [] = 2
                                        let z: [] = 3
                                    }
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'z' is already declared", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestSameVariablesInFewFunctions()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                }
                            fun() -> void {
                                let x: [m] = 8
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestSameVariablesInFewElseIfs()
    {
        const string code = @"
                            main() -> void {
                                let e: [] = 3
                                if(e > 5) {
                                    let same: [] = 2
                                } else if(e < 3) {
                                    let same: [] = 5
                                } else {
                                    let same: [] = 8
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
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
    [Trait("Category", "Invalid")]
    public void TestVariableUndeclaredInIfCondition()
    {
        const string code = @"
                            main() -> void {
                                if(i > 5) {
                                    let y: [] = 5
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'i' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestVariableUndeclaredInWhileCondition()
    {
        const string code = @"
                            main() -> void {
                                while(x > 5) {
                                    let y: [] = 5
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
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
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationErrorStringPlusInt()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = ""my string"" + 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationErrorStringTimesString()
    {
        const string code = @"
                            main() -> void {
                                let firstString = ""sss""
                                let secondString = ""www""
                                let x: string = firstString * secondString
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationErrorStringDivideString()
    {
        const string code = @"
                            main() -> void {
                                let firstString = ""sss""
                                let secondString = ""www""
                                let x: string = firstString / secondString
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationErrorStringMinusString()
    {
        const string code = @"
                            main() -> void {
                                let firstString = ""sss""
                                let secondString = ""www""
                                let x: string = firstString - secondString
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationErrorStringMinusStringFromFun()
    {
        const string code = @"
                            fun() -> string {
                                return ""string""
                            }
                            main() -> void {
                                let firstString: string = ""sss""
                                let x: string = firstString - fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationIntAndBool()
    {
        const string code = @"
                            fun() -> bool {
                                return true
                            }
                            main() -> void {
                                let firstInt = 5
                                let x: [] = firstInt - fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        
        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestOperationMultiplicationAndDivisionOfUnits()
    {
        const string code = @"
                            main() -> void {
                                let s: [m] = 5
                                let t: [s] = 10
                                let x: [m*s^-1] = 3
                                let v: [m^2*s^-3] = s / t * x
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        var e = Assert.Throws<UnitNotPermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("[m^2*s^-2] type does not match required [m^2*s^-3] type", e.Message);
    }
    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestOperationMultiplicationAndDivisionOfUnitsValid()
    {
        const string code = @"
                            main() -> void {
                                let s: [m] = 5
                                let t: [s] = 10
                                let x: [m*s^-1] = 3
                                let v: [m^2*s^-2] = s / t * x
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    [Trait("Category", "Invalid")]
    public void TestAddTwoUnitsInValid()
    {
        const string code = @"
                            main() -> void {
                                let length: [m] = 5
                                let duration: [s] = 10
                                let speed: [s] = length + duration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
        // error
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestSubtractTwoUnitsInValid()
    {
        const string code = @"
                            main() -> void {
                                let length: [m] = 5
                                let duration: [s] = 10
                                let speed: [s] = length - duration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
        // error
    }
    
        
    [Fact]
    [Trait("Category", "Valid")]
    public void TestAddAndSubtractTwoUnitsValid()
    {
        const string code = @"
                            main() -> void {
                                let s1: [m] = 5
                                let s2: [m] = 10
                                let sAdd: [m] = s1 + s2
                                let sSubtract: [m] = s1 - s2
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestUnitOperationWithUnitNotDefined()
    {
        const string code = @"
                            main() -> void {
                                let length: [N] = 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
        // error
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestUnitOperationWithScalarOnLeftInvalid()
    {
        const string code = @"
                            main() -> void {
                                let length: [] = 5 [m] / 5 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
        // error
    }
    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestUnitOperationWithScalarOnLeftValid()
    {
        const string code = @"
                            main() -> void {
                                let scalar: [] = 5 [m] / 5 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    [Trait("Category", "Invalid")]
    public void TestUnitOperationWhichLeadToScalarOnRightSideInvalid()
    {
        const string code = @"
                            main() -> void {
                                let length: [m] = 5 [m] / 5 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        var e = Assert.Throws<UnitNotPermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("[] type does not match required [m] type", e.Message);
    }

    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestCoreUnitOperations()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let mass: [kg] = 1000
                                let v: [m*s^-1] = 50
                                let v0: [m*s^-1] = 20
                                let t: [s] = 40

                                let acceleration: [m*s^-2] = (v - v0) / t

                                let force: [N] = mass * acceleration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    [Trait("Category", "Valid")]
    public void TestCoreUnitOperationsWithErrors()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let mass: [kg] = 1000
                                let v: [m*s^-1] = 50
                                let v0: [m*s^-1] = 20
                                let t: [s] = 40
                                
                                // should be m*s^-2 here
                                let acceleration: [m*s^-1] = (v - v0) / t

                                let force: [N] = mass * acceleration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var semanticAnalyzer = new SemanticAnalyzerVisitor();
        semanticAnalyzer.Visit(program);
    }
}
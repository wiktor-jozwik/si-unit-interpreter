using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' is already declared", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'w' is already declared", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'o' is already declared", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'e' is already declared", e.Message);
    }

    [Fact]
    public void TestVariableRedeclarationInIfBody()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3
                                if(x >= 5) {
                                    let y: [] = 2
                                    let y: [] = 3
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' is already declared", e.Message);
    }

    [Fact]
    public void TestVariableRedeclarationInNestedIf()
    {
        const string code = @"
                            main() -> void {
                                let nested: [] = 3
                                if(nested <= 5) {
                                    if (nested < 14) {
                                        let nested: [m] = 18
                                    }
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'nested' is already declared", e.Message);
    }


    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'w' is already declared", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'z' is already declared", e.Message);
    }


    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'z' is already declared", e.Message);
    }

    [Fact]
    public void TestSameVariablesInFewFunctions()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 3 []
                                }
                            fun() -> void {
                                let x: [m] = 8 [m]
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestRedeclareSameVariableAsParameter()
    {
        const string code = @"
                            fun(zzz: []) -> void {
                                let zzz: [m] = 8 [m]
                                return zzz + 5
                            }
                            main() -> void {
                                let x: [] = fun(5)
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'zzz' is already declared", e.Message);
    }
    
    [Fact]
    public void TestTwoSameParameters()
    {
        const string code = @"
                            fun(zzz: [], zzz: [s]) -> void {
                                return zzz + 5 [s]
                            }
                            main() -> void {
                                let x: [] = fun(5, 5 [s])
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotUniqueParametersNamesException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' got two or more 'zzz' parameters", e.Message);
    }

    [Fact]
    public void TestUsageOfVariableInWrongScope()
    {
        const string code = @"
                            fun() -> [m] {
                                return zzz + 5 [m]
                            }
                            main() -> void {
                                let zzz: [m] = 8 [m]
                                fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'zzz' is not defined", e.Message);
    }

    [Fact]
    public void TestRedeclareSameVariableAsParameterInIf()
    {
        const string code = @"
                            fun(var: []) -> void {
                                if (var > 5) {
                                    let var: [] = 8
                                }
                                return var + 5
                            }
                            main() -> void {
                                let x: [] = fun(5)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'var' is already declared", e.Message);
    }

    [Fact]
    public void TestRedeclareSameVariableAsParameterInElseIf()
    {
        const string code = @"
                            fun(varElseIf: []) -> void {
                                if (varElseIf > 5) {
                                } else if(varElseIf < 2) {
                                    let varElseIf: [] = 8
                                }
                                return varElseIf + 5
                            }
                            main() -> void {
                                let x: [] = fun(5)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'varElseIf' is already declared", e.Message);
    }

    [Fact]
    public void TestRedeclareSameVariableAsParameterInElse()
    {
        const string code = @"
                            fun(varElse: []) -> void {
                                if (varElse > 5) {
                                }
                                else {
                                    let varElse: [] = 8
                                }
                                return varElseIf + 5
                            }
                            main() -> void {
                                let x: [] = fun(5)
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<VariableRedeclarationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'varElse' is already declared", e.Message);
    }

    [Fact]
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
                                    let same: [m] = 8 [m]
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' is not defined", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'i' is not defined", e.Message);
    }
    
    [Fact]
    public void TestTryingToAccessVariableDeclaredInIf()
    {
        const string code = @"
                            main() -> void {
                                if(2 > 5) {
                                    let y: [] = 5
                                }
                                y = 10
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' is not defined", e.Message);
    }

    [Fact]
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<VariableUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' is not defined", e.Message);
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

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<FunctionUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'fun' is not defined", e.Message);
    }

    [Fact]
    public void TestInvalidVariableDeclarationWithUnitMismatch()
    {
        const string code = @"
                            main() -> void {
                                let s: [m] = 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'s' requires [m] type but received []", e.Message);
    }

    [Fact]
    public void TestOperationMultiplicationAndDivisionOfUnits()
    {
        const string code = @"
                            main() -> void {
                                let s: [m] = 5 [m]
                                let t: [s] = 10 [s]
                                let x: [m*s^-1] = 3 [m*s^-1]
                                let z: [m^2*s^-3] = s / t * x
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'z' requires [m^2*s^-3] type but received [m^2*s^-2]", e.Message);
    }

    [Fact]
    public void TestOperationMultiplicationAndDivisionOfUnitsValid()
    {
        const string code = @"
                            main() -> void {
                                let s: [m] = 5 [m]
                                let t: [s] = 10 [s]
                                let x: [m*s^-1] = 3 [m*s^-1]
                                let v: [m^2*s^-2] = s / t * x
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestAddTwoUnitsInvalid()
    {
        const string code = @"
                            main() -> void {
                                let length: [m] = 5 [m]
                                let duration: [s] = 10 [s]
                                let speed: [s] = length + duration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '+' for [m] and [s]", e.Message);
    }

    [Fact]
    public void TestAddTwoUnitsInlineInvalid()
    {
        const string code = @"
                            main() -> void {
                                let speed: [s] = 5 [kg] - 3 [m^2*s^-2]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for [kg] and [m^2*s^-2]", e.Message);
    }

    [Fact]
    public void TestSubtractTwoUnitsInvalid()
    {
        const string code = @"
                            main() -> void {
                                let length: [m] = 5 [m]
                                let duration: [s] = 10 [s]
                                let speed: [s] = duration - length
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for [s] and [m]", e.Message);
    }


    [Fact]
    public void TestAddAndSubtractTwoUnitsValid()
    {
        const string code = @"
                            main() -> void {
                                let s1: [m] = 5 [m]
                                let s2: [m] = 10 [m]
                                let sAdd: [m] = s1 + s2
                                let sSubtract: [m] = s1 - s2
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitOperationWithUnitNotDefined()
    {
        const string code = @"
                            main() -> void {
                                let length: [N] = 5 [N]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnitUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'N' is not defined", e.Message);
    }

    [Fact]
    public void TestSiUnitWithTypo()
    {
        const string code = @"
                            main() -> void {
                                let length: [k] = 5 [k]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnitUndeclaredException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'k' is not defined", e.Message);
    }

    [Fact]
    public void TestSiUnitWithoutTypo()
    {
        const string code = @"
                            main() -> void {
                                let length: [K] = 5 [K]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }


    [Fact]
    public void TestUnitOperationWithScalarOnLeftInvalid()
    {
        const string code = @"
                            main() -> void {
                                let scalar: [] = 5 [m] / 5 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'scalar' requires [] type but received [m*s^-1]", e.Message);
    }

    [Fact]
    public void TestUnitOperationWithScalarOnLeftValid()
    {
        const string code = @"
                            main() -> void {
                                let scalar: [] = 5 [m] / 5 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitOperationWhichLeadToScalarOnRightSideInvalid()
    {
        const string code = @"
                            main() -> void {
                                let l: [cd] = 5 [mol] / 5 [mol]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'l' requires [cd] type but received []", e.Message);
    }


    [Fact]
    public void TestCoreUnitOperations()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let mass: [kg] = 1000 [kg]
                                let v: [m*s^-1] = 50 [m*s^-1]
                                let v0: [m*s^-1] = 20 [m*s^-1]
                                let t: [s] = 40 [s]

                                let acceleration: [m*s^-2] = (v - v0) / t

                                let force: [N] = mass * acceleration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestCoreUnitOperationsWithErrors()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let mass: [kg] = 1000 [kg]
                                let v: [m*s^-1] = 50 [m*s^-1]
                                let v0: [m*s^-1] = 20 [m*s^-1]
                                let t: [s] = 40 [s]
                                
                                // should be m*s^-2 here
                                let acceleration: [m*s^-1] = (v - v0) / t

                                let force: [N] = mass * acceleration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'acceleration' requires [m*s^-1] type but received [m*s^-2]", e.Message);
    }

    [Fact]
    public void TestCoreUnitOperationsWithErrors2()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let mass: [kg] = 1000 [kg]
                                let v: [m*s^-1] = 50 [m*s^-1]
                                let v0: [m*s^-1] = 20 [m*s^-1]
                                let t: [s] = 40 [s]
                                
                                let acceleration: [m*s^-2] = (v - v0) / t

                                let force: [N] = acceleration
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'force' requires [N] type but received [m*s^-2]", e.Message);
    }

    [Fact]
    public void TestCoreUnitOperationsWithInlineDeclarations()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let force: [N] = 1000 [kg] * (50.5 [m*s^-1] - 20.4 [m*s^-1]) / 40.2 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    public void TestUnitEvaluationReturnFromFunction()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            calculateGForce(m1: [kg], m2: [kg], distance: [m]) -> [N] {
                                return 6.6732e-11 [N*m^2*kg^-2] * m1 * m2 / (distance * distance)
                            }
                            main() -> void {
                                let gForce: [N] = calculateGForce(200 [kg], 4500 [kg], 8000 [m])
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }
    
    [Fact]
    public void TestBigUnits()
    {
        const string code = @"
                            unit R: [kg*m^2*s^-3*A^-2]
                            unit V: [kg*m^2*A^-1*s^-3]

                            calculateElectricity(resistance: [R], voltage: [V]) -> [A] {
                                return voltage / resistance
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestCoreUnitOperationsWithInlineDeclarationsWithError()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let f: [N] = 1000 [kg] * (50 [m*s^-1] - 20 [m*s^-1]) * 10 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'f' requires [N] type but received [kg*m]", e.Message);
    }

    [Fact]
    public void TestUnitAddOperationWhenOneUnitIsAsAlias()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let force: [N] = 10 [N] + 12 [kg*m*s^-2]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitAddOperationWhenOneUnitIsAsAliasWithErrors()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let force: [N] = (10 [N] + 12 [kg*m*s^-2]) * 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'force' requires [N] type but received [kg*m^2*s^-2]", e.Message);
    }

    [Fact]
    public void TestUnitAddOperationWhenOneUnitDeclaredUnitIsOnRight()
    {
        const string code = @"
                            unit N: [kg*m*s^-2]
                            main() -> void {
                                let force: [kg*m*s^-2] = 10 [N] + 12 [kg*m*s^-2]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitOperationWithFunctionCallValid()
    {
        const string code = @"
                            getLength() -> [m] {
                                return 5 [m]
                            }
                            main() -> void {
                                let s: [m] = 3 [m]
                                let length: [m] = getLength() - s
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitOperationWithFunctionCallInvalid()
    {
        const string code = @"
                            getTime() -> [s] {
                                return 5 [s]
                            }
                            main() -> void {
                                let t: [s] = 3 [s]
                                let s: [m] = getTime() - t
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'s' requires [m] type but received [s]", e.Message);
    }

    [Fact]
    public void TestUnitOperationWithFunctionCallInvalid2()
    {
        const string code = @"
                            getLength() -> [m] {
                                return 5 [m]
                            }
                            main() -> void {
                                let s: [m] = 3 [m]
                                let length: [m] = getLength() / s
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'length' requires [m] type but received []", e.Message);
    }

    [Fact]
    public void TestFunctionCallsWrongNumberOfArguments()
    {
        const string code = @"
                            getLength(s1: [m], s2: [m]) -> [m] {
                                return s2 - s1
                            }
                            main() -> void {
                                let s: [m] = 3 [m]
                                let length: [m] = getLength(s) - s
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<WrongNumberOfArgumentsException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'getLength' function was invoked with 1 argument(s) but expected 2 argument(s)", e.Message);
    }

    [Fact]
    public void TestSameVariables()
    {
        const string code = @"
                            fn() -> void {
                                let s: [m] = 3 [m]
                                let s1: [m] = 4 [m]
                            }

                            main() -> void {
                                let s: [m] = 3 [m]
                                let s1: [m] = 4 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestFunctionCallsWrongTypesOfArguments()
    {
        const string code = @"
                            getLength(s1: [m], s2: [m]) -> [m] {
                                return s2 - s1
                            }
                            main() -> void {
                                let length: [m] = getLength(5 [s], 10 [s])
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'s1' requires [m] type but received [s]", e.Message);
    }


    [Fact]
    public void TestFunctionCallsWrongTypesOfArgumentsWhenExpressionValid()
    {
        const string code = @"
                            getTime(v: [m*s^-1], s: [m]) -> [s] {
                                return s / v
                            }
                            main() -> void {
                                let t: [s] = 10 [s]
                                let s: [m] = 10 [m]

                                let length: [m] = getTime(s / t, 10 [s])
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'s' requires [m] type but received [s]", e.Message);
    }

    [Fact]
    public void TestFunctionCallsWrongTypesOfArgumentsWhenExpressionInvalid()
    {
        const string code = @"
                            getTime(v: [m*s^-1], s: [m]) -> [s] {
                                return s / v
                            }
                            main() -> void {
                                let t: [s] = 10 [s]
                                let s: [m] = 10 [m]

                                let length: [m] = getTime(t / s, 10 [s])
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'v' requires [m*s^-1] type but received [s*m^-1]", e.Message);
    }


    [Fact]
    public void TestOperationErrorStringPlusInt()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = ""my string"" + 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '+' for string and []", e.Message);
    }

    [Fact]
    public void TestStringNotEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = ""sss"" != ""www""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestStringEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = ""sss"" == ""www""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestBooleansNotEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = true != false
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestBooleansEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = true == true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitsNotEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] != 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }


    [Fact]
    public void TestUnitsEquality()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] == 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitsEqualityUnitMismatch()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] == 10 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '==' for [m] and [s]", e.Message);
    }

    [Fact]
    public void TestEqualityOfDifferentTypes()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] == true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '==' for [m] and bool", e.Message);
    }

    [Fact]
    public void TestNotEqualityOfDifferentTypes()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = true != ""asd""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '!=' for bool and string", e.Message);
    }

    [Fact]
    public void TestGreaterThanUnits()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] > 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitEqualityWrongVariableType()
    {
        const string code = @"
                            main() -> void {
                                let x: [] = 5 [m] == 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'x' requires [] type but received bool", e.Message);
    }

    [Fact]
    public void TestGreaterThanUnitsFailure()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = 5 [m] > 10 [s]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '>' for [m] and [s]", e.Message);
    }

    [Fact]
    public void TestUnitsNotEqualityUnitMismatch()
    {
        const string code = @"
                            getS(t1: [s]) -> [s] {
                                return 5 [s] + t1
                            }
                            main() -> void {
                                let x: bool = 5 [m] != getS(3 [s])
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '!=' for [m] and [s]", e.Message);
    }

    [Fact]
    public void TestOperationErrorStringDivideString()
    {
        const string code = @"
                            main() -> void {
                                let firstString: string= ""sss""
                                let secondString: string = ""www""
                                let x: string = firstString / secondString
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '/' for string and string", e.Message);
    }

    [Fact]
    public void TestOperationErrorStringGreaterEqualFromFunction()
    {
        const string code = @"
                            fun() -> string {
                                return ""string""
                            }
                            main() -> void {
                                let firstString: string = ""sss""
                                let x: bool = firstString >= fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);

        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '>=' for string and string", e.Message);
    }

    [Fact]
    public void TestOperationIntAndBool()
    {
        const string code = @"
                            fun() -> bool {
                                return true
                            }
                            main() -> void {
                                let firstInt: [] = 5
                                let x: [] = firstInt - fun()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for [] and bool", e.Message);
    }

    [Fact]
    public void TestUnitsIfConditionsValid()
    {
        const string code = @"
                            main() -> void {
                                let mass: [kg] = 3 [kg]
                                if(mass > 5 [kg]) {
                                    
                                } else if(mass < 3 [kg]) {
                                    
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestUnitsIfConditionsGreaterThanInvalid()
    {
        const string code = @"
                            main() -> void {
                                let mass: [kg] = 3 [kg]
                                if(mass > 5) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '>' for [kg] and []", e.Message);
    }

    [Fact]
    public void TestUnitsIfConditionsSmallerThanInvalid()
    {
        const string code = @"
                            main() -> void {
                                let mass: [kg] = 3 [kg]
                                if(mass < 5 [s]) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '<' for [kg] and [s]", e.Message);
    }

    [Fact]
    public void TestUnitsIfConditionsEqualInvalid()
    {
        const string code = @"
                            main() -> void {
                                if(3 [kg] == 5 [mol]) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '==' for [kg] and [mol]", e.Message);
    }

    [Fact]
    public void TestUnitsIfConditionsNotEqualInvalid()
    {
        const string code = @"
                            main() -> void {
                                if(3 [mol] != 5 [s]) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '!=' for [mol] and [s]", e.Message);
    }


    [Fact]
    public void TestStringsIfConditionGreaterOrEqualThan()
    {
        const string code = @"
                            main() -> void {
                                let s1: string = ""string1""
                                let s2: string = ""string2""
                                if(s1 >= s2) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '>=' for string and string", e.Message);
    }

    [Fact]
    public void TestStringsIfConditionSmallerOrEqualThanInline()
    {
        const string code = @"
                            main() -> void {
                                if(""string1"" <= ""string2"") {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '<=' for string and string", e.Message);
    }

    [Fact]
    public void TestBooleansIfCondition()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = true
                                let b2: bool = false
                                if(b1 <= b2) {
                                    print()
                                }
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '<=' for bool and bool", e.Message);
    }

    [Fact]
    public void TestBooleansExpression()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = true * false
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '*' for bool and bool", e.Message);
    }

    [Fact]
    public void TestBooleanUnitExpression()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = 5 / true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '/' for [] and bool", e.Message);
    }

    [Fact]
    public void TestBooleanStringExpression()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = true + ""sss""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '+' for bool and string", e.Message);
    }

    [Fact]
    public void TestUnitStringExpression()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = 5 + ""sss""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '+' for [] and string", e.Message);
    }

    [Fact]
    public void TestStringUnitExpression()
    {
        const string code = @"
                            main() -> void {
                                let b1: [] = ""sss"" * 5 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '*' for string and [m]", e.Message);
    }

    [Fact]
    public void TestAddTwoStrings()
    {
        const string code = @"
                            main() -> void {
                                let s1: string = ""true"" + ""sss""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestSubtractTwoStrings()
    {
        const string code = @"
                            main() -> void {
                                let s1: string = ""true"" - ""sss""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for string and string", e.Message);
    }

    [Fact]
    public void TestBooleansTypeMatchAndOrOperator()
    {
        const string code = @"
                            boolFn() -> bool {
                                return true
                            }
                            main() -> void {
                                let a: bool = false
                                let b1: bool = a || boolFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestBooleansTypeMatchAndAndOperator()
    {
        const string code = @"
                            boolFn() -> bool {
                                return true
                            }
                            main() -> void {
                                let a: bool = false
                                let b1: bool = a && boolFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestBooleansTypeMismatch()
    {
        const string code = @"
                            notBoolFn() -> [] {
                                return 5
                            }
                            main() -> void {
                                let a: bool = false
                                let b1: bool = notBoolFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'b1' requires bool type but received []", e.Message);
    }

    [Fact]
    public void TestOrOperatorNotValid()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = 5 [m] || false
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '||' for [m] and bool", e.Message);
    }

    [Fact]
    public void TestAndOperatorNotValid()
    {
        const string code = @"
                            main() -> void {
                                let b1: bool = 5 [m] || 10 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '||' for [m] and [m]", e.Message);
    }

    [Fact]
    public void TestAndOperatorForStringsNotValid()
    {
        const string code = @"
                            main() -> void {
                                let b1: string = ""c"" && ""xx""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '&&' for string and string", e.Message);
    }

    [Fact]
    public void TestExpressionWithVoid()
    {
        const string code = @"
                            voidFn() -> void {
                                return
                            }
                            main() -> void {
                                let a: [] = 5 [m] + voidFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '+' for [m] and void", e.Message);
    }

    [Fact]
    public void TestMinusString()
    {
        const string code = @"
                            main() -> void {
                                let x: string = -""aaa""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for string type", e.Message);
    }

    [Fact]
    public void TestMinusUnit()
    {
        const string code = @"
                            main() -> void {
                                let x: [m] = -2 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestMinusBool()
    {
        const string code = @"
                            main() -> void {
                                let x: bool = -true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '-' for bool type", e.Message);
    }

    [Fact]
    public void TestNotString()
    {
        const string code = @"
                            main() -> void {
                                let x: string = !""aaa""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '!' for string type", e.Message);
    }

    [Fact]
    public void TestNotUnitFloat()
    {
        const string code = @"
                            main() -> void {
                                let y: [] = 5.5
                                let x: [] = !y
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<UnpermittedOperationException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("Unsupported operator '!' for [] type", e.Message);
    }

    [Fact]
    public void TestNotBool()
    {
        const string code = @"
                            boolFn() -> bool {
                                return true
                            }
                            main() -> void {
                                let y: bool = false
                                let x: bool = !y || !boolFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestNotValidBoolReturnType()
    {
        const string code = @"
                            boolFn() -> bool {
                                return 5
                            }
                            main() -> void {
                                let x: bool = !y || !boolFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'boolFn' return requires bool type but returned []", e.Message);
    }

    [Fact]
    public void TestValidStringReturnType()
    {
        const string code = @"
                            stringFn() -> string {
                                return ""test""
                            }
                            main() -> void {
                                let x: string = ""abc "" + stringFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestNotValidStringReturnType()
    {
        const string code = @"
                            stringFn() -> string {
                                return true
                            }
                            main() -> void {
                                let x: string = ""abc "" + stringFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'stringFn' return requires string type but returned bool", e.Message);
    }

    [Fact]
    public void TestValidVoidReturnType()
    {
        const string code = @"
                            main() -> void {
                                return
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestInvalidReturnType()
    {
        const string code = @"
                            main() -> void {
                                return 5 [m]
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'main' return requires void type but returned [m]", e.Message);
    }

    [Fact]
    public void TestValidReturnTypeFromIf()
    {
        const string code = @"
                            myFn() -> [m] {
                                let x: [] = 10
                                if (5 [] > x) {
                                    return 5 [m]
                                }
                                return 4 [m]
                            }
                            main() -> void {
                                let y: [m] = myFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestNotValidReturnTypeFromIf()
    {
        const string code = @"
                            myFn() -> [m] {
                                let x: [] = 10
                                if (5 [] > x) {
                                    return 5 [s]
                                }
                                return 4 [m]
                            }
                            main() -> void {
                                let y: [m] = myFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'myFn' return requires [m] type but returned [s]", e.Message);
    }

    [Fact]
    public void TestValidIfReturnTypeAndNotValidFromFunctionBody()
    {
        const string code = @"
                            myFn22() -> [kg] {
                                let x: [] = 10
                                if (5 [] > x) {
                                    return 5 [kg]
                                }
                                return 4 [A]
                            }
                            main() -> void {
                                let y: [kg] = myFn22()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'myFn22' return requires [kg] type but returned [A]", e.Message);
    }


    [Fact]
    public void TestNotValidReturnFromNestedIfInWhile()
    {
        const string code = @"
                            myFn() -> [] {
                                let x: [] = 10
                                while (5 < x) {
                                    if (x == 6) {
                                        return 5 [m]
                                    }
                                }
                            }
                            main() -> void {
                                let y: [] = myFn()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<NotValidReturnTypeException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'myFn' return requires [] type but returned [m]", e.Message);
    }

    [Fact]
    public void TestReAssigningValid()
    {
        const string code = @"
                            myFn() -> [m] {
                                return 10 [m]
                            }
                            main() -> void {
                                let y: [m] = myFn()
                                let z: [m] = 20 [m]
                                y = z
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestNotValidReAssigningWithVariable()
    {
        const string code = @"
                            myFn() -> [m] {
                                return 10 [m]
                            }
                            main() -> void {
                                let meter: [m] = myFn()
                                let z: [mol] = 20 [mol]
                                meter = z
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'meter' requires [m] type but received [mol]", e.Message);
    }

    [Fact]
    public void TestReAssigningUnits()
    {
        const string code = @"
                            myFn() -> [m] {
                                return 10 [m]
                            }
                            main() -> void {
                                let y: [m] = myFn()
                                y = 5
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'y' requires [m] type but received []", e.Message);
    }

    [Fact]
    public void TestReAssigningStringsNotValid()
    {
        const string code = @"
                            myFn() -> string {
                                return ""test""
                            }
                            main() -> void {
                                let str: string = myFn()
                                str = true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'str' requires string type but received bool", e.Message);
    }

    [Fact]
    public void TestReAssigningStringsValid()
    {
        const string code = @"
                            myFn() -> string {
                                return ""test""
                                }
                            }
                            main() -> void {
                                let str: string = myFn()
                                str = ""new test""
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }

    [Fact]
    public void TestReAssigningBooleansNotValid()
    {
        const string code = @"
                            getUnit() -> [] {
                                return 20
                            }
                            main() -> void {
                                let boolean: bool = true
                                boolean = getUnit()
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        var e = Assert.Throws<TypeMismatchException>(() =>
            semanticAnalyzer.Visit(program));
        Assert.Equal("'boolean' requires bool type but received []", e.Message);
    }

    [Fact]
    public void TestReAssigningBooleansValid()
    {
        const string code = @"
                            main() -> void {
                                let boolean: bool = false
                                boolean = true
                            }";

        var parser = Helper.PrepareParser(code);
        var program = parser.Parse();

        var builtinFunctionsProvider = new BuiltInFunctionsProvider();
        var semanticAnalyzer = new SemanticAnalyzerVisitor(builtinFunctionsProvider);
        semanticAnalyzer.Visit(program);
    }
}
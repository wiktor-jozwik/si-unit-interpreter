using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.semantic_analyzer;

public class TypeAnalyzerVisitor: IVisitor<IType>
{
    private readonly LinkedList<SemanticScope> _scopes;
    private readonly Dictionary<string, FunctionStatement> _functions;
    private readonly IDictionary<string, UnitType> _units;

    public TypeAnalyzerVisitor(LinkedList<SemanticScope> scopes, Dictionary<string, FunctionStatement> functions, IDictionary<string, UnitType> units)
    {
        _scopes = scopes;
        _functions = functions;
        _units = units;
    }
    
    public IType Visit(TopLevel element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(FunctionStatement element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(Block element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(VariableDeclaration element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(IfStatement element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(ElseIfStatement element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(WhileStatement element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(AssignStatement element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(ReturnStatement element)
    {
        throw new NotImplementedException();
    }
    public IType Visit(AddExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            CheckOperation(leftType, "+", rightType);
            
            // As leftType and rightType are the same we can return either
            return leftType;
        }

        throw new Exception();
    }

    public IType Visit(SubtractExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            CheckOperation(leftType, "-", rightType);
            
            // As leftType and rightType are the same we can return either
            return leftType;
        }

        throw new Exception();
    }

    public IType Visit(EqualExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(GreaterEqualThanExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(GreaterThanExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(NotEqualExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(SmallerEqualThanExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(SmallerThanExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(BoolLiteral element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(FloatLiteral element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(IntLiteral element)
    {
        return element.UnitType;
    }

    public IType Visit(StringLiteral element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(DivideExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var expressionUnits = ((UnitType) leftType).Units.Select(u => u.Clone()).ToList();
            var divideByUnits = ((UnitType) rightType).Units.Select(u => u.Clone()).ToList();

            foreach (var rightUnit in divideByUnits)
            {
                rightUnit.Power *= -1;
            }

            return JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new Exception();
    }

    public IType Visit(MultiplicateExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var expressionUnits = ((UnitType) leftType).Units.Select(u => u.Clone()).ToList();
            var divideByUnits = ((UnitType) rightType).Units.Select(u => u.Clone()).ToList();

            return JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new Exception();   
    }

    public IType Visit(MinusExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(NotExpression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(Expression element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(FunctionCall element)
    {
        var name = element.Name;
        
        if (_functions.TryGetValue(name, out var function))
        {
            var expectedNumberOfArguments = function.Parameters.Count;
            var passedNumberOfArguments = element.Arguments.Count;
            if (passedNumberOfArguments != expectedNumberOfArguments)
            {
                throw new WrongNumberOfArgumentsException(name, expectedNumberOfArguments, passedNumberOfArguments);
            }

            CompareArguments(element.Arguments, function.Parameters);            
            
            return function.ReturnType;
        }
        throw new FunctionUndeclaredException(name);
    }

    public IType Visit(Identifier element)
    {
        foreach (var scope in _scopes)
        {
            if (scope.Variables.TryGetValue(element.Name, out var type))
            {
                return type;
            }
        }

        throw new Exception();
    }

    public IType Visit(LogicFactor element)
    {
        throw new NotImplementedException();
    }

    public IType Visit(Parameter element)
    {
        throw new NotImplementedException();
    }

    public void CompareTypes(string name, IType left, IType right)
    {
        if (left.GetType() == typeof(UnitType) && right.GetType() == typeof(UnitType))
        {
            var leftUnit = (UnitType) left;
            var rightUnit = (UnitType) right;
            if (!_AreUnitsTheSame(leftUnit, rightUnit))
            {
                throw new TypeMismatchException(name, leftUnit, rightUnit);
            }
        }
    }
    
    private static UnitType JoinTwoUnits(IList<Unit> leftUnits, IList<Unit> rightUnits)
    {
        var units = new List<Unit>();

        foreach (var leftUnit in leftUnits)
        {
            var unit = rightUnits.FirstOrDefault(u => u.Name == leftUnit.Name);
            if (unit != null)
            {
                leftUnit.Power += unit.Power;
                if (leftUnit.Power != 0)
                {
                    units.Add(leftUnit);
                }
                rightUnits = new List<Unit>(rightUnits.Where(u => u != unit));
            }
            else
            {
                units.Add(leftUnit);
            }
        }

        units.AddRange(rightUnits);

        return new UnitType(units);
    }
    
    private void CompareArguments(IEnumerable<IExpression> passedArguments, IEnumerable<Parameter> expectedParameters)
    {
        foreach (var (passedArg, expectedParameter) in passedArguments.Zip(expectedParameters))
        {
            var passedType = passedArg.Accept(this);
            var expectedType = expectedParameter.Type;
            
            CompareTypes(expectedParameter.Name, expectedType, passedType);
        }
    }

    private void CheckOperation(IType left, string operation, IType right)
    {
        if (left.GetType() == typeof(UnitType) && right.GetType() == typeof(UnitType))
        {
            var leftUnit = (UnitType) left;
            var rightUnit = (UnitType) right;
            if (!_AreUnitsTheSame(leftUnit, rightUnit))
            {
                throw new UnpermittedOperationException(leftUnit, operation, rightUnit);
            }
        }
    }

    private bool _AreUnitsTheSame(UnitType leftUnit, UnitType rightUnit)
    {
        var evaluatedLeftUnits = _EvaluateUnit(leftUnit);        
        var evaluatedRightUnits = _EvaluateUnit(rightUnit);        
        
        if (evaluatedLeftUnits.Count != evaluatedRightUnits.Count)
        {
            return false;
        }
        
        foreach (var unit in evaluatedLeftUnits)
        {
            var foundUnitInRight = evaluatedRightUnits.FirstOrDefault(u => unit.Name == u.Name && unit.Power == u.Power) != null;
            // var foundUnitInDeclaredUnits = _units.ContainsKey(unit.Name);
            if (!foundUnitInRight)
            {
                return false;
            }
        }

        return true;
    }

    private List<Unit> _EvaluateUnit(UnitType unitType)
    {
        var units = new List<Unit>();
        foreach (var unit in unitType.Units)
        {
            if (_units.TryGetValue(unit.Name, out var foundUnitInDeclared))
            {
                units.AddRange(foundUnitInDeclared.Units);
            }
            else
            {
                units.Add(unit);
            }
        }

        return units;
    }
}
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

public class TypeAnalyzerVisitor : IVisitor<IType>
{
    private readonly LinkedList<SemanticScope> _scopes;
    private readonly SemanticScope _functionParameters;
    private readonly Dictionary<string, FunctionStatement> _functions;
    private readonly IDictionary<string, UnitType> _units;
    private readonly IDictionary<string, UnitType> _defaultSiUnits;

    public TypeAnalyzerVisitor(LinkedList<SemanticScope> scopes, SemanticScope functionParameters, Dictionary<string, FunctionStatement> functions,
        IDictionary<string, UnitType> units)
    {
        _scopes = scopes;
        _functionParameters = functionParameters;
        _functions = functions;
        _units = units;
        // init SI units
        _defaultSiUnits = new Dictionary<string, UnitType>()
        {
            ["s"] = new(new List<Unit> {new("s")}),
            ["m"] = new(new List<Unit> {new("m")}),
            ["kg"] = new(new List<Unit> {new("kg")}),
            ["A"] = new(new List<Unit> {new("A")}),
            ["K"] = new(new List<Unit> {new("K")}),
            ["mol"] = new(new List<Unit> {new("mol")}),
            ["cd"] = new(new List<Unit> {new("cd")}),
        };
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
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;
            if (!_AreUnitsTheSame(leftUnits, rightUnits))
            {
                throw new UnpermittedOperationException(leftType, "+", rightType);
            }

            return leftType;
        }

        if (leftType.GetType() == typeof(StringType) && rightType.GetType() == typeof(StringType))
        {
            return leftType;
        }

        throw new UnpermittedOperationException(leftType, "+", rightType);
    }

    public IType Visit(SubtractExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;
            if (!_AreUnitsTheSame(leftUnits, rightUnits))
            {
                throw new UnpermittedOperationException(leftType, "-", rightType);
            }

            return leftType;
        }

        throw new UnpermittedOperationException(leftType, "-", rightType);
    }

    public IType Visit(EqualExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForEquality(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "==", rightType);
    }

    public IType Visit(GreaterEqualThanExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, ">=", rightType);
    }

    public IType Visit(GreaterThanExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, ">", rightType);
    }

    public IType Visit(NotEqualExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForEquality(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "!=", rightType);
    }

    public IType Visit(SmallerEqualThanExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "<=", rightType);
    }

    public IType Visit(SmallerThanExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "<", rightType);
    }

    public IType Visit(BoolLiteral element)
    {
        return new BoolType();
    }

    public IType Visit(FloatLiteral element)
    {
        return element.UnitType;
    }

    public IType Visit(IntLiteral element)
    {
        return element.UnitType;
    }

    public IType Visit(StringLiteral element)
    {
        return new StringType();
    }

    public IType Visit(DivideExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;

            var expressionUnits = leftUnits.Units.Select(u => u.Clone()).ToList();
            var divideByUnits = rightUnits.Units.Select(u => u.Clone()).ToList();

            foreach (var rightUnit in divideByUnits)
            {
                rightUnit.Power *= -1;
            }

            return JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new UnpermittedOperationException(leftType, "/", rightType);
    }

    public IType Visit(MultiplicateExpression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;

            var expressionUnits = leftUnits.Units.Select(u => u.Clone()).ToList();
            var divideByUnits = rightUnits.Units.Select(u => u.Clone()).ToList();

            return JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new UnpermittedOperationException(leftType, "*", rightType);
    }

    public IType Visit(MinusExpression element)
    {
        var type = element.Child.Accept(this);
        if (_CheckTypeOfLiteral(type, typeof(UnitType))) return type;

        throw new UnpermittedOperationException(type, "-");
    }

    public IType Visit(NotExpression element)
    {
        var type = element.Child.Accept(this);
        if (_CheckTypeOfLiteral(type, typeof(BoolType))) return type;

        throw new UnpermittedOperationException(type, "!");
    }

    public IType Visit(Expression element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(BoolType) && rightType.GetType() == typeof(BoolType)) return leftType;

        throw new UnpermittedOperationException(leftType, "||", rightType);
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
        var name = element.Name;
        foreach (var scope in _scopes)
        {
            if (scope.Variables.TryGetValue(name, out var type))
            {
                return type;
            }
        }
        
        if (_functionParameters.Variables.TryGetValue(name, out var parameterType))
        {
            return parameterType;
        }

        throw new VariableUndeclaredException(name);
    }

    public IType Visit(LogicFactor element)
    {
        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (leftType.GetType() == typeof(BoolType) && rightType.GetType() == typeof(BoolType)) return leftType;

        throw new UnpermittedOperationException(leftType, "&&", rightType);
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

            return;
        }

        if (left.GetType() == typeof(BoolType) && right.GetType() == typeof(BoolType)) return;
        if (left.GetType() == typeof(StringType) && right.GetType() == typeof(StringType)) return;

        throw new TypeMismatchException(name, left, right);
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

    public bool _AreUnitsTheSame(UnitType leftUnit, UnitType rightUnit)
    {
        var evaluatedLeftUnits = _EvaluateUnit(leftUnit);
        var evaluatedRightUnits = _EvaluateUnit(rightUnit);

        if (evaluatedLeftUnits.Count != evaluatedRightUnits.Count)
        {
            return false;
        }

        foreach (var unit in evaluatedLeftUnits)
        {
            var foundUnitInRight =
                evaluatedRightUnits.FirstOrDefault(u => unit.Name == u.Name && unit.Power == u.Power) != null;
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
            else if (!_defaultSiUnits.ContainsKey(unit.Name))
            {
                throw new UnitUndeclaredException(unit.Name);
            }
            else
            {
                units.Add(unit);
            }
        }

        return units;
    }

    private bool _CheckIfValidTypesForComparison(IType leftType, IType rightType)
    {
        if (leftType.GetType() == typeof(UnitType) && rightType.GetType() == typeof(UnitType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;
            return _AreUnitsTheSame(leftUnits, rightUnits);
        }

        return false;
    }

    private bool _CheckIfValidTypesForEquality(IType leftType, IType rightType)
    {
        if (_CheckTypeOfLiteral(leftType, typeof(UnitType)) &&
            _CheckTypeOfLiteral(rightType, typeof(UnitType)))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;
            return _AreUnitsTheSame(leftUnits, rightUnits);
        }

        if (_CheckTypeOfLiteral(leftType, typeof(BoolType)) &&
            _CheckTypeOfLiteral(rightType, typeof(BoolType)))
        {
            return true;
        }

        if (_CheckTypeOfLiteral(leftType, typeof(StringType)) &&
            _CheckTypeOfLiteral(rightType, typeof(StringType)))
        {
            return true;
        }

        return false;
    }

    private static bool _CheckTypeOfLiteral(IType actualType, Type expectedType)
    {
        return actualType.GetType() == expectedType;
    }
}
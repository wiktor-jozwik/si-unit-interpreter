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

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class TypeAnalyzerVisitor : IVisitor<IType>
{
    private readonly SemanticFunctionCallContext _semanticFunctionCallContext;
    private readonly Dictionary<string, FunctionStatement> _functions;
    private readonly IDictionary<string, UnitType> _units;
    private readonly IDictionary<string, UnitType> _defaultSiUnits;

    public TypeAnalyzerVisitor(SemanticFunctionCallContext semanticFunctionCallContext, Dictionary<string, FunctionStatement> functions,
        IDictionary<string, UnitType> units)
    {
        _semanticFunctionCallContext = semanticFunctionCallContext;
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

    public IType Visit(VariableDeclaration element)
    {
        var name = element.Parameter.Name;
        var variableType = element.Parameter.Type;

        if (
            _semanticFunctionCallContext.Scopes.Any(scope => scope.Variables.Any(variables => variables.Key.Contains(name))) ||
            _semanticFunctionCallContext.Parameters.Any(param => param.Key.Contains(name))
        )
        {
            throw new VariableRedeclarationException(name);
        }

        var typeExpression = element.Expression.Accept(this);

        CompareTypes(name, variableType, typeExpression);

        return variableType;
    }

    public IType Visit(AssignStatement element)
    {
        var variableType = element.Identifier.Accept(this);
        var typeExpression = element.Expression.Accept(this);

        CompareTypes(element.Identifier.Name, variableType, typeExpression);

        return variableType;
    }

    public IType Visit(ReturnStatement element)
    {
        var functionName = _semanticFunctionCallContext.FunctionName;

        if (!_functions.TryGetValue(functionName, out var function))
        {
            throw new FunctionUndeclaredException(functionName);
        }

        var functionReturnType = function.ReturnType;
        var returnedType = element.Expression == null ? new VoidType() : element.Expression.Accept(this);

        if (returnedType.GetType() == typeof(UnitType) && functionReturnType.GetType() == typeof(UnitType))
        {
            if (!_AreUnitsTheSame((UnitType) functionReturnType, (UnitType) returnedType))
            {
                throw new NotValidReturnTypeException(functionName, functionReturnType, returnedType);
            }
        }

        if (returnedType.GetType() != functionReturnType.GetType())
        {
            throw new NotValidReturnTypeException(functionName, functionReturnType, returnedType);
        }

        return returnedType;
    }

    public IType Visit(Expression element)
    {
        // only valid for two booleans

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (_IsBool(leftType) && _IsBool(rightType)) return leftType;

        throw new UnpermittedOperationException(leftType, "||", rightType);
    }

    public IType Visit(LogicFactor element)
    {
        // only valid for two booleans

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (_IsBool(leftType) && _IsBool(rightType)) return leftType;

        throw new UnpermittedOperationException(leftType, "&&", rightType);
    }

    public IType Visit(EqualExpression element)
    {
        // only valid for two same units or two booleans or two strings

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForEquality(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "==", rightType);
    }

    public IType Visit(NotEqualExpression element)
    {
        // only valid for two same units or two booleans or two strings

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForEquality(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "!=", rightType);
    }

    public IType Visit(GreaterEqualThanExpression element)
    {
        // only valid for two same units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, ">=", rightType);
    }

    public IType Visit(GreaterThanExpression element)
    {
        // only valid for two same units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, ">", rightType);
    }

    public IType Visit(SmallerEqualThanExpression element)
    {
        // only valid for two same units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "<=", rightType);
    }

    public IType Visit(SmallerThanExpression element)
    {
        // only valid for two same units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_CheckIfValidTypesForComparison(leftType, rightType)) return new BoolType();

        throw new UnpermittedOperationException(leftType, "<", rightType);
    }

    public IType Visit(AddExpression element)
    {
        // only valid for two same units or two strings

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;
            if (!_AreUnitsTheSame(leftUnits, rightUnits))
            {
                throw new UnpermittedOperationException(leftType, "+", rightType);
            }

            return leftType;
        }

        if (_IsString(leftType) && _IsString(rightType))
        {
            return leftType;
        }

        throw new UnpermittedOperationException(leftType, "+", rightType);
    }

    public IType Visit(SubtractExpression element)
    {
        // only valid for two same units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);
        if (_IsUnit(leftType) && _IsUnit(rightType))
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

    public IType Visit(MultiplicateExpression element)
    {
        // only valid for two units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;

            var expressionUnits = _CloneUnitList(leftUnits);
            var divideByUnits = _CloneUnitList(rightUnits);

            return _JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new UnpermittedOperationException(leftType, "*", rightType);
    }

    public IType Visit(DivideExpression element)
    {
        // only valid for two units

        var leftType = element.Left.Accept(this);
        var rightType = element.Right.Accept(this);

        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            var leftUnits = (UnitType) leftType;
            var rightUnits = (UnitType) rightType;

            var expressionUnits = _CloneUnitList(leftUnits);
            var divideByUnits = _CloneUnitList(rightUnits);

            foreach (var rightUnit in divideByUnits)
            {
                rightUnit.Power *= -1;
            }

            return _JoinTwoUnits(expressionUnits, divideByUnits);
        }

        throw new UnpermittedOperationException(leftType, "/", rightType);
    }

    public IType Visit(NotExpression element)
    {
        // only valid for a boolean

        var type = element.Child.Accept(this);
        if (_IsBool(type)) return type;

        throw new UnpermittedOperationException(type, "!");
    }

    public IType Visit(MinusExpression element)
    {
        // Operator -
        // only valid for a unit

        var type = element.Child.Accept(this);
        if (_IsUnit(type)) return type;

        throw new UnpermittedOperationException(type, "-");
    }

    public IType Visit(Identifier element)
    {
        var name = element.Name;
        foreach (var scope in _semanticFunctionCallContext.Scopes)
        {
            if (scope.Variables.TryGetValue(name, out var type))
            {
                return type;
            }
        }

        if (_semanticFunctionCallContext.Parameters.TryGetValue(name, out var parameterType))
        {
            return parameterType;
        }

        throw new VariableUndeclaredException(name);
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

            _CompareArguments(element.Arguments, function.Parameters);

            return function.ReturnType;
        }

        throw new FunctionUndeclaredException(name);
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

    public IType Visit(Parameter element)
    {
        throw new NotImplementedException();
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

    // private methods

    private void CompareTypes(string name, IType leftType, IType rightType)
    {
        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            var leftUnit = (UnitType) leftType;
            var rightUnit = (UnitType) rightType;
            if (!_AreUnitsTheSame(leftUnit, rightUnit))
            {
                throw new TypeMismatchException(name, leftUnit, rightUnit);
            }

            return;
        }

        if (_IsBool(leftType) && _IsBool(rightType)) return;
        if (_IsString(leftType) && _IsString(rightType)) return;

        throw new TypeMismatchException(name, leftType, rightType);
    }

    private static List<Unit> _CloneUnitList(UnitType unitType)
    {
        return unitType.Units.Select(u => u.Clone()).ToList();
    }

    private static UnitType _JoinTwoUnits(IList<Unit> leftUnits, IList<Unit> rightUnits)
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

    private void _CompareArguments(IEnumerable<IExpression> passedArguments, IEnumerable<Parameter> expectedParameters)
    {
        foreach (var (passedArg, expectedParameter) in passedArguments.Zip(expectedParameters))
        {
            var passedType = passedArg.Accept(this);
            var expectedType = expectedParameter.Type;

            CompareTypes(expectedParameter.Name, expectedType, passedType);
        }
    }

    private bool _CheckIfValidTypesForComparison(IType leftType, IType rightType)
    {
        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            return _AreUnitsTheSame((UnitType) leftType, (UnitType) rightType);
        }

        return false;
    }

    private bool _CheckIfValidTypesForEquality(IType leftType, IType rightType)
    {
        if (_IsUnit(leftType) && _IsUnit(rightType))
        {
            return _AreUnitsTheSame((UnitType) leftType, (UnitType) rightType);
        }

        return _IsBool(leftType) && _IsBool(rightType) || _IsString(leftType) && _IsString(rightType);
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

    private static bool _IsUnit(IType type)
    {
        return type.GetType() == typeof(UnitType);
    }

    private static bool _IsString(IType type)
    {
        return type.GetType() == typeof(StringType);
    }

    private static bool _IsBool(IType type)
    {
        return type.GetType() == typeof(BoolType);
    }
}
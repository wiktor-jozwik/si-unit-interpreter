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
using si_unit_interpreter.semantic_analyzer;

namespace si_unit_interpreter;

public class TypeVisitor: IVisitor<IType>
{
    private readonly LinkedList<SemanticScope> _scopes;
    private readonly Dictionary<string, IType> _functions;
    private readonly IDictionary<string, UnitType> _units;

    public TypeVisitor(LinkedList<SemanticScope> scopes, Dictionary<string, IType> functions, IDictionary<string, UnitType> units)
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
        throw new NotImplementedException();
    }

    public IType Visit(SubtractExpression element)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
}
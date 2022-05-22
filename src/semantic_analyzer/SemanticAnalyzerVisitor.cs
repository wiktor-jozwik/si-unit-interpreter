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

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticAnalyzerVisitor : IVisitor
{
    private readonly LinkedList<SemanticScope> _scopes = new();
    private readonly Dictionary<string, FunctionStatement> _functions = new();
    private IDictionary<string, UnitType> _units;

    private Tuple<string, SemanticScope> _currentFunctionScope;

    public void Visit(TopLevel element)
    {
        _units = element.Units;

        foreach (var (name, function) in element.Functions)
        {
            _currentFunctionScope = new Tuple<string, SemanticScope>(name, new SemanticScope());

            _functions[name] = function;
            function.Accept(this);
        }
    }

    public void Visit(FunctionStatement element)
    {
        foreach (var parameter in element.Parameters)
        {
            parameter.Accept(this);
        }
        element.Statements.Accept(this);
    }

    public void Visit(Block element)
    {
        _scopes.AddLast(new SemanticScope());
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }

        _scopes.RemoveLast();
    }

    public void Visit(VariableDeclaration element)
    {
        var name = element.Parameter.Name;
        var variableType = element.Parameter.Type;

        if (_scopes.Any(scope => scope.Variables.Any(variables => variables.Key.Contains(name))))
        {
            throw new VariableRedeclarationException(name);
        }
        if (_currentFunctionScope.Item2.Variables.Any(v => v.Key.Contains(name)))
        {
            throw new VariableRedeclarationException(name);
        }

        var typeVisitor = new TypeAnalyzerVisitor(_scopes, _currentFunctionScope.Item2, _functions, _units);
        var typeExpression = element.Expression.Accept(typeVisitor);

        typeVisitor.CompareTypes(name, variableType, typeExpression);

        _scopes.Last().Variables[name] = variableType;
    }

    public void Visit(IfStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_scopes, _currentFunctionScope.Item2, _functions, _units);

        element.Condition.Accept(typeVisitor);

        element.Statements.Accept(this);

        foreach (var elseIfStatement in element.ElseIfStatements)
        {
            elseIfStatement.Condition.Accept(typeVisitor);

            elseIfStatement.Accept(this);
        }

        element.ElseStatement.Accept(this);
    }

    public void Visit(ElseIfStatement element)
    {
        element.Statements.Accept(this);
    }

    public void Visit(WhileStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_scopes, _currentFunctionScope.Item2, _functions, _units);

        element.Condition.Accept(typeVisitor);

        element.Statements.Accept(this);
    }

    public void Visit(AssignStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(AddExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(SubtractExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(EqualExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(GreaterEqualThanExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(GreaterThanExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(NotEqualExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(SmallerEqualThanExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(SmallerThanExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(DivideExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(MultiplicateExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(MinusExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(NotExpression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(Expression element)
    {
        throw new NotImplementedException();
    }

    public void Visit(LogicFactor element)
    {
        throw new NotImplementedException();
    }

    public void Visit(BoolLiteral element)
    {
        throw new NotImplementedException();
    }

    public void Visit(FloatLiteral element)
    {
        throw new NotImplementedException();
    }

    public void Visit(IntLiteral element)
    {
        throw new NotImplementedException();
    }

    public void Visit(StringLiteral element)
    {
        throw new NotImplementedException();
    }

    public void Visit(FunctionCall element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_scopes, _currentFunctionScope.Item2, _functions, _units);
        element.Accept(typeVisitor);
    }

    public void Visit(Identifier element)
    {
        var name = element.Name;

        if (!_scopes.Any(scope => scope.Variables.ContainsKey(name)))
        {
            if (_currentFunctionScope.Item2.Variables.Any(v => v.Key.Contains(name)))
            {
                throw new VariableUndeclaredException(name);
            }
        }
    }

    public void Visit(Parameter element)
    {
        _currentFunctionScope.Item2.Variables[element.Name] = element.Type;
    }

    public void Visit(ReturnStatement element)
    {
        var typeVisitor = new TypeAnalyzerVisitor(_scopes, _currentFunctionScope.Item2, _functions, _units);

        var name = _currentFunctionScope.Item1;
        if (_functions.TryGetValue(name, out var function))
        {
            var functionReturnType = function.ReturnType;

            var returnedType = element.Expression == null ? new VoidType() : element.Expression.Accept(typeVisitor);

            if (returnedType.GetType() == typeof(UnitType) && functionReturnType.GetType() == typeof(UnitType))
            {
                if (!typeVisitor._AreUnitsTheSame((UnitType) functionReturnType, (UnitType) returnedType))
                {
                    throw new NotValidReturnTypeException(name, functionReturnType, returnedType);
                }
            }

            if (returnedType.GetType() != functionReturnType.GetType())
            {
                throw new NotValidReturnTypeException(name, functionReturnType, returnedType);
            }
        }
    }
}
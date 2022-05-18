using si_unit_interpreter.exceptions.semantic_analyzer;
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticAnalyzerVisitor: IVisitor
{
    private readonly LinkedList<SemanticScope> _scopes = new();
    public void Visit(TopLevel element)
    {
        foreach (var (_, function) in element.Functions)
        {
            function.Accept(this);
        }
    }
    
    public void Visit(FunctionStatement element)
    {
        _scopes.AddLast(new SemanticScope());
        element.Statements.Accept(this);
        _scopes.RemoveLast();
    }
    
    public void Visit(Block element)
    {
        foreach (var statement in element.Statements)
        {
            statement.Accept(this);
        }
    }
    
    public void Visit(VariableDeclaration element)
    {
        var lastScope = _scopes.Last();
        var name = element.Parameter.Name;
        if (lastScope.VariableNames.Contains(name))
        {
            throw new VariableRedeclarationException(name);
        }
        lastScope.VariableNames.Add(name);
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

    public void Visit(FunctionCall element)
    {
        throw new NotImplementedException();
    }

    public void Visit(Identifier element)
    {
        throw new NotImplementedException();
    }

    public void Visit(LogicFactor element)
    {
        throw new NotImplementedException();
    }

    public void Visit(AssignStatement element)
    {
        throw new NotImplementedException();
    }



    public void Visit(ElseIfStatement element)
    {
        throw new NotImplementedException();
    }



    public void Visit(IfStatement element)
    {
        throw new NotImplementedException();
    }

    public void Visit(Parameter element)
    {
        throw new NotImplementedException();
    }

    public void Visit(ReturnStatement element)
    {
        throw new NotImplementedException();
    }



    public void Visit(WhileStatement element)
    {
        throw new NotImplementedException();
    }
}

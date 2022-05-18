using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter;

public interface IVisitor
{
    void Visit(TopLevel element);
    void Visit(FunctionStatement element);
    void Visit(Block element);
    void Visit(VariableDeclaration element);

    void Visit(AddExpression element);
    void Visit(SubtractExpression element);
    void Visit(EqualExpression element);
    void Visit(GreaterEqualThanExpression element);
    void Visit(GreaterThanExpression element);
    void Visit(NotEqualExpression element);
    void Visit(SmallerEqualThanExpression element);
    void Visit(SmallerThanExpression element);
    void Visit(BoolLiteral element);
    void Visit(FloatLiteral element);
    void Visit(IntLiteral element);
    void Visit(StringLiteral element);
    void Visit(DivideExpression element);
    void Visit(MultiplicateExpression element);
    void Visit(MinusExpression element);
    void Visit(NotExpression element);
    void Visit(Expression element);
    void Visit(FunctionCall element);
    void Visit(Identifier element);
    void Visit(LogicFactor element);
    void Visit(AssignStatement element);
    void Visit(ElseIfStatement element);
    void Visit(IfStatement element);
    void Visit(Parameter element);
    void Visit(ReturnStatement element);
    void Visit(WhileStatement element);
}
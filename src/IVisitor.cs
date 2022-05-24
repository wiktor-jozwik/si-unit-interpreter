using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter;

public interface IVisitor<out T>
{
    T Visit(TopLevel element);
    T Visit(FunctionStatement element);
    T Visit(Block element);
    T Visit(VariableDeclaration element);
    T Visit(IfStatement element);
    T Visit(ElseIfStatement element);
    T Visit(WhileStatement element);
    T Visit(AssignStatement element);
    T Visit(ReturnStatement element);
    T Visit(AddExpression element);
    T Visit(SubtractExpression element);
    T Visit(EqualExpression element);
    T Visit(GreaterEqualThanExpression element);
    T Visit(GreaterThanExpression element);
    T Visit(NotEqualExpression element);
    T Visit(SmallerEqualThanExpression element);
    T Visit(SmallerThanExpression element);
    T Visit(BoolLiteral element);
    T Visit(FloatLiteral element);
    T Visit(IntLiteral element);
    T Visit(StringLiteral element);
    T Visit(DivideExpression element);
    T Visit(MultiplicateExpression element);
    T Visit(MinusExpression element);
    T Visit(NotExpression element);
    T Visit(Expression element);
    T Visit(FunctionCall element);
    T Visit(Identifier element);
    T Visit(LogicFactor element);
    T Visit(Parameter element);
}

public interface IVisitor
{
    void Visit(TopLevel element);
    void Visit(FunctionStatement element);
    void Visit(Block element);
    void Visit(VariableDeclaration element);
    void Visit(IfStatement element);
    void Visit(ElseIfStatement element);
    void Visit(WhileStatement element);
    void Visit(AssignStatement element);
    void Visit(ReturnStatement element);
    void Visit(FunctionCall element);
    void Visit(Parameter element);
}
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public interface ITypeVisitor
{
    IType Visit(VariableDeclaration element);
    IType Visit(AssignStatement element);
    IType Visit(ReturnStatement element);
    IType Visit(AddExpression element);
    IType Visit(SubtractExpression element);
    IType Visit(EqualExpression element);
    IType Visit(GreaterEqualThanExpression element);
    IType Visit(GreaterThanExpression element);
    IType Visit(NotEqualExpression element);
    IType Visit(SmallerEqualThanExpression element);
    IType Visit(SmallerThanExpression element);
    IType Visit(BoolLiteral element);
    IType Visit(FloatLiteral element);
    IType Visit(IntLiteral element);
    IType Visit(StringLiteral element);
    IType Visit(DivideExpression element);
    IType Visit(MultiplicateExpression element);
    IType Visit(MinusExpression element);
    IType Visit(NotExpression element);
    IType Visit(OrExpression element);
    IType Visit(FunctionCall element);
    IType Visit(Identifier element);
    IType Visit(AndExpression element);
}
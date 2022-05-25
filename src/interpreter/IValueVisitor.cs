using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;

namespace si_unit_interpreter.interpreter;

public interface IValueVisitor
{
    dynamic Visit(Expression element);
    dynamic Visit(LogicFactor element);
    dynamic Visit(EqualExpression element);
    dynamic Visit(NotEqualExpression element);
    dynamic Visit(GreaterThanExpression element);
    dynamic Visit(GreaterEqualThanExpression element);
    dynamic Visit(SmallerThanExpression element);
    dynamic Visit(SmallerEqualThanExpression element);
    dynamic Visit(AddExpression element);
    dynamic Visit(SubtractExpression element);
    dynamic Visit(MultiplicateExpression element);
    dynamic Visit(DivideExpression element);
    dynamic Visit(MinusExpression element);
    dynamic Visit(NotExpression element);
    dynamic Visit(Identifier element);
    dynamic Visit(FunctionCall element);
    dynamic Visit(BoolLiteral element);
    dynamic Visit(FloatLiteral element);
    dynamic Visit(IntLiteral element);
    dynamic Visit(StringLiteral element);
}
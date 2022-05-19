using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.expression;

public interface IExpression: IVisitable<IType>, IStatement {}
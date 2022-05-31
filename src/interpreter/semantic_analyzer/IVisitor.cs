using si_unit_interpreter.parser;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

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
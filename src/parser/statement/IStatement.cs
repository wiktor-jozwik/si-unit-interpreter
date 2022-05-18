namespace si_unit_interpreter.parser.statement;

public interface IStatement
{
    void Accept(IVisitor visitor);
}
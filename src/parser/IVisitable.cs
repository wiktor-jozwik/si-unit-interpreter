namespace si_unit_interpreter.parser;

public interface IVisitable<T>
{
    T Accept(IVisitor<T> visitor);
}

public interface IVisitable
{
    void Accept(IVisitor visitor);
}

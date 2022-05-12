using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class Parameter: IStatement
{
    public readonly string Name;
    public readonly IType Type;
    
    public Parameter(string name, IType type)
    {
        Name = name;
        Type = type;
    }
}
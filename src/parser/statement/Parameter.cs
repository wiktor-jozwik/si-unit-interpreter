using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class Parameter: IStatement
{
    public readonly string Identifier;
    public readonly IType Type;
    
    public Parameter(string identifier, IType type)
    {
        Identifier = identifier;
        Type = type;
    }
}
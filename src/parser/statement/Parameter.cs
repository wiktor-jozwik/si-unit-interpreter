using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class Parameter: IStatement
{
    public string Identifier;
    public IType Type;
    
    public Parameter(string identifier, IType type)
    {
        Identifier = identifier;
        Type = type;
    }
}
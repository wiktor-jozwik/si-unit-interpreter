using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.parser.statement;

public class UnitDeclaration: IStatement
{
    public string Identifier;
    public UnitType Type;

    public UnitDeclaration(string identifier, UnitType type)
    {
        Identifier = identifier;
        Type = type;
    }
}
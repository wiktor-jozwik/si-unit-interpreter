using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter;

public class FunctionCallContext
{
    public string FunctionName;
    public LinkedList<Scope> Scopes = new();
    public Dictionary<string, dynamic> Parameters;
}
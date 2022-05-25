using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class SemanticFunctionCallContext
{
    public string FunctionName;
    public LinkedList<SemanticScope> Scopes = new();
    public Dictionary<string, IType> Parameters;
}
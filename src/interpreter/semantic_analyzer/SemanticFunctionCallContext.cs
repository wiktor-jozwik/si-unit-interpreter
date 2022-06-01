using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class SemanticFunctionCallContext
{
    public string FunctionName;
    public readonly LinkedList<SemanticScope> Scopes = new();
    public Dictionary<string, IType> Parameters;

    public SemanticFunctionCallContext(string functionName, Dictionary<string, IType> parameters)
    {
        FunctionName = functionName;
        Parameters = parameters;
    }
}
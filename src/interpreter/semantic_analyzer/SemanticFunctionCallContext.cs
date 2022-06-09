namespace si_unit_interpreter.interpreter.semantic_analyzer;

public class SemanticFunctionCallContext
{
    public string FunctionName = "";
    public readonly LinkedList<SemanticScope> Scopes = new();
}
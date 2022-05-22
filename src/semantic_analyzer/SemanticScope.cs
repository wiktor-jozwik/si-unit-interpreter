using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticScope
{
    public string ContextFunctionName { get; set; }
    public Dictionary<string, IType> ContextFunctionParameters { get; set; }
    public Dictionary<string, IType> Variables { get; set; } = new();
}
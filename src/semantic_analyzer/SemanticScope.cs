using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticScope
{
    public Dictionary<string, IType> Variables { get; set; } = new();
}
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.semantic_analyzer;

public class SemanticScope
{
    public readonly Dictionary<string, IType> Variables = new();
}
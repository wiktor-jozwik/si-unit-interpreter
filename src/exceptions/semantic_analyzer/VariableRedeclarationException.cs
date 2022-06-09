namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class VariableRedeclarationException : Exception
{
    public VariableRedeclarationException(string name)
        : base($"'{name}' is already declared")
    {
    }
}
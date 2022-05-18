namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class VariableUndeclaredException: Exception
{
    public VariableUndeclaredException(string name)
        : base($"'{name}' is not defined")
    {
    }
}
namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class FunctionUndeclaredException : Exception
{
    public FunctionUndeclaredException(string name)
        : base($"'{name}' is not defined")
    {
    }
}
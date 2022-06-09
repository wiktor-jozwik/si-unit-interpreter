namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class UnitUndeclaredException : Exception
{
    public UnitUndeclaredException(string name)
        : base($"'{name}' is not defined")
    {
    }
}
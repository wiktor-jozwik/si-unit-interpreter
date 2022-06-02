namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class NotUniqueParametersNamesException: Exception
{
    public NotUniqueParametersNamesException(string functionName, string parameterName)
        : base($"'{functionName}' got two or more '{parameterName}' parameters")
    {
    }
}
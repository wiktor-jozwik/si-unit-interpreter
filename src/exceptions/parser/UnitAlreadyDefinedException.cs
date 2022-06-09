namespace si_unit_interpreter.exceptions.parser;

public class UnitAlreadyDefinedException: Exception
{
    public UnitAlreadyDefinedException(string name): 
        base($"'{name}' unit is already defined")
    {
    } 
}
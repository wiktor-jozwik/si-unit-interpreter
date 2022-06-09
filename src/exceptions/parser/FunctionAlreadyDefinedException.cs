namespace si_unit_interpreter.exceptions.parser;

public class FunctionAlreadyDefinedException: Exception
{
    public FunctionAlreadyDefinedException(string name): 
        base($"'{name}' function is already defined")
    {
    } 
}
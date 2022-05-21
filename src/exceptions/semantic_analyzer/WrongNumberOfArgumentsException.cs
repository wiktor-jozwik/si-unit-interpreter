namespace si_unit_interpreter.exceptions.semantic_analyzer;

public class WrongNumberOfArgumentsException: Exception
{
    public WrongNumberOfArgumentsException(string functionName, int expectedNumber, int receivedNumber) 
        :base($"'{functionName}' function was invoked with {receivedNumber} argument(s) but expected {expectedNumber} argument(s)") {}
}
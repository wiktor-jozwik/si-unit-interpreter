namespace si_unit_interpreter.interpreter;

public interface IBuiltInFunctionsProvider
{
    Dictionary<string, Func<dynamic, dynamic>> GetOneArgumentFunctions();
    Dictionary<string, Func<dynamic, dynamic, dynamic>> GetTwoArgumentFunctions();
}
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter;

public class BuiltInFunctionsProvider : IBuiltInFunctionsProvider
{
    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly Dictionary<string, Func<dynamic, dynamic, dynamic>> _twoArgumentFunctions;

    public BuiltInFunctionsProvider()
    {
        _oneArgumentFunctions = new Dictionary<string, Func<dynamic, dynamic>>
        {
            ["print"] = value =>
            {
                Console.WriteLine($"{value}");
                return null!;
            },
            ["sqrt"] = value => Math.Sqrt(value)
        };
        
        _twoArgumentFunctions = new Dictionary<string, Func<dynamic, dynamic, dynamic>>
        {
            ["power"] = (value, power) => Math.Pow(value, power),
        };
    }

    public Dictionary<string, Func<dynamic, dynamic>> GetOneArgumentFunctions()
    {
        return _oneArgumentFunctions;
    }

    public Dictionary<string, Func<dynamic, dynamic, dynamic>> GetTwoArgumentFunctions()
    {
        return _twoArgumentFunctions;
    }
}
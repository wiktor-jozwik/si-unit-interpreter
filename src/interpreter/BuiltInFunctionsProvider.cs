using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter;

public class BuiltInFunctionsProvider
{
    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly Dictionary<string, IType> _oneArgumentFunctionReturnTypes;

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
        };
        
        _oneArgumentFunctionReturnTypes = new Dictionary<string, IType>()
        {
            ["print"] = new VoidType()
        };
        //
        // _twoArgumentFunctions = new Dictionary<string, Func<dynamic, dynamic, dynamic>>
        // {
        //     ["power"] = (value, power) => Math.Pow(value, power),
        // };
    }

    public Dictionary<string, Func<dynamic, dynamic>> GetOneArgumentFunctions()
    {
        return _oneArgumentFunctions;
    }

    public Dictionary<string, Func<dynamic, dynamic, dynamic>> GetTwoArgumentFunctions()
    {
        return _twoArgumentFunctions;
    }

    public Dictionary<string, IType> GetOneArgumentFunctionReturnTypes()
    {
        return _oneArgumentFunctionReturnTypes;
    }
}
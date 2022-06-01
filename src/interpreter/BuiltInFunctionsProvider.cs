using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter;

public class BuiltInFunctionsProvider
{
    private readonly Dictionary<string, Func<dynamic, dynamic>> _oneArgumentFunctions;
    private readonly Dictionary<string, IType> _oneArgumentFunctionReturnTypes;

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
    }

    public Dictionary<string, Func<dynamic, dynamic>> GetOneArgumentFunctions()
    {
        return _oneArgumentFunctions;
    }

    public Dictionary<string, IType> GetOneArgumentFunctionReturnTypes()
    {
        return _oneArgumentFunctionReturnTypes;
    }
}
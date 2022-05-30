using si_unit_interpreter.interpreter;
using Xunit.Abstractions;

namespace si_unit_interpreter.spec;

public class TestBuiltInFunctionsProvider : IBuiltInFunctionsProvider
{
    private readonly Dictionary<string, Func<dynamic?, dynamic?>> _oneArgumentFunctions;

    public TestBuiltInFunctionsProvider(ITestOutputHelper testOutputHelper)
    {
        _oneArgumentFunctions = new Dictionary<string, Func<dynamic?, dynamic?>>
        {
            ["print"] = value =>
            {
                testOutputHelper.WriteLine($"{value}");
                return null!;
            },
        };
    }

    public Dictionary<string, Func<dynamic?, dynamic?>> GetOneArgumentFunctions()
    {
        return _oneArgumentFunctions;
    }

    public Dictionary<string, Func<dynamic?, dynamic?, dynamic?>> GetTwoArgumentFunctions()
    {
        return new Dictionary<string, Func<dynamic?, dynamic?, dynamic?>>();
    }
}
namespace si_unit_interpreter.input;

public class InputHandler : IInputHandler
{
    public IEnumerable<string> GetInput(string method, string pathOrCode)
    {
        var inputMethod = GetInputMethod(method);

        return inputMethod.GetLines(pathOrCode);
    }

    private static IInputProvider GetInputMethod(string method)
    {
        var result = int.TryParse(method, out var intMethod);

        if (!result)
        {
            throw new ArgumentException("Please provide number as method.");
        }

        return intMethod switch
        {
            0 => new PathInputProvider(),
            1 => new StringInputProvider(),
            _ => throw new ArgumentException("Please provide 0 or 1 as method.")
        };
    }
}

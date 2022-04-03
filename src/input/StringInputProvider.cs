namespace si_unit_interpreter.input;

public class StringInputProvider : IInputProvider
{
    public IEnumerable<string> GetLines(string stringCode)
    {
        return stringCode.Split("...");
    }
}
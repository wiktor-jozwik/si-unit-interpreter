namespace si_unit_interpreter.input;

public interface IInputProvider
{
    IEnumerable<string> GetLines(string str);
}
namespace si_unit_interpreter.input;

public interface IInputHandler
{
    IEnumerable<string> GetInput(string path);
}
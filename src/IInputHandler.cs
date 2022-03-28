namespace si_unit_interpreter;

public interface IInputHandler
{
    IEnumerable<string> GetData(string filePath);
}
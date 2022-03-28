namespace si_unit_compiler;

public interface IInputHandler
{
    IEnumerable<string> GetData(string filePath);
}
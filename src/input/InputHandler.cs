namespace si_unit_interpreter.input;

public class InputHandler : IInputHandler
{
    public IEnumerable<string> GetInput(string path)
    {
        try
        {
            var lines = File.ReadAllLines(path);
            return lines;
        }
        catch(Exception ex)
        {
            if (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                Console.WriteLine($"File or directory {path} was not found.");
            }
            throw;
        }
    }
}

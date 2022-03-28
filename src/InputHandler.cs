namespace si_unit_interpreter;

public class InputHandler : IInputHandler
{
    public IEnumerable<string> GetData(string inputArg)
    {
        try
        {
            var lines = File.ReadAllLines(inputArg);
            return lines;
        }
        catch(FileNotFoundException)
        {
            Console.WriteLine($"File {inputArg} was not found.");
            throw;
        }
    }
}
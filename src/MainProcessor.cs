using si_unit_interpreter.interpreter;

namespace si_unit_interpreter;

public class MainProcessor
{
    private readonly string[] _args;

    public MainProcessor(string[] args)
    {
        _args = args;
    }

    public void Run()
    {
        ValidateInput();
        var codePath = _args[0];
        StreamReader streamReader;
        try
        {
            streamReader = new StreamReader(codePath);  
        }
        catch(Exception ex)
        {
            if (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                Console.WriteLine($"File or directory {codePath} was not found.");
            }
            throw;
        }

        var interpreter = new Interpreter(streamReader);

        interpreter.Run();
    }

    private void ValidateInput()
    {
        if (_args.Length != 1)
        {
            throw new ArgumentException(
                "Please pass path to code as an argument."
            );
        }
    }
}
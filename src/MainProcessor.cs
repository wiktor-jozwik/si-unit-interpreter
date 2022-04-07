using si_unit_interpreter.input;

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
        var lines = new InputHandler().GetInput(_args[0]);

        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
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
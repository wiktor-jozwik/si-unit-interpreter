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

        using var sr = new StreamReader(_args[0]);

        while (!sr.EndOfStream)
        {
            Console.Write((char) sr.Read());
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
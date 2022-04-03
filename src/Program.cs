using si_unit_interpreter;
using si_unit_interpreter.input;

if (args.Length != 2)
{
    throw new ArgumentException(
        "Please pass two arguments:\n" +
        "First: 0 - read file, 1 - read given string\n" +
        "Second: file path or string with code"
        );
}
IInputHandler inputHandler = new InputHandler();

var lines = inputHandler.GetInput(args[0], args[1]);

foreach (var line in lines)
{
    Console.WriteLine(line);
}

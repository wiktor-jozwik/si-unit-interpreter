using si_unit_interpreter;

if (args.Length == 0)
{
    throw new ArgumentException("Please pass one argument with a file path.");
}
IInputHandler inputHandler = new InputHandler();

var inputArg = args[0]; 

var lines = inputHandler.GetData(inputArg);

foreach (var line in lines)
{
    Console.WriteLine(line);
}

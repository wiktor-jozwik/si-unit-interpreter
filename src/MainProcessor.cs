using si_unit_interpreter.interpreter;
using si_unit_interpreter.lexer;
using si_unit_interpreter.parser;

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
        try
        {
            TopLevel program; 
            using (var streamReader = new StreamReader(codePath))
            {
                var lexer = new CommentFilteredLexer(streamReader);
                var parser = new Parser(lexer);
                program = parser.Parse();

            }  
            var interpreter = new Interpreter(program);

            interpreter.Run();
        }
        catch(Exception ex)
        {
            if (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                Console.WriteLine($"File or directory {codePath} was not found.");
            }
            Console.WriteLine(ex.Message);
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
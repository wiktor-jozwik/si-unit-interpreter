using System.Reflection;
using Shouldly;
using Xunit;

namespace si_unit_interpreter.spec;

public class InterpreterAcceptanceTests
{
    [Fact]
    public void TestGForceExample()
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath);
        var path = Path.Combine(dirPath!, "code_examples", "g_force.txt");

        string[] args =
        {
            path
        };
        var mainProcessor = new MainProcessor(args);

        using var consoleOutput = new ConsoleOutput();
        mainProcessor.Run();
        consoleOutput.GetOutput().ShouldBe("Earth on g equals:\n9.791001719715803\n");
    }

    [Fact]
    public void TestElectricityExample()
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath);
        var path = Path.Combine(dirPath!, "code_examples", "electricity.txt");

        string[] args =
        {
            path
        };
        var mainProcessor = new MainProcessor(args);

        using var consoleOutput = new ConsoleOutput();
        mainProcessor.Run();
        consoleOutput.GetOutput().ShouldBe("1.7699774792265277\n");
    }

    public class ConsoleOutput : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}
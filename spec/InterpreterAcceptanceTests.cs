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
        consoleOutput.GetOutput().ShouldBe("Earth on g equals:\n9.791001719715803 [m*s^-2]\n");
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
        consoleOutput.GetOutput().ShouldBe("1.7699774792265277 [A]\n");
    }
    
    [Fact]
    public void TestEnergyKExample()
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath);
        var path = Path.Combine(dirPath!, "code_examples", "energy_k.txt");

        string[] args =
        {
            path
        };
        var mainProcessor = new MainProcessor(args);

        using var consoleOutput = new ConsoleOutput();
        mainProcessor.Run();
        consoleOutput.GetOutput().ShouldBe("Energy equals: \n20 [J]\n");
    }
}
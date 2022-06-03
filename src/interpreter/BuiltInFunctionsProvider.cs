using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;

namespace si_unit_interpreter.interpreter;

public class BuiltInFunctionsProvider
{
    private readonly Dictionary<string, FunctionStatement> _builtInFunctions = new();

    public BuiltInFunctionsProvider()
    {
        _builtInFunctions["print"] = new FunctionStatement(
            new List<Parameter>
            {
                new(
                    "printable",
                    new LiteralType()
                )
            },
            new VoidType(),
            new Block(
                new List<IStatement>
                {
                    new BuiltInFunction(
                        literalValue =>
                        {
                            var printoutString = $"{literalValue.Value}";
                            if (literalValue.UnitType != null && literalValue.UnitType.Units.Count != 0)
                            {
                                printoutString += $" {literalValue.UnitType.Format()}";
                            }

                            Console.WriteLine(printoutString);
                            return null!;
                        },
                        "printable"
                    )
                }
            )
        );
    }

    public Dictionary<string, FunctionStatement> GetBuiltInFunctions()
    {
        return _builtInFunctions;
    }
}
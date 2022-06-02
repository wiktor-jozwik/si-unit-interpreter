using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.parser;

namespace si_unit_interpreter.interpreter;

public class Interpreter
{
    private readonly TopLevel _program;

    public Interpreter(TopLevel program)
    {
        _program = program;
    }

    public void Run()
    {
        var builtInFunctionsProvider = new BuiltInFunctionsProvider();
        
        var semanticAnalyzerVisitor = new SemanticAnalyzerVisitor(builtInFunctionsProvider);
        var interpreterVisitor = new InterpreterVisitor("main", builtInFunctionsProvider);
        
        semanticAnalyzerVisitor.Visit(_program);
        interpreterVisitor.Visit(_program);
    }
}
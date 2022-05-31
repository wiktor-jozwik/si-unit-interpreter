using si_unit_interpreter.interpreter.interpreter;
using si_unit_interpreter.interpreter.semantic_analyzer;
using si_unit_interpreter.lexer;
using si_unit_interpreter.parser;

namespace si_unit_interpreter.interpreter;

public class Interpreter
{
    private readonly StreamReader _streamReader;

    public Interpreter(StreamReader streamReader)
    {
        _streamReader = streamReader;
    }

    public void Run()
    {
        var lexer = new CommentFilteredLexer(_streamReader);
        var parser = new Parser(lexer);
        
        var builtInFunctionsProvider = new BuiltInFunctionsProvider();
        
        var semanticAnalyzerVisitor = new SemanticAnalyzerVisitor(builtInFunctionsProvider);
        var interpreterVisitor = new InterpreterVisitor("main", builtInFunctionsProvider);
        
        var topLevelObject = parser.Parse();

        semanticAnalyzerVisitor.Visit(topLevelObject);
        interpreterVisitor.Visit(topLevelObject);
    }
}
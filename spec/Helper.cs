using System.Text;
using si_unit_interpreter.lexer;
using si_unit_interpreter.parser;
using si_unit_interpreter.parser.statement;

namespace si_unit_interpreter.spec;

public static class Helper
{
    public static StreamReader GetStreamReaderFromString(string text)
    {
        var byteArray = Encoding.UTF8.GetBytes(text);
        var stream = new MemoryStream(byteArray);
        
        return new StreamReader(stream);
    }
    
    public static Parser PrepareParser(string code)
    {
        var lexer = new CommentFilteredLexer(GetStreamReaderFromString(code));

        return new Parser(lexer);
    }

    public static Block GetStatementsFromMain(TopLevel topLevel)
    {
        return topLevel.Functions["main"].Statements;
    }

}
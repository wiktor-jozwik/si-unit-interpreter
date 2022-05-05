using System.Text;

namespace si_unit_interpreter.spec;

public static class Helper
{
    public static StreamReader GetStreamReaderFromString(string text)
    {
        var byteArray = Encoding.UTF8.GetBytes(text);
        var stream = new MemoryStream(byteArray);
        
        return new StreamReader(stream);
    }
}
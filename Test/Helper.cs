using System.Text;
using Antlr4.Runtime;
using Cast;

namespace Test;

public class Helper
{
    public static CastParser Setup(string? input = null)
    {
        StringBuilder sourceBuilder = new StringBuilder();
        string std = StdHelper.getStd();
        if (input != null)
        {
            sourceBuilder.Append(input);
        }
        else
        {
            sourceBuilder.Append(std);
        }

        string source = sourceBuilder.ToString();
        AntlrInputStream  inputStream = new AntlrInputStream(source);
        CastLexer  lexer = new CastLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        CastParser  parser = new CastParser(tokens);
        
        return parser;
    }
}
using System.Text;
using Antlr4.Runtime;
using Cast;

namespace Test;

public class Helper
{
    public static CastParser Setup(string input)
    {
        AntlrInputStream  inputStream = new AntlrInputStream(input);
        CastLexer  lexer = new CastLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        CastParser  parser = new CastParser(tokens);
        
        return parser;
    }
}
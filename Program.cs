using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Cast;
using Cast.Visitors;
using Cocona;

class Program
{
    private static String[] std = new[] { "types", "vectors", "spaces", "math", "opengl" };
    static void Main(string[] args)
    {
        var builder = CoconaApp.CreateBuilder(args);
        var app = builder.Build();

        app.AddCommand("compile", (string file, string? output) => Compile(file, output));
        app.Run();
    }

    static String getStd()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var file in std)
        {
            String path = "std/" + file + ".cst";
            String content = File.ReadAllText(path);
            builder.Append(content);
            builder.Append("\n");
        }
        builder.Append("\n");
        return builder.ToString();
    }

    static void Compile(String file, String? output)
    {
        String std =  getStd();
        if (!File.Exists(file))
        {
            Console.WriteLine($"File {file} not found");
            return;
        }

        String sourceFileContent = File.ReadAllText(file);
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append(std);
        sourceBuilder.Append(sourceFileContent);
        
        String source = sourceBuilder.ToString();
        AntlrInputStream  inputStream = new AntlrInputStream(source);
        CastLexer  lexer = new CastLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        CastParser  parser = new CastParser(tokens);

        try
        {
            CastParser.ProgramContext context = parser.program();
            SymbolPassVisitor symbolPassVisitor = new SymbolPassVisitor();
            symbolPassVisitor.Visit(context);
        
            SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(symbolPassVisitor);
            semanticPassVisitor.Visit(context);
        
            GlslPassVisitor glslPassVisitor = new GlslPassVisitor(semanticPassVisitor);
            String result = glslPassVisitor.Visit(context);

            String outFileName = file.Replace(".cst", ".glsl");
            if (output != null)
            {
                outFileName = output;
            }
        
            Console.WriteLine(source);
            File.WriteAllText(outFileName, result);
        } catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            Console.WriteLine($"Error: {e.StackTrace}");
        }
    }
}
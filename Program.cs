using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Cast;
using Cast.Visitors;
using Cocona;

class Program
{
    private static string[] std = new[] { "types", "vectors", "spaces", "math", "opengl" };
    static void Main(string[] args)
    {
        var builder = CoconaApp.CreateBuilder(args);
        var app = builder.Build();

        app.AddCommand("compile", (string path, string? output) => {
            if (Directory.Exists(path)) CompileFolder(path, output);
            if (File.Exists(path)) Compile(path, output);
        });
        app.Run();
    }

    static string getStd()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var file in std)
        {
            string path = "std/" + file + ".cst";
            string content = File.ReadAllText(path);
            builder.Append(content);
            builder.Append("\n");
        }
        builder.Append("\n");
        return builder.ToString();
    }

    static void CompileFolder(string folder, string output)
    {
        List<string> files = Directory.GetFiles(folder, "*.cst").ToList();
        foreach (string file in files) {
            Compile(file, null);
        }
    }

    static void Compile(string file, string? output)
    {
        string std =  getStd();
        if (!File.Exists(file))
        {
            Console.WriteLine($"File {file} not found");
            return;
        }

        string sourceFileContent = File.ReadAllText(file);
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append(std);
        sourceBuilder.Append(sourceFileContent);

        string source = sourceBuilder.ToString();
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
            string result = glslPassVisitor.Visit(context);

            string outFileName = file.Replace(".cst", ".glsl");
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
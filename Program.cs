using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Cast;
using Cast.Visitors;
using ConsoleAppFramework;

class Program
{
    static void Main(string[] args)
    {
        var app = ConsoleApp.Create();

        app.Add("compile", (string path, string? output) => {
            if (Directory.Exists(path)) CompileFolder(path, output);
            if (File.Exists(path)) Compile(path, output);
        });
        
        app.Run(args);
    }

    static void CompileFolder(string folder, string output)
    {
        List<string> files = Directory.GetFiles(folder, "*.*").ToList();
        foreach (string file in files) {
            Compile(file, output);
        }
    }

    static void Compile(string file, string? output)
    {
        string std = StdHelper.getStd();
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
                outFileName = output + Path.GetFileName(file);
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
using System.Reflection;
using System.Text;
using Antlr4.Runtime;
using Cast;
using Cast.listeners;
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
        
        app.Add("repl", () => {
            Repl();
        });
        
        app.Run(args);
    }

static void Repl()
{
    Console.WriteLine("Cast Shader Language REPL");
    Console.WriteLine("Type code, then 'run' to compile. Type 'exit' to quit.");
    Console.WriteLine("-----------------------------------------------------");

    // 1. Move StringBuilder OUTSIDE the loop so it remembers previous lines
    StringBuilder sb = new StringBuilder();

    string std = StdHelper.getStd();
    sb.AppendLine(std);

    while (true)
    {
        // Simple prompt: ">" for new command, "|" for continuation
        Console.Write(sb.Length == 0 ? "> " : "| ");

        string? line = Console.ReadLine();

        // 2. Handle Exit
        if (line == "exit" || line == null)
        {
            break;
        }

        if (line == "reset")
        {
            sb.Clear();
            sb.AppendLine(StdHelper.getStd());
            continue;
        }

        // 3. Handle Run
        if (line == "run")
        {
            string source = sb.ToString();
            
            if (string.IsNullOrWhiteSpace(source)) 
            {
                sb.Clear();
                continue;
            }

            try 
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Compiling...");
                Console.ResetColor();

                AntlrInputStream  inputStream = new AntlrInputStream(source);
                CastLexer  lexer = new CastLexer(inputStream);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                CastParser  parser = new CastParser(tokens);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new VisualErrorListener(sb.ToString()));

                var tree = parser.program(); 

                if (parser.NumberOfSyntaxErrors == 0)
                {
                    SymbolPassVisitor symbolPassVisitor = new SymbolPassVisitor();
                    symbolPassVisitor.Visit(tree);
                
                    SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(symbolPassVisitor);
                    semanticPassVisitor.Visit(tree);
                
                    GlslPassVisitor glslPassVisitor = new GlslPassVisitor(semanticPassVisitor);
                    string result = glslPassVisitor.Visit(tree);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Output: \n" + result); 
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Trace: {ex.StackTrace}");
            }
            finally 
            {
                Console.ResetColor();
                sb.Clear();
            }
        }
        else
        {
            sb.AppendLine(line);
        }
    }
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
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new VisualErrorListener(source));

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
            
            Console.WriteLine(result);
            File.WriteAllText(outFileName, result);
        } catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            // Console.WriteLine($"Error: {e.StackTrace}");
        }
    }
}
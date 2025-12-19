// See https://aka.ms/new-console-template for more information

using System.Text;
using Antlr4.Runtime;
using Cast;
using Cast.Visitors;

void printGlsl(string source)
{
    var inputStream = new AntlrInputStream(source);
    var castLexer = new CastLexer(inputStream);
    var tokenStream = new CommonTokenStream(castLexer);
    var parser = new CastParser(tokenStream);

    var program = parser.program();

    var symbolPassVisitor = new SymbolPassVisitor();
    symbolPassVisitor.Visit(program);

    var semanticPassVisitor = new SemanticPassVisitor(symbolPassVisitor);
    semanticPassVisitor.Visit(program);

    var glslVisitor = new GlslPassVisitor(semanticPassVisitor);
    Console.WriteLine(glslVisitor.Visit(program));
}

Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

var stdDir = AppDomain.CurrentDomain.BaseDirectory + "std";
var includeOrder = new[] { "spaces", "types", "vectors", "color", "texture", "math" };
StringBuilder std = new  StringBuilder();
foreach (var stdInclude in includeOrder)
{
    std.Append("\n");
    string p = File.ReadAllText(Path.Combine(stdDir, stdInclude + ".cst"));
    std.Append(p);
    std.Append("\n");
    var inputStream = new AntlrInputStream(std.ToString());
    var castLexer = new CastLexer(inputStream);
    var tokenStream = new CommonTokenStream(castLexer);
    var parser = new CastParser(tokenStream);

    var program = parser.program();

    var symbolPassVisitor = new SymbolPassVisitor();
    symbolPassVisitor.Visit(program);

    var semanticPassVisitor = new SemanticPassVisitor(symbolPassVisitor);
    semanticPassVisitor.Visit(program);
}

string combinedSource = std + "\n" + File.ReadAllText("arithmetic.cst");
printGlsl(combinedSource);
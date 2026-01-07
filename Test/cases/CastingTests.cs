using System.Text;
using Cast;
using Cast.Visitors;

namespace Test.cases;

public class CastingTests
{
    [Test]
    public void DownCast()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(StdHelper.getStd());
        builder.AppendLine("let a = vec3<Model>(1.0);");
        builder.AppendLine("let b : vec3 = a;"); // downcast

        string source = builder.ToString();
        SymbolPassVisitor symbolPass = new SymbolPassVisitor();
        
        CastParser parser = Helper.Setup(source);
        var programContext = parser.program();
        Assert.DoesNotThrow(() => symbolPass.Visit(programContext));

        SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(symbolPass);
        semanticPassVisitor.Visit(programContext);

        CastSymbol b = symbolPass.Scope.Lookup("b")!;
        Assert.That(b.SpaceName == "None");
    }
    
    [Test]
    public void UpCast()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(StdHelper.getStd());
        builder.AppendLine("let a = vec3(1.0);");
        builder.AppendLine("let b : vec3<Model> = a;"); // upcast
        
        string source = builder.ToString();
        CastParser parser = Helper.Setup(source);
        SymbolPassVisitor symbolPass = new SymbolPassVisitor();
        
        var programContext = parser.program();
        Assert.DoesNotThrow(() => symbolPass.Visit(programContext));

        SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(symbolPass);
        semanticPassVisitor.Visit(programContext);
        
        CastSymbol b = symbolPass.Scope.Lookup("b")!;
        Assert.That(b.SpaceName == "Model");
    }
}
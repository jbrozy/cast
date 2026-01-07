using System.Text;
using Cast;
using Cast.core.exceptions;
using Cast.Visitors;

namespace Test.cases;

public class CastingTests
{
    [Test]
    public void InvalidCast()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(StdHelper.getStd());
        builder.AppendLine("let a = vec3<Model>(1.0);");
        builder.AppendLine("let b : vec3<World> = a;"); // recast

        string source = builder.ToString();
        SymbolPassVisitor symbolPass = new SymbolPassVisitor();
        
        CastParser parser = Helper.Setup(source);
        var programContext = parser.program();
        symbolPass.Visit(programContext);

        SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(symbolPass);
        Assert.Throws<InvalidAssignmentException>(() => semanticPassVisitor.Visit(programContext));
    }
    
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
        Assert.That(b.SpaceName, Is.EqualTo("None"));
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
        Assert.That(b.SpaceName, Is.EqualTo("Model"));
    }
}
using System.Text;
using Cast;
using Cast.Visitors;

namespace Test;

public class Functions
{
    [Test]
    public void DeclaredTypedFunctionDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("declare fn (float) a(rhs: float) : float;");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol function = visitor.Visit(parser.typedFunctionDecl());
        Assert.That(function.CastType == CastType.FUNCTION);
        Assert.That(function.IsDeclaration);
    }
    
    [Test]
    public void TypedFunctionDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("fn (float) a(rhs: float) : float {}");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol function = visitor.Visit(parser.typedFunctionDecl());
        Assert.That(function.CastType == CastType.FUNCTION);
        Assert.That(!function.IsDeclaration);
    }
    
    [Test]
    public void TypedFunctionDeclNoBody()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("fn (float) a(rhs: float) : float;");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol function = visitor.Visit(parser.typedFunctionDecl());
        Assert.That(function.CastType == CastType.FUNCTION);
        Assert.That(!function.IsDeclaration);
    }
    
    [Test]
    public void FunctionDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("fn test() {}");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol function = visitor.Visit(parser.functionDecl());
        Assert.That(function.CastType == CastType.FUNCTION);
        Assert.That(!function.IsDeclaration);
    }
}
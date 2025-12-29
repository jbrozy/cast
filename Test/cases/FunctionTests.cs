using System.Text;
using Cast;
using Cast.Visitors;

namespace Test;

public class FunctionTests
{
    [Test]
    public void ConstructorFunctionDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(StdHelper.getStd());
        sourceBuilder.AppendLine("declare fn (vec3) (rhs: vec2) : vec3;");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        visitor.Visit(parser.program());
        CastSymbol? symbol = visitor.Scope.Lookup("vec3");
        CastSymbol? param = visitor.Scope.Lookup("vec2");
        
        Assert.That(symbol.CastType == CastType.STRUCT);

        List<CastSymbol> parameters = new List<CastSymbol>()
        {
            param
        };

        FunctionKey key = FunctionKey.Of("vec3", parameters);
        CastSymbol? fn = symbol.Functions[key];
        Assert.That(fn, Is.Not.EqualTo(null));
    }
    
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
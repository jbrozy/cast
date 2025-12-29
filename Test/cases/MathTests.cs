using System.Text;
using Cast;
using Cast.Visitors;

namespace Test;

public class MathTests
{
    [Test]
    public void Addition()
    {        
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append(StdHelper.getStd());
        sourceBuilder.Append("let a = 1 + 1;");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        Assert.DoesNotThrow(() => visitor.Visit(parser.program()));

        CastSymbol? variable = visitor.Scope.Lookup("a");
        Assert.That(variable != null);
    }
    
    [Test]
    public void VectorAdditionWithFloat()
    {        
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(StdHelper.getStd());
        sourceBuilder.AppendLine("let a = vec3(1.0) + 1.0;");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        Assert.DoesNotThrow(() => visitor.Visit(parser.program()));
        
        CastSymbol? variable = visitor.Scope.Lookup("a");
        Assert.That(variable.CastType == CastType.STRUCT);
        Assert.That(variable.StructName == "vec3");
    }
    
    [Test]
    public void VectorAdditionWithInt()
    {        
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(StdHelper.getStd());
        sourceBuilder.Append("let a = vec3(1.0) + 1;");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        Assert.DoesNotThrow(() => visitor.Visit(parser.program()));
        
        CastSymbol? variable = visitor.Scope.Lookup("a");
        Assert.That(variable.CastType == CastType.STRUCT);
        Assert.That(variable.StructName == "vec3");
    }
    
    [Test]
    public void VectorAdditionWithVector()
    {        
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine(StdHelper.getStd());
        sourceBuilder.Append("let a = vec3(1.0) + vec3(1.0);");
        
        string source = sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        Assert.DoesNotThrow(() => visitor.Visit(parser.program()));
        
        CastSymbol? variable = visitor.Scope.Lookup("a");
        Assert.That(variable.CastType == CastType.STRUCT);
        Assert.That(variable.StructName == "vec3");
    }
}
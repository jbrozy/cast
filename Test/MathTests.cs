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
        sourceBuilder.Append("1 + 1;");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        CastParser.StatementContext statement = parser.statement();
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol declAssign = visitor.Visit(statement);
        
        Assert.That(declAssign.CastType == CastType.INT);
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
        visitor.Visit(parser.program());
        parser = Helper.Setup(source);

        CastParser.StatementContext statement = parser.statement();
        CastSymbol declAssign = visitor.Visit(statement);
        
        Assert.That(declAssign.CastType == CastType.STRUCT && declAssign.StructName == "vec3");
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
        visitor.Visit(parser.program());
        parser = Helper.Setup(source);

        CastParser.StatementContext statement = parser.statement();
        CastSymbol declAssign = visitor.Visit(statement);
        
        Assert.That(declAssign.CastType == CastType.STRUCT && declAssign.StructName == "vec3");
    }
}
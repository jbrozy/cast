using System.Text;
using Cast;
using Cast.core.exceptions;
using Cast.Visitors;

namespace Test;

public class Tests
{
    [Test]
    public void AssignmentDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("let a : int = 1;\n");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        CastParser.StatementContext statement = parser.statement();
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol declAssign = visitor.Visit(statement);
        
        Assert.That(declAssign.CastType == CastType.INT);
    }
    
    [Test]
    public void Assignment()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("let a : int;\n");
        sourceBuilder.Append("a = 1;\n");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        CastParser.StatementContext statement = parser.statement();
        CastParser.AssignmentContext assignment = parser.assignment();
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        CastSymbol decl = visitor.Visit(statement);
        CastSymbol assign = visitor.Visit(assignment);
        
        Assert.That(decl.CastType == CastType.INT);
        Assert.That(assign.CastType == CastType.INT);
    }
    
    [Test]
    public void InvalidAssignment()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("let a : int;\n");
        sourceBuilder.Append("a = 1.0;\n");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        CastParser.StatementContext statement = parser.statement();
        CastParser.AssignmentContext assignment = parser.assignment();
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        visitor.Visit(statement);
        
        Exception exception = Assert.Throws<InvalidAssignmentException>(() => visitor.Visit(assignment));
        
        Console.WriteLine(exception.Message);
    }
    
    [Test]
    public void InvalidAssignmentDecl()
    {
        StringBuilder sourceBuilder = new StringBuilder();
        sourceBuilder.Append("let a : int = 1.0;\n");
        
        string source =  sourceBuilder.ToString();
        CastParser parser = Helper.Setup(source);
        CastParser.StatementContext statement = parser.statement();
        
        SymbolPassVisitor visitor = new SymbolPassVisitor();
        Exception exception = Assert.Throws<InvalidAssignmentException>(() => visitor.Visit(statement));
        
        Console.WriteLine(exception.Message);
    }
}
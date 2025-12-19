using System.Text;
using Antlr4.Runtime.Tree;

namespace Cast.Visitors;

public class GlslPassVisitor(SemanticPassVisitor semanticPassVisitor) : ICastVisitor<string>
{
    private Scope<CastSymbol> _scope = semanticPassVisitor.Scope;
    private IDictionary<IParseTree, CastSymbol> Nodes = semanticPassVisitor.Nodes;

    public string Visit(IParseTree tree)
    {
        return tree.Accept(this);
    }

    public string VisitChildren(IRuleNode node)
    {
        return "";
    }

    public string VisitTerminal(ITerminalNode node)
    {
        throw new NotImplementedException();
    }

    public string VisitErrorNode(IErrorNode node)
    {
        throw new NotImplementedException();
    }

    public string VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        StringBuilder uniforms = new StringBuilder();
        foreach (var uniform in context.uniformTypeDecl())
        {
            uniforms.Append(Visit(uniform));
        }
        return uniforms.ToString();
    }

    public string VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        return $"uniform {context.typeDecl().type.Text} {context.typeDecl().variable.Text};\n";
    }

    public string VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        var name = context.typeDecl().variable.Text;
        var value = Visit(context.value);

        var node = Nodes[context];
        if (node.CastType == CastType.STRUCT) return $"{node.StructName} {name} = {value}";

        var type = node.CastType.ToString().ToLower();
        return $"{type} {name} = {value};";
    }

    public string VisitVarAssign(CastParser.VarAssignContext context)
    {
        var name = context.varRef.Text;
        var value = Visit(context.value);
        var node = Nodes[context.value];
        return $"{name} = {value}";
    }

    public string VisitAddSub(CastParser.AddSubContext context)
    {
        if (context.op.Text == "+") return Visit(context.left) + " + " + Visit(context.right);

        return Visit(context.left) + " - " + Visit(context.right);
    }

    public string VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        string leftSide = Visit(context.expr);
        string memberName = context.name.Text;
        return $"{leftSide}.{memberName}";
    }

    public string VisitAtomExpr(CastParser.AtomExprContext context)
    {
        return Visit(context.atom());
    }

    public string VisitMethodCallExpr(CastParser.MethodCallExprContext context)
    {
        var methodName = context.name.Text;
        var expr = Visit(context.expr);
        var args = Visit(context.args);
        var fnArgs = new List<string> { expr };
        if (!string.IsNullOrEmpty(args)) fnArgs.Add(args);
        return $"{methodName}({string.Join(", ", fnArgs)})";
    }

    public string VisitMultDiv(CastParser.MultDivContext context)
    {
        if (context.op.Text == "*") return Visit(context.left) + " * " + Visit(context.right);

        return Visit(context.left) + " / " + Visit(context.right);
    }

    public string VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public string VisitExprStmt(CastParser.ExprStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitAssignStmt(CastParser.AssignStmtContext context)
    {
        return Visit(context.assignment()) + ";";
    }

    public string VisitStructDeclStmt(CastParser.StructDeclStmtContext context)
    {
        return Visit(context.structDecl());
    }

    public string VisitBlockStmt(CastParser.BlockStmtContext context)
    {
        return Visit(context.block());
    }

    public string VisitUniformStmtWrapper(CastParser.UniformStmtWrapperContext context)
    {
        return Visit(context.uniformStmt());
    }

    public string VisitSpaceDeclStmt(CastParser.SpaceDeclStmtContext context)
    {
        return "";
    }

    public string VisitReturnStmt(CastParser.ReturnStmtContext context)
    {
        return "return " + Visit(context.simpleExpression()) + ";";
    }

    public string VisitCallAtom(CastParser.CallAtomContext context)
    {
        return Visit(context.functionCall());
    }

    public string VisitParenAtom(CastParser.ParenAtomContext context)
    {
        return Visit(context.simpleExpression());
    }

    public string VisitVarAtom(CastParser.VarAtomContext context)
    {
        return context.ID().GetText();
    }

    public string VisitFloatAtom(CastParser.FloatAtomContext context)
    {
        return context.FLOAT().GetText();
    }

    public string VisitIntAtom(CastParser.IntAtomContext context)
    {
        return context.INT().GetText();
    }

    public string VisitProgram(CastParser.ProgramContext context)
    {
        var builder = new StringBuilder();
        foreach (var statementContext in context.statement()) builder.Append(Visit(statementContext));

        return builder.ToString();
    }

    public string VisitStatement(CastParser.StatementContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitPrimitiveDecl(CastParser.PrimitiveDeclContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitInOut(CastParser.InOutContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitImportStmt(CastParser.ImportStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitAssignment(CastParser.AssignmentContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitUniformStmt(CastParser.UniformStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        return $"uniform {context.type.Text} {context.name.Text};\n";
    }

    public string VisitBlock(CastParser.BlockContext context)
    {
        var builder = new StringBuilder();
        builder.Append(" {\n");
        foreach (var statementContext in context.statement())
        {
            builder.Append("    ");
            builder.Append(Visit(statementContext) + "\n");
        }

        builder.Append("}");
        return builder.ToString();
    }

    public string VisitStructDecl(CastParser.StructDeclContext context)
    {
        var node = Nodes[context];
        if (node.IsDeclaration) return "";
        var builder = new StringBuilder();
        builder.Append("\n");
        builder.Append($"struct {context.name.Text}");
        builder.Append(" {\n");
        foreach (var typeDeclContext in context.typeDecl())
        {
            builder.Append("    ");
            builder.Append(Visit(typeDeclContext) + "\n");
        }

        builder.Append("};\n");
        return builder.ToString();
    }

    public string VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        var fn = Nodes[context];
        if (fn.IsDeclaration) return "";

        var functionName = String.IsNullOrEmpty(fn.Identifier) ? fn.FunctionName : fn.Identifier;
        var builder = new StringBuilder();
        builder.Append("\n");
        var args = new List<string>();
        if (functionName == fn.Identifier)
        {
            args.Add($"{fn.FunctionName} self");
        }

        string type = fn.ReturnType.CastType.ToString().ToLower();
        if (type == "struct")
        {
            type = fn.ReturnType.StructName;
        }
        foreach (var arg in context.@params.typeDecl()) 
            args.Add($"{arg.type.Text} {arg.variable.Text}");
        builder.Append($"{type} {functionName}({string.Join(", ", args)})");
        builder.Append(Visit(context.block()));
        builder.Append("\n");
        return builder.ToString();
    }

    public string VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        return $"{context.name.Text}({Visit(context.args)})";
    }

    public string VisitArgList(CastParser.ArgListContext context)
    {
        return string.Join(", ", context.simpleExpression().Select(c => Visit(c)));
    }

    public string VisitParamList(CastParser.ParamListContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitTypeSpace(CastParser.TypeSpaceContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        var node = Nodes[context];
        return $"{context.type.Text} {context.variable.Text}";
    }

    public string VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitSimpleExpression(CastParser.SimpleExpressionContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitAtom(CastParser.AtomContext context)
    {
        return context.GetText();
    }
}
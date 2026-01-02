using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Antlr4.Runtime.Tree;

namespace Cast.Visitors;

public class GlslPassVisitor(SemanticPassVisitor semanticPassVisitor) : ICastVisitor<string>
{
    private Scope<CastSymbol> _scope = semanticPassVisitor.Scope;
    private IDictionary<IParseTree, CastSymbol> Nodes = semanticPassVisitor.Nodes;
    private static int indent = 0;

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

    public string VisitOutVarDecl(CastParser.OutVarDeclContext context)
    {
        string name = context.outTypeDecl().name.Text;
        string type = context.outTypeDecl().type.Text;
        
        Console.WriteLine($"out {type} {name};");
        
        return $"out {type} {name};";
    }

    public string VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        StringBuilder uniforms = new StringBuilder();
        uniforms.Append("\n");
        foreach (var uniform in context.uniformTypeDecl())
        {
            uniforms.Append(Visit(uniform));
        }
        return uniforms.ToString();
    }

    public string VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        return $"uniform {context.uniformTypeDecl().type.Text} {context.uniformTypeDecl().variable.Text};\n";
    }

    // public string VisitVarDecl(CastParser.VarDeclContext context)
    // {
    //     return "";
    // }

    public string VisitStageStmt(CastParser.StageStmtContext context)
    {
        return "";
    }

    public string VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string name = context.typeDecl().variable.Text;
        CastSymbol node = Nodes[context];
        if (node.IsDeclaration)
            return "";

        string type = node.CastType.ToString().ToLower();
        if (node.IsStruct())
        {
            type = node.StructName;
        }
        
        if (context.value != null)
        {
            string eval = Visit(context.value);
            return $"{type} {name} = {eval};\n";
        }
        
        return $"\n{type} {name};\n";
    }

    public string VisitVarDecl(CastParser.VarDeclContext context)
    {
        var node = Nodes[context];
        if (node.IsDeclaration)
        {
            return "";
        }
        string name = context.typeDecl().variable.Text;
        string type = context.typeDecl().type.Text;
        
        return $"\n{type} {name};\n";
    }

    public string VisitVarAssign(CastParser.VarAssignContext context)
    {
        var name = context.varRef.Text;
        var value = Visit(context.value);
        return $"{name} = {value};";
    }

    public string VisitInBlockDecl(CastParser.InBlockDeclContext context)
    {
        StringBuilder ins = new StringBuilder();
        ins.Append("\n");
        
        foreach (var inTypeDecl in context._members)
        {
            ins.Append(Visit(inTypeDecl));
            ins.Append("\n");
        }
        
        return ins.ToString();
    }

    public string VisitInVarDecl(CastParser.InVarDeclContext context)
    {
        string name = context.inTypeDecl().name.Text;
        string type = context.inTypeDecl().type.Text;
        
        return $"in {type} {name};";
    }

    public string VisitOutBlockDecl(CastParser.OutBlockDeclContext context)
    {
        StringBuilder outs = new StringBuilder();
        outs.Append("\n");
        foreach (var outTypeDecl in context._members)
        {
            outs.AppendLine(Visit(outTypeDecl));
        }
        return outs.ToString();
    }

    public string VisitAddSub(CastParser.AddSubContext context)
    {
        if (context.op.Text == "+") return Visit(context.left) + " + " + Visit(context.right);

        return Visit(context.left) + " - " + Visit(context.right);
    }

    public string VisitBooleanExpression(CastParser.BooleanExpressionContext context)
    {
        string left = Visit(context.left);
        string right = Visit(context.right);
        string symbol = context.op.Text;
        
        return $"{left} {symbol} {right}";
    }

    public string VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        string leftSide = Visit(context.expr);
        string memberName = context.name.Text;
        return $"{leftSide}.{memberName}";
    }

    public string VisitUnaryMinusExpr(CastParser.UnaryMinusExprContext context)
    {
        return $"-{Visit(context.expr)}";
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

    public string VisitConstructorFnDeclStmt(CastParser.ConstructorFnDeclStmtContext context)
    {
        return "";
    }

    public string VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public string VisitTypedFnDeclStmt(CastParser.TypedFnDeclStmtContext context)
    {
        return Visit(context.typedFunctionDecl());
    }

    public string VisitForDeclStmt(CastParser.ForDeclStmtContext context)
    {
        return Visit(context.forStmt());
    }

    public string VisitIfStmt(CastParser.IfStmtContext context)
    {
        StringBuilder ifStmt = new StringBuilder();
        ifStmt.Append("if (");
        ifStmt.Append(Visit(context.simpleExpression()));
        ifStmt.Append(")");
        ifStmt.Append(Visit(context.block()));
        ifStmt.AppendLine();
        return ifStmt.ToString();
    }

    public string VisitExprStmt(CastParser.ExprStmtContext context)
    {
        return "";
    }

    public string VisitOutStmtWrapper(CastParser.OutStmtWrapperContext context)
    {
        return Visit(context.outStmt());
    }

    public string VisitAssignStmt(CastParser.AssignStmtContext context)
    {
        return Visit(context.assignment());
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

    public string VisitInStmtWrapper(CastParser.InStmtWrapperContext context)
    {
        return Visit(context.inStmt());
    }

    public string VisitSpaceDeclStmt(CastParser.SpaceDeclStmtContext context)
    {
        return "";
    }

    public string VisitContinueStmt(CastParser.ContinueStmtContext context)
    {
        return "continue;\n";
    }

    public string VisitBreakStmt(CastParser.BreakStmtContext context)
    {
        return "break;\n";
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
        builder.Append($"#version 330\n");
        foreach (var statementContext in context.statement()) builder.Append(Visit(statementContext));

        return builder.ToString();
    }

    public string VisitStatement(CastParser.StatementContext context)
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

    public string VisitStage(CastParser.StageContext context)
    {
        return "";
    }

    public string VisitAssignment(CastParser.AssignmentContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitLocationDecl(CastParser.LocationDeclContext context)
    {
        return "";
    }

    public string VisitSwizzleDecl(CastParser.SwizzleDeclContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitInStmt(CastParser.InStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitOutStmt(CastParser.OutStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitUniformStmt(CastParser.UniformStmtContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitOutTypeDecl(CastParser.OutTypeDeclContext context)
    {
        StringBuilder output = new StringBuilder();
        if (context.locationDecl() != null)
        {
            string layoutId = context.locationDecl().layoutId.Text;
            output.Append($"layout(location = {layoutId}) ");
        }
        
        output.Append($"out {context.type.Text} {context.name.Text};");
        return output.ToString();
    }

    public string VisitInTypeDecl(CastParser.InTypeDeclContext context)
    {
        return $"in {context.type.Text} {context.name.Text};";
    }

    public string VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        return $"uniform {context.type.Text} {context.variable.Text};\n";
    }

    public string VisitForStmt(CastParser.ForStmtContext context)
    {
        StringBuilder builder = new StringBuilder();
        string var = context.var.Text;
        CastSymbol varType = _scope.Lookup(var);
        string varTypeText = varType.CastType.ToString().ToLower();

        string start = Visit(context.start);
        string end = Visit(context.end);

        string it = $"{var}++";
        if (context.it != null)
        {
            string op = context.it.Text == "inc" ? "+" : "-";
            it = $"{var} {op}= {context.inc.GetText()}";
        }

        string cond = context.cond.Text == "to" ? "<=" : "<";

        builder.Append($"for ({varTypeText} {var} = {start}; {var} {cond} {end}; {it})");
        builder.Append(Visit(context.block()));
        builder.AppendLine();
        return builder.ToString();
    }

    public string VisitBlock(CastParser.BlockContext context)
    {
        var builder = new StringBuilder();
        builder.Append(" {\n");

        indent += 1;
        string indentLevel = new string('\t', int.Max(0, indent));
        foreach (var statementContext in context.statement())
        {
            builder.Append(indentLevel + Visit(statementContext));
        }

        indent -= 1;
        indentLevel = new string('\t', int.Max(0, indent));
        builder.Append(indentLevel + "}");
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
        indent += 1;
        string indentLevel = new string('\t', int.Max(0, indent));
        foreach (var typeDeclContext in context.typeDecl())
        {
            builder.Append(indentLevel + Visit(typeDeclContext) + ";\n");
        }
        indent -= 1;
        indentLevel = new string('\t', int.Max(0, indent));
        builder.Append(indentLevel + "};\n");
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
        builder.Append($"{type} {functionName.ToLower()}({string.Join(", ", args)})");
        if (context.block() != null)
        {
            builder.Append(Visit(context.block()));
        }
        return builder.ToString();
    }

    public string VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        throw new NotImplementedException();
    }

    public string VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        var fn = Nodes[context];
        if (fn.IsDeclaration) return "";

        var functionName = String.IsNullOrEmpty(fn.Identifier) ? fn.FunctionName : fn.Identifier;
        var builder = new StringBuilder();
        builder.Append("\n");
        var args = new List<string>();
        if (context.typeVarName != null)
        {
            args.Add($"{context.typeFn.Text} {context.typeVarName.Text}");
        }

        string type = context.returnType.Text;
        foreach (var arg in context.@params.typeDecl()) 
            args.Add($"{arg.type.Text} {arg.variable.Text}");
        builder.Append($"{type} {functionName.ToLower()}({string.Join(", ", args)})");
        if (context.block() != null)
        {
            builder.Append(Visit(context.block()));
        }
        return builder.ToString();
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

    public string VisitTypeSpaceConversion(CastParser.TypeSpaceConversionContext context)
    {
        return "";
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

    public string VisitUnaryExpression(CastParser.UnaryExpressionContext context)
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
using Antlr4.Runtime.Tree;
using Cast.core.scope;
using Cast.core.symbols;

namespace Cast.Visitors.v2;

public class ResolutionPassVisitor : ICastVisitor<Symbol>
{
    private IScope CurrentScope;

    public ResolutionPassVisitor(DeclarationPassVisitor declarationPassVisitor)
    {
        CurrentScope =  declarationPassVisitor.CurrentScope;
    }
    
    public Symbol Visit(IParseTree tree)
    {
        return tree.Accept(this);
    }

    public Symbol VisitChildren(IRuleNode node)
    {
        return null;
    }

    public Symbol VisitTerminal(ITerminalNode node)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitErrorNode(IErrorNode node)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitIfStmt(CastParser.IfStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitStructDeclStmt(CastParser.StructDeclStmtContext context)
    {
        return Visit(context.structDecl());
    }

    public Symbol VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public Symbol VisitConstructorFnDeclStmt(CastParser.ConstructorFnDeclStmtContext context)
    {
        return null;
    }

    public Symbol VisitTypedFnDeclStmt(CastParser.TypedFnDeclStmtContext context)
    {
        return null;
    }

    public Symbol VisitForDeclStmt(CastParser.ForDeclStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBlockStmt(CastParser.BlockStmtContext context)
    {
        return Visit(context.block());
    }

    public Symbol VisitUniformStmtWrapper(CastParser.UniformStmtWrapperContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInStmtWrapper(CastParser.InStmtWrapperContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitOutStmtWrapper(CastParser.OutStmtWrapperContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitAssignStmt(CastParser.AssignStmtContext context)
    {
        return Visit(context.assignment());
    }

    public Symbol VisitSpaceDeclStmt(CastParser.SpaceDeclStmtContext context)
    {
        return Visit(context.spaceDecl());
    }

    public Symbol VisitContinueStmt(CastParser.ContinueStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBreakStmt(CastParser.BreakStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitReturnStmt(CastParser.ReturnStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitExprStmt(CastParser.ExprStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitStageStmt(CastParser.StageStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitVarDecl(CastParser.VarDeclContext context)
    {
        return null;
    }

    public Symbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string name = context.typeDecl().variable.Text;

        VariableSymbol? variable = CurrentScope.Resolve(name) as VariableSymbol;
        Symbol? expression = Visit(context.expression());
        
        if (context.typeDecl()?.type != null)
        {
            string type = context.typeDecl().type.Text;
            variable.TypeRef.Name = type;

            if (context.typeDecl().typeSpace() != null)
            {
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpace().spaceName.Text);
            }
            
            if (context.typeDecl().typeSpaceConversion() != null)
            {
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().From.Text);
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().To.Text);
            }
        }
        
        return null;
    }

    public Symbol VisitVarAssign(CastParser.VarAssignContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitVarExprAssign(CastParser.VarExprAssignContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInBlockDecl(CastParser.InBlockDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInVarDecl(CastParser.InVarDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitOutBlockDecl(CastParser.OutBlockDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitOutVarDecl(CastParser.OutVarDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitMethodCallExpr(CastParser.MethodCallExprContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUnaryMinusExpr(CastParser.UnaryMinusExprContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitMultDiv(CastParser.MultDivContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBooleanExpression(CastParser.BooleanExpressionContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitAddSub(CastParser.AddSubContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitAtomExpr(CastParser.AtomExprContext context)
    {
        return Visit(context.atom());
    }

    public Symbol VisitIntAtom(CastParser.IntAtomContext context)
    {
        return CurrentScope.Resolve("int");
    }

    public Symbol VisitFloatAtom(CastParser.FloatAtomContext context)
    {
        return CurrentScope.Resolve("float");
    }

    public Symbol VisitCallAtom(CastParser.CallAtomContext context)
    {
        string functionCall = context.functionCall().name.Text;

        return CurrentScope.Resolve(functionCall);
    }

    public Symbol VisitVarAtom(CastParser.VarAtomContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitParenAtom(CastParser.ParenAtomContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitProgram(CastParser.ProgramContext context)
    {
        foreach (var stmt in context.statement())
        {
            Visit(stmt);
        }

        return null;
    }

    public Symbol VisitStatement(CastParser.StatementContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInOut(CastParser.InOutContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitImportStmt(CastParser.ImportStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitStage(CastParser.StageContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitAssignment(CastParser.AssignmentContext context)
    {
        return null;
    }

    public Symbol VisitLocationDecl(CastParser.LocationDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitSwizzleDecl(CastParser.SwizzleDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInStmt(CastParser.InStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitOutStmt(CastParser.OutStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUniformStmt(CastParser.UniformStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitOutTypeDecl(CastParser.OutTypeDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInTypeDecl(CastParser.InTypeDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitForStmt(CastParser.ForStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBlock(CastParser.BlockContext context)
    {
        foreach (var statementContext in context.statement())
        {
            Visit(statementContext);
        }

        return null;
    }

    public Symbol VisitStructDecl(CastParser.StructDeclContext context)
    {
        return null;
    }

    public Symbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        string name = context.functionIdentifier().GetText();
        FunctionSymbol? functionSymbol = CurrentScope.Resolve(name) as FunctionSymbol;
        if (functionSymbol == null)
            throw new Exception($"Function {name} not found.");
        
        CurrentScope = new BaseScope(CurrentScope, $"Function {name}");
        foreach (var parameter in functionSymbol.Parameters)
        {
            CurrentScope.Define(parameter);
        }

        Visit(context.block());
        CurrentScope = CurrentScope.EnclosingScope;
        return null;
    }

    public Symbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitArgList(CastParser.ArgListContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitParamList(CastParser.ParamListContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitTypeSpace(CastParser.TypeSpaceContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitTypeSpaceConversion(CastParser.TypeSpaceConversionContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        return null;
    }

    public Symbol VisitUnaryExpression(CastParser.UnaryExpressionContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitExpression(CastParser.ExpressionContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitAtom(CastParser.AtomContext context)
    {
        throw new NotImplementedException();
    }
}
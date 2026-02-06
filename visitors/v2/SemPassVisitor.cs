using Antlr4.Runtime.Tree;
using Cast.core.scope;
using Cast.core.symbols;

namespace Cast.Visitors.v2;

public class SemPassVisitor : ICastVisitor<Symbol>
{
    private IScope CurrentScope;
    public SemPassVisitor(ResolutionPassVisitor resolutionPassVisitor)
    {
        CurrentScope = resolutionPassVisitor.CurrentScope;
    }
    
    public Symbol Visit(IParseTree tree)
    {
        return tree.Accept(this);
    }

    public Symbol VisitChildren(IRuleNode node)
    {
        return node.Accept(this);
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
        return null;
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
        return Visit(context.typedFunctionDecl());
    }

    public Symbol VisitForDeclStmt(CastParser.ForDeclStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBlockStmt(CastParser.BlockStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUniformStmtWrapper(CastParser.UniformStmtWrapperContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitInStmtWrapper(CastParser.InStmtWrapperContext context)
    {
        return Visit(context.inStmt());
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
        return null;
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
        return null;
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
        string name = context.typeDecl().variable.Text;
        VariableSymbol? variableSymbol = CurrentScope.Resolve(name) as VariableSymbol;

        Symbol type = CurrentScope.Resolve(variableSymbol.TypeRef.Name);
        variableSymbol.TypeRef.ResolvedType = type;

        foreach (var rawArg in variableSymbol.TypeRef.RawArgs)
        {
            Symbol? arg = CurrentScope.Resolve(rawArg);

            if (arg is not SpaceSymbol)
                throw new Exception($"Invalid Type");
            
            variableSymbol.TypeRef.ResolvedArgs.Add(arg);
        }
        return null;
    }

    public Symbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string name = context.typeDecl().variable.Text;
        VariableSymbol? variableSymbol = CurrentScope.Resolve(name) as VariableSymbol;
        Console.WriteLine(context.expression().GetText());
        Symbol expression = Visit(context.value);

        if (variableSymbol.TypeRef.ResolvedType is not null)
        {
            if (!variableSymbol.TypeRef.ResolvedType.Equals(expression))
            {
                throw new Exception($"Incompatible types");
            }
        }

        if (expression is FunctionSymbol functionSymbol)
        {
            variableSymbol.TypeRef = functionSymbol.ReturnTypeRef;
        }
        else
        {
            variableSymbol.TypeRef.ResolvedType = expression;
        }
        return null;
    }

    public Symbol VisitVarAssign(CastParser.VarAssignContext context)
    {
        Symbol expression = Visit(context.value);
        return null;
    }

    public Symbol VisitVarExprAssign(CastParser.VarExprAssignContext context)
    {
        return null;
    }

    public Symbol VisitInBlockDecl(CastParser.InBlockDeclContext context)
    {
        foreach (var inTypeDeclContext in context._members)
        {
            Visit(inTypeDeclContext);
        }
        return null;
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
        return Visit(context.functionCall());
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
        foreach (var statementContext in context.statement())
        {
            Visit(statementContext);
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
        string name = context.name.Text;
        string type = context.type.Text;
        VariableSymbol variableSymbol = CurrentScope.Resolve(name) as VariableSymbol;

        variableSymbol.TypeRef.ResolvedType = CurrentScope.Resolve(type);
        
        // params
        foreach (string arg in variableSymbol.TypeRef.RawArgs)
        {
            variableSymbol.TypeRef.ResolvedArgs.Add(CurrentScope.Resolve(arg) as Symbol);
        }
        return null;
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
        throw new NotImplementedException();
    }

    public Symbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        string functionName = context.functionIdentifier().functionName.Text;
        FunctionSymbol? functionSymbol = CurrentScope.Resolve(functionName) as FunctionSymbol;
        CurrentScope = functionSymbol;
        Visit(context.block());
        CurrentScope = functionSymbol.EnclosingScope;
        return null;
    }

    public Symbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        StructSymbol type = CurrentScope.Resolve(context.typeFn.Text) as StructSymbol;
        string functionName = context.functionIdentifier().functionName.Text;
        List<Symbol> overloads = new List<Symbol>();

        string? receiverTypeName = context.typeVarName?.Text;
        if (!string.IsNullOrEmpty(receiverTypeName))
        {
            overloads.Add(type);
        }
        
        foreach (var typeDeclContext in context.paramList().typeDecl())
        {
            overloads.Add(Visit(typeDeclContext));
        }
        
        FunctionSymbol? function = type.ResolveFunctionOverload(functionName, overloads) as FunctionSymbol;
        if (context.block() != null)
        {
            CurrentScope = function;
            Visit(context.block());
            CurrentScope = function.EnclosingScope;
        }

        return null;
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
        string functionName = context.name.Text;
        Symbol? symbol = CurrentScope.Resolve(functionName);
        
        var argTypes = new List<Symbol>();
        foreach (var expression in context.argList().expression())
        {
            Symbol sym = Visit(expression);
            argTypes.Add(sym);
        }

        if (symbol is StructSymbol structSymbol)
        {
            if (functionName == structSymbol.Name)
            {
                return structSymbol.ResolveConstructor(argTypes);
            }
            return structSymbol.ResolveFunctionOverload(functionName, argTypes);
        }
        
        if (symbol is FunctionSymbol functionSymbol)
        {
            return functionSymbol;
        }

        return null;
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
        string name = context.variable.Text;
        string type = context.type.Text;
        return CurrentScope.Resolve(type);
    }

    public Symbol VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitUnaryExpression(CastParser.UnaryExpressionContext context)
    {
        return Visit(context.GetChild(0));
    }

    public Symbol VisitExpression(CastParser.ExpressionContext context)
    {
        return Visit(context.GetChild(0));
    }

    public Symbol VisitAtom(CastParser.AtomContext context)
    {
        return Visit(context.GetChild(0));
    }
}
using System.Text;
using Antlr4.Runtime.Tree;
using Cast.core.scope;
using Cast.core.symbols;
using Cast.core.symbols.types;

namespace Cast.Visitors.v2;

public class ResolutionPassVisitor : ICastVisitor<Symbol>
{
    public IScope CurrentScope;

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
        return Visit(context.constructorFunctionDecl());
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
        return Visit(context.block());
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

        if (variableSymbol == null)
        {
            variableSymbol = new VariableSymbol()
            {
                Name = name,
            };
            CurrentScope.Define(variableSymbol);
        }

        if (context.typeDecl().type != null)
        {
            variableSymbol.TypeRef.Name = context.typeDecl().type.Text;

            if (context.typeDecl().typeSpace() != null)
            {
                variableSymbol.TypeRef.RawArgs.Add(context.typeDecl().typeSpace().spaceName.Text);
            }
            
            if (context.typeDecl().typeSpaceConversion() != null)
            {
                variableSymbol.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().From.Text);
                variableSymbol.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().To.Text);
            }
        }
        return null;
    }

    public Symbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string variableName = context.typeDecl().variable.Text;
        VariableSymbol? variable = CurrentScope.Resolve(variableName) as VariableSymbol;
        if (variable == null)
        {
            variable = new VariableSymbol()
            {
                Name = variableName
            };
        }
        
        // resolve type when explicitly set
        if (context.typeDecl()?.type != null)
        {
            string typeName = context.typeDecl().type.Text;
            variable.TypeRef.ResolvedType = CurrentScope.Resolve(typeName).Type;

            if (context.typeDecl()?.typeSpace() != null)
            {
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpace().spaceName.Text);
            }
            if (context.typeDecl()?.typeSpaceConversion() != null)
            {
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().From.Text);
                variable.TypeRef.RawArgs.Add(context.typeDecl().typeSpaceConversion().To.Text);
            }
        }

        if (CurrentScope.Resolve(variableName) == null)
        {
            CurrentScope.Define(variable);
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
        return null;
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
        string variableName = context.name.Text;
        string typeName = context.name.Text;

        VariableSymbol? variableSymbol = new VariableSymbol()
        {
            Name = variableName,
            Qualifier = StorageQualifier.Input,
            TypeRef = new TypeReference()
            {
                Name = typeName
            }
        };

        if (context.typeSpace() != null)
        {
            string space = context.typeSpace().spaceName.Text;
            variableSymbol.TypeRef.RawArgs.Add(space);
        }
        
        CurrentScope.Define(variableSymbol);
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
        string name = context.name.Text;
        StructTypeSymbol? symbol = CurrentScope.Resolve(name) as StructTypeSymbol;
        symbol.Type = CurrentScope.Resolve(symbol.Name).Type;
        symbol.EnclosingScope = CurrentScope;

        foreach (var member in symbol.Fields)
        {
            VariableSymbol? memberType = member.Value as VariableSymbol;
            memberType.Type = CurrentScope.Resolve(memberType.TypeRef.Name) as TypeSymbol;
            memberType.TypeRef.Name = memberType.Type.Name;
        }
        return null;
    }

    public Symbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        string functionName = context.functionIdentifier().functionName.Text;
        FunctionSymbol? symbol = CurrentScope.Resolve(functionName) as FunctionSymbol;
        symbol.EnclosingScope = CurrentScope;

        CurrentScope = symbol.Scope;
        Visit(context.block());
        CurrentScope = symbol.EnclosingScope;

        return null;
    }

    public Symbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        string typeFn = context.typeFn.Text;
        string functionName = context.functionIdentifier().functionName.Text;
        TypeSymbol? symbol = CurrentScope.Resolve(typeFn) as TypeSymbol;
        

        List<string> parameters = new List<string>();
        if (context.@params.typeDecl().Any())
        {
            // for typed functions without parameters
            if (!string.IsNullOrEmpty(context.typeVarName.Text))
            {
                parameters.Add(context.typeFn.Text);
            }
            foreach (var typeDeclContext in context.@params.typeDecl())
            {
                parameters.Add(typeDeclContext.type.Text);
            }
        }
        else
        {
            // for typed functions without parameters
            if (string.IsNullOrEmpty(context.typeVarName.Text))
            {
                parameters.Add(context.typeFn.Text);
            }
        }
        
        string paramSig = string.Join(", ", parameters);
        string signature = functionName + "(" + paramSig + ")";
        FunctionSymbol function = symbol.ResolveFunctionBySig(signature);
        if (function == null)
        {
            foreach (var fn in symbol.Functions)
            {
                if (fn.Name == functionName)
                    Console.WriteLine(fn.GetSignature());
            }
        }
        
        foreach (VariableSymbol parameter in function.Parameters)
        {
            parameter.Type = CurrentScope.Resolve(parameter.TypeRef.Name) as TypeSymbol;
        }

        if (context.block() != null)
        {
            Visit(context.block());
        }
        CurrentScope = function.EnclosingScope;
        return null;
    }

    public Symbol VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        string typeFn = context.typeFn.Text;
        
        StructTypeSymbol? symbol = CurrentScope.Resolve(typeFn) as StructTypeSymbol;
        var parameters = new List<Symbol>();
        foreach (var typeDecl in context.@params.typeDecl())
        {
            Symbol param = CurrentScope.Resolve(typeDecl.type.Text);
            parameters.Add(param);
        }

        FunctionSymbol constructor = symbol.ResolveConstructor(parameters) as FunctionSymbol;
        CurrentScope = constructor;
        if (context.block() != null)
        {
            Visit(context.block());
        }
        CurrentScope =  constructor.EnclosingScope;
        constructor.ReturnTypeRef.ResolvedType = CurrentScope.Resolve(constructor.ReturnTypeRef.Name).Type; 
        return null;
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
        return null;
    }

    public Symbol VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        string variableName = context.variable.Text;
        VariableSymbol? reference = CurrentScope.Resolve(variableName) as VariableSymbol;
        string typeName = context.type.Text;
        
        StructTypeSymbol? returnType = CurrentScope.Resolve(typeName) as StructTypeSymbol;
        if (returnType == null)
        {
            throw new Exception($"The type {typeName} does not exist.");
        }

        // Verify args
        foreach (var arg in reference.TypeRef.RawArgs)
        {
            var resolvedArg = CurrentScope.Resolve(arg);
            if (resolvedArg == null)
            {
                throw new Exception($"The type {typeName} does not exist.");
            }
            
            reference.TypeRef.ResolvedArgs.Add(resolvedArg);
        }

        reference.TypeRef.ResolvedType = returnType;

        return reference;
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
using System.Reflection;
using Antlr4.Runtime.Tree;
using Cast.core.scope;
using Cast.core.symbols;

namespace Cast.Visitors.v2;
using Cast.core;

public class DeclarationPassVisitor : ICastVisitor<Symbol>
{
    public IScope CurrentScope { get; set; }
    
    public DeclarationPassVisitor()
    {
        CurrentScope = new BaseScope(null, "root");
        
        // primitive types
        CurrentScope.Define(new StructSymbol(){Name = "float"});
        CurrentScope.Define(new StructSymbol(){Name = "int"});
        CurrentScope.Define(new StructSymbol(){Name = "bool"});
        CurrentScope.Define(new StructSymbol(){Name = "void"});
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
        return null;
    }

    public Symbol VisitStageStmt(CastParser.StageStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitVarDecl(CastParser.VarDeclContext context)
    {
        string name = context.typeDecl().variable.Text;

        // TODO: type
        VariableSymbol variableSymbol = new VariableSymbol()
        {
            Name = name,
            Qualifier = StorageQualifier.None,
            Offset = 0,
            Scope = CurrentScope,
            Initializer = null
        };
        
        CurrentScope.Define(variableSymbol);
        return null;
    }

    public Symbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string name = context.typeDecl().variable.Text;

        VariableSymbol variableSymbol = new VariableSymbol()
        {
            Name = name,
            Offset = 0,
            Qualifier = StorageQualifier.None,
            Scope = CurrentScope,
            Initializer = context.expression()
        };
        
        CurrentScope.Define(variableSymbol);
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
        foreach (var varDecl in context._members)
        {
            Visit(varDecl);
        }

        return null;
    }

    public Symbol VisitInVarDecl(CastParser.InVarDeclContext context)
    {
        string name = context.inTypeDecl().name.Text;

        VariableSymbol variableSymbol = new VariableSymbol()
        {
            Name = name,
            Qualifier = StorageQualifier.Input,
            Offset = 0,
            Scope = CurrentScope,
            Initializer = null
        };
        
        CurrentScope.Define(variableSymbol);
        return null;
    }

    public Symbol VisitOutBlockDecl(CastParser.OutBlockDeclContext context)
    {
        foreach (var outDecl in context._members)
        {
            Visit(outDecl);
        }
        
        return null;
    }

    public Symbol VisitOutVarDecl(CastParser.OutVarDeclContext context)
    {
        string name = context.outTypeDecl().name.Text;

        VariableSymbol variableSymbol = new VariableSymbol()
        {
            Name = name,
            Qualifier = StorageQualifier.Output,
            Offset = 0,
            Scope = CurrentScope,
            Initializer = null
        };
        
        CurrentScope.Define(variableSymbol);
        return null;
    }

    public Symbol VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        foreach (var uniformDecl in context._members)
        {
            Visit(uniformDecl);
        }
        
        return null;
    }

    public Symbol VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        string name = context.uniformTypeDecl().variable.Text;

        VariableSymbol variableSymbol = new VariableSymbol()
        {
            Name = name,
            Qualifier = StorageQualifier.Uniform,
            Offset = 0,
            Scope = CurrentScope,
            Initializer = null
        };
        
        CurrentScope.Define(variableSymbol);
        return null;
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
        throw new NotImplementedException();
    }

    public Symbol VisitIntAtom(CastParser.IntAtomContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitFloatAtom(CastParser.FloatAtomContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitCallAtom(CastParser.CallAtomContext context)
    {
        throw new NotImplementedException();
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
        string lhs = context.GetText();
        Console.WriteLine(lhs);

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
        return null;
    }

    public Symbol VisitInTypeDecl(CastParser.InTypeDeclContext context)
    {
        return null;
    }

    public Symbol VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        return null;
    }

    public Symbol VisitForStmt(CastParser.ForStmtContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitBlock(CastParser.BlockContext context)
    {
        throw new NotImplementedException();
    }

    public Symbol VisitStructDecl(CastParser.StructDeclContext context)
    {
        string name = context.name.Text;

        StructSymbol structSymbol = new StructSymbol()
        {
            Name = name,
            EnclosingScope = CurrentScope
        };

        CurrentScope = structSymbol;
        foreach (var typeDeclContext in context._members)
        {
            structSymbol.Fields.Add(typeDeclContext.variable.Text, Visit(typeDeclContext));
        }
        
        // default constructor
        FunctionSymbol functionSymbol = new FunctionSymbol()
        {
            Name = name,
            EnclosingScope = CurrentScope,
        };

        CurrentScope = functionSymbol;
        foreach (var typeDeclContext in context._members)
        {
            functionSymbol.Parameters.Add(Visit(typeDeclContext) as VariableSymbol);
        }
        CurrentScope = structSymbol.EnclosingScope;
        structSymbol.Constructors.Add(functionSymbol);
        CurrentScope.Define(structSymbol);

        return null;
    }

    public Symbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        Console.WriteLine(context.GetText());
        string name = context.functionIdentifier().functionName.Text;
        List<VariableSymbol> parameters = [];
        FunctionSymbol functionSymbol = new FunctionSymbol()
        {
            Name = name,
            Parameters = parameters,
            IsExternal = context.DECLARE() != null && context.DECLARE().GetText() == "DECLARE"
        };
        
        functionSymbol.EnclosingScope = CurrentScope;
        CurrentScope = functionSymbol;
        foreach (var @param in context.paramList().typeDecl())
        {
            Visit(@param);
        }
        CurrentScope = functionSymbol.EnclosingScope;
        functionSymbol.ReturnTypeRef.Name = context.returnType?.Text;
        CurrentScope.Define(functionSymbol);
        return null;
    }

    public Symbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        string returnTypeText = context.returnType?.Text;
        string type = context.typeFn.Text;
        string functionName = context.functionIdentifier().functionName.Text;
        if (string.IsNullOrEmpty(returnTypeText))
        {
            returnTypeText = "void";
        }

        if (functionName == "length")
        {
            Console.WriteLine("debug");
        }
        
        StructSymbol? parent = CurrentScope.Resolve(type) as StructSymbol;
        string? receiverTypeName = context.typeVarName?.Text;
        List<VariableSymbol> parameters = new List<VariableSymbol>();
        if (!string.IsNullOrEmpty(receiverTypeName))
        {
            VariableSymbol variableSymbol = new VariableSymbol()
            {
                Name = receiverTypeName,
                TypeRef = new TypeReference()
                {
                    Name = parent.Name
                }
            };
            parameters.Add(variableSymbol);
        }

        FunctionSymbol functionSymbol = new FunctionSymbol()
        {
            Name = functionName,
            Parameters = parameters,
            EnclosingScope = CurrentScope
        };

        CurrentScope = functionSymbol;
        foreach (var param in context.paramList().typeDecl())
        {
            VariableSymbol p = Visit(param) as VariableSymbol;
            CurrentScope.Define(p);
            parameters.Add(p);
        }
        CurrentScope = functionSymbol.EnclosingScope;
        parent.Functions.Add(functionSymbol);
        return null;
    }

    public Symbol VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        string typeName = context.typeFn.Text;
        StructSymbol? structSymbol = CurrentScope.Resolve(typeName) as StructSymbol;
        FunctionSymbol functionSymbol = new FunctionSymbol()
        {
            Name = typeName,
            Parameters = new List<VariableSymbol>(),
            EnclosingScope = CurrentScope
        };

        CurrentScope = functionSymbol;
        foreach (var typeDeclContext in context.paramList().typeDecl())
        {
            VariableSymbol s = Visit(typeDeclContext) as VariableSymbol;
            functionSymbol.Parameters.Add(s);
            functionSymbol.Define(s);
        }
        
        CurrentScope = structSymbol.EnclosingScope;
        Console.WriteLine(context.GetText());

        functionSymbol.ReturnTypeRef.Name = context.returnType.Text;
        structSymbol.Constructors.Add(functionSymbol);

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
        throw new NotImplementedException();
    }

    public Symbol VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        VariableSymbol variable = new VariableSymbol()
        {
            Name = context.variable.Text,
        };

        if (context.typeSpace() != null)
        {
            variable.TypeRef.RawArgs.Add(context.typeSpace().spaceName.Text);
        }
        
        if (context.typeSpaceConversion() != null)
        {
            variable.TypeRef.RawArgs.Add(context.typeSpaceConversion().From.Text);
            variable.TypeRef.RawArgs.Add(context.typeSpaceConversion().To.Text);
        }
            
        variable.TypeRef.Name = context.type.Text;
        return variable;
    }

    public Symbol VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        string name = context.spaceName.Text;
        SpaceSymbol spaceSymbol = new SpaceSymbol()
        {
            Name = name,
            Scope = null,
        };
        
        CurrentScope.Define(spaceSymbol);
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
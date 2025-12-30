using Antlr4.Runtime.Tree;

namespace Cast.Visitors;

public class SemanticPassVisitor : ICastVisitor<CastSymbol>
{
    public Dictionary<IParseTree, CastSymbol> Nodes { get; } = new();
    private Scope<CastSymbol> _scope = new();
    public Scope<CastSymbol> Scope => _scope;

    public SemanticPassVisitor(SymbolPassVisitor visitor)
    {
        _scope = visitor.Scope;
        Nodes = visitor.Nodes;
    }

    public CastSymbol Visit(IParseTree tree)
    {
        return tree.Accept(this);
    }

    public CastSymbol VisitChildren(IRuleNode node)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitTerminal(ITerminalNode node)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitErrorNode(IErrorNode node)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitOutVarDecl(CastParser.OutVarDeclContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        var node = Nodes[context];
        return node;
    }

    // public CastSymbol VisitVarDecl(CastParser.VarDeclContext context)
    // {
    //     return CastSymbol.Void;
    // }

    public CastSymbol VisitStageStmt(CastParser.StageStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        String variableName = context.variable.Text;
        CastSymbol lhs = CastSymbol.Void;
        CastSymbol rhs = CastSymbol.Void;

        // type based on lhs
        if (context.type != null)
        {
            lhs = _scope.Lookup(context.type.Text); // Visit(context.typ);
        }
        if (context.value != null)
        {
            // infer type based on rhs
            rhs = Visit(context.simpleExpression()).Clone();
        }

        if (rhs.CastType != CastType.VOID)
        {
            if (lhs.CastType != CastType.VOID && lhs.StructName != rhs.StructName)
            {
                throw new Exception($"Incompatible types: '{lhs.StructName}' and '{rhs.StructName}'");
            }

            if (lhs.CastType != CastType.VOID && lhs.SpaceName != rhs.SpaceName)
            {
                throw new Exception($"Incompatible spaces: '{lhs.SpaceName}' and '{rhs.SpaceName}'");
            }

            if (lhs.CastType == CastType.VOID)
            {
                lhs = rhs.Clone();
            }
        }

        if (_scope.Exists(variableName))
        {
            _scope.Assign(variableName, lhs);
        }
        else
        {
            _scope.Define(variableName, lhs);
        }

        lhs.IsDeclaration = context.DECLARE() != null && context.DECLARE().GetText() == "declare";
        Nodes[context] = lhs;
        return lhs;
    }

    public CastSymbol VisitVarDecl(CastParser.VarDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitVarAssign(CastParser.VarAssignContext context)
    {
        string varRef = context.varRef.Text;
        CastSymbol rhs = Visit(context.simpleExpression());

        if (!_scope.TryGetSymbol(varRef, out CastSymbol? lhs))
        {
            lhs = rhs.Clone();
            // throw new  Exception($"Variable '{varRef}' not found");
        }

        if (lhs?.CastType != rhs.CastType)
        {
            throw new  Exception($"Incompatible Types, left is '{lhs.CastType}' and right was '{rhs.CastType}'.");
        }

        rhs = rhs.Clone();

        // assign space inheritance from left to right
        if (!String.IsNullOrEmpty(lhs.SpaceName) && String.IsNullOrEmpty(rhs.SpaceName))
        {
            CastSymbol space = lhs.TypeSpace.Clone();
            rhs.TypeSpace = space;
            rhs.SpaceName = lhs.SpaceName;
        }
        
        if (!string.IsNullOrEmpty(lhs.SpaceName) && !String.IsNullOrEmpty(rhs.SpaceName))
        {
            if (lhs.SpaceName != rhs.SpaceName)
            {
                throw new Exception($"Incompatible space left: '{lhs.SpaceName}' and right: '{rhs.SpaceName}'");
            }
        }

        if (!_scope.Exists(varRef))
        {
            _scope.Define(varRef, rhs);
        }
        else
        {
            _scope.Assign(varRef, rhs);
        }
        
        Nodes[context] = rhs;
        return rhs;
    }

    public CastSymbol VisitInBlockDecl(CastParser.InBlockDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitInVarDecl(CastParser.InVarDeclContext context)
    {
        return Visit(context.inTypeDecl());
    }

    public CastSymbol VisitOutBlockDecl(CastParser.OutBlockDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitAddSub(CastParser.AddSubContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);

        if (!string.IsNullOrEmpty(left.SpaceName) && !string.IsNullOrEmpty(right.SpaceName))
        {
            if (left.SpaceName != right.SpaceName)
                throw new Exception($"Space mismatch: Cannot combine Space '{left.SpaceName}' with '{right.SpaceName}'");
        }

        string opName = context.op.Text == "+" ? "__add__" : "__sub__";
        var args = new List<CastSymbol> {  };
        
        if (left.IsStruct())
        {
            args.AddRange( left, right );
        }
        else
        {
            args.Add( right );
        }

        string targetTypeName = left.IsStruct() 
            ? left.StructName 
            : left.CastType.ToString().ToLower();

        CastSymbol typeSymbol = _scope.Lookup(targetTypeName);
        if (typeSymbol == null)
        {
            throw new Exception($"Type definition for '{targetTypeName}' not found.");
        }

        FunctionKey key = FunctionKey.Of(opName, args);
        if (!typeSymbol.Functions.TryGetValue(key, out CastSymbol fn))
        {
            string leftName = GetReadableName(left);
            string rightName = GetReadableName(right);
            throw new Exception($"Operator '{context.op.Text}' ({opName}) not defined for '{leftName}' and '{rightName}'");
        }

        Nodes[context] = fn.ReturnType;
        return fn.ReturnType;
    }

    public CastSymbol VisitBooleanExpression(CastParser.BooleanExpressionContext context)
    {
        return CastSymbol.Bool;
    }

    private string GetReadableName(CastSymbol sym)
    {
        return sym.IsStruct() ? sym.StructName : sym.CastType.ToString().ToLower();
    }
    
    public CastSymbol VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        var parent = Visit(context.expr);
        if (parent.CastType != CastType.STRUCT)
            throw new Exception($"Cannot access Member: '{context.name.Text}' on non-struct type '{parent.CastType}'");

        var memberName = context.name.Text;
        CastSymbol structSymbol = _scope.Lookup(parent.StructName);
        CastSymbol member = structSymbol.Fields[memberName];
        if (!string.IsNullOrEmpty(parent.SpaceName))
            if (member.CastType == CastType.STRUCT && string.IsNullOrEmpty(member.SpaceName))
                member.SpaceName = parent.SpaceName;
        Nodes[context] = member;
        return member;
    }

    public CastSymbol VisitAtomExpr(CastParser.AtomExprContext context)
    {
        return Visit(context.atom());
    }
    

    public CastSymbol VisitMethodCallExpr(CastParser.MethodCallExprContext context)
    {
        var leftExpr = Visit(context.expr);
        if (leftExpr.CastType == CastType.STRUCT)
        {
            var left = _scope.Lookup(leftExpr.StructName);
            if (left == null) throw new Exception($"No struct with Name {leftExpr.StructName} defined");

            var argList = new List<CastSymbol>();
            argList.Add(leftExpr);
            foreach (var arg in context.args.simpleExpression())
            {
                var result = Visit(arg);
                if (result.IsFunction)
                    argList.Add(result.ReturnType);
                else
                    argList.Add(result);
            }

            var functionKey = FunctionKey.Of(context.name.Text, argList);
            if (left.Functions.TryGetValue(functionKey, out var fn)) return Nodes[context] = fn.ReturnType;
            string argTypes = string.Join(", ", argList.Select(a => a.IsStruct() ? a.StructName : a.CastType.ToString()));
            throw new Exception($"Method '{context.name.Text}' not defined for types: ({argTypes}) on struct '{left.StructName}'");
        }

        Nodes[context] = leftExpr;
        return leftExpr.Clone();
    }

    public CastSymbol VisitMultDiv(CastParser.MultDivContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);

        if (!string.IsNullOrEmpty(left.SpaceName) && !string.IsNullOrEmpty(right.SpaceName))
        {
            if (left.SpaceName != right.SpaceName)
                throw new Exception($"Space mismatch: Cannot combine Space '{left.SpaceName}' with '{right.SpaceName}'");
        }

        string opName = context.op.Text == "*" ? "__mul__" : "__div__";
        var args = new List<CastSymbol> {};
        if (left.IsStruct())
        {
            args.AddRange( left, right );
        }
        else
        {
            args.Add( right );
        }

        string targetTypeName = left.IsStruct() 
            ? left.StructName 
            : left.CastType.ToString().ToLower();

        CastSymbol typeSymbol = _scope.Lookup(targetTypeName);
        if (typeSymbol == null)
        {
            throw new Exception($"Type definition for '{targetTypeName}' not found.");
        }

        typeSymbol = typeSymbol.Clone();

        FunctionKey key = FunctionKey.Of(opName, args);
        if (!typeSymbol.Functions.TryGetValue(key, out CastSymbol fn))
        {
            string leftName = GetReadableName(left);
            string rightName = GetReadableName(right);
            throw new Exception($"Operator '{context.op.Text}' ({opName}) not defined for '{leftName}' and '{rightName}'");
        }

        Nodes[context] = fn.ReturnType;
        return fn.ReturnType;
    }

    public CastSymbol VisitConstructorFnDeclStmt(CastParser.ConstructorFnDeclStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public CastSymbol VisitTypedFnDeclStmt(CastParser.TypedFnDeclStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitIfStmt(CastParser.IfStmtContext context)
    {
        _scope = new Scope<CastSymbol>(_scope);
        try
        {
            Visit(context.simpleExpression());
            if (context.block() != null)
            {
                Visit(context.block());
            }
        }
        finally
        {
            _scope = _scope.Parent;
        }
        
        return CastSymbol.Void;
    }

    public CastSymbol VisitExprStmt(CastParser.ExprStmtContext context)
    {
        return Visit(context.simpleExpression());
    }

    public CastSymbol VisitOutStmtWrapper(CastParser.OutStmtWrapperContext context)
    {
        return Visit(context.outStmt());
    }

    public CastSymbol VisitAssignStmt(CastParser.AssignStmtContext context)
    {
        var result = Visit(context.assignment());
        return Nodes[context] = result;
    }

    public CastSymbol VisitBlockStmt(CastParser.BlockStmtContext context)
    {
        foreach (var statement in context.children) Visit(statement);
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformStmtWrapper(CastParser.UniformStmtWrapperContext context)
    {
        return Visit(context.uniformStmt());
    }

    public CastSymbol VisitInStmtWrapper(CastParser.InStmtWrapperContext context)
    {
        return  Visit(context.inStmt());
    }

    public CastSymbol VisitSpaceDeclStmt(CastParser.SpaceDeclStmtContext context)
    {
        return Visit(context.spaceDecl());
    }

    public CastSymbol VisitReturnStmt(CastParser.ReturnStmtContext context)
    {
        var rhs = Visit(context.simpleExpression());
        Nodes[context] = rhs;
        rhs.IsReturn = true;

        return rhs;
    }

    public CastSymbol VisitStructDeclStmt(CastParser.StructDeclStmtContext context)
    {
        return Visit(context.structDecl());
    }

    public CastSymbol VisitCallAtom(CastParser.CallAtomContext context)
    {
        return Nodes[context] = Visit(context.functionCall());
    }

    public CastSymbol VisitParenAtom(CastParser.ParenAtomContext context)
    {
        return Nodes[context] = Visit(context.simpleExpression());
    }

    public CastSymbol VisitVarAtom(CastParser.VarAtomContext context)
    {
        if (_scope.Lookup(context.varRef.Text) != null)
        {
            return Nodes[context] = _scope.Lookup(context.varRef.Text).Clone();
        }
        
        throw new Exception($"Compilation  error: {context.varRef.Text} was not found in scope.");
    }

    public CastSymbol VisitFloatAtom(CastParser.FloatAtomContext context)
    {
        if (_scope.Lookup("float") != null)
        {
            Nodes[context] = _scope.Lookup("float").Clone();
            return Nodes[context];
        }

        throw new Exception("Compilation error: float was not found");
    }

    public CastSymbol VisitIntAtom(CastParser.IntAtomContext context)
    {
        if (_scope.Lookup("int") != null)
        {
            Nodes[context] = _scope.Lookup("int").Clone();
            return Nodes[context];
        }

        throw new Exception("Compilation error: int was not found");
    }

    public CastSymbol VisitProgram(CastParser.ProgramContext context)
    {
        foreach (var statementCtx in context.statement()) Visit(statementCtx);

        return CastSymbol.Void;
    }

    public CastSymbol VisitStatement(CastParser.StatementContext context)
    {
        return Visit(context.GetChild(0));
    }

    public CastSymbol VisitInOut(CastParser.InOutContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitImportStmt(CastParser.ImportStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitStage(CastParser.StageContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitAssignment(CastParser.AssignmentContext context)
    {
        var result = Visit(context);
        return Nodes[context] = result;
    }

    public CastSymbol VisitInStmt(CastParser.InStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitOutStmt(CastParser.OutStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitUniformStmt(CastParser.UniformStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitOutTypeDecl(CastParser.OutTypeDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitInTypeDecl(CastParser.InTypeDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitBlock(CastParser.BlockContext context)
    {
        foreach (var statementCtx in context.statement()) Visit(statementCtx);

        return CastSymbol.Void;
    }

    public CastSymbol VisitStructDecl(CastParser.StructDeclContext context)
    {
        var result = _scope.Lookup(context.name.Text).Clone();
        Nodes[context] = result;
        return result;
    }

    public CastSymbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        // _scope = new Scope<CastSymbol>(_scope);
        // var node = Nodes[context];
        // if (context.paramList() != null)
        //     if (context.paramList().typeDecl() != null)
        //         foreach (var type in context.paramList().typeDecl())
        //         {
        //             var result = Visit(type).Clone();
        //             _scope.Define(type.variable.Text, result);
        //         }

        // String? typeFn = context.typedFunctionDecl()?.typeFn?.Text;
        // if (typeFn != null)
        // {
        //     _scope.Define("self", _scope.Lookup(typeFn).Clone());
        // }

        // if (context.block() != null)
        // {
        //     Visit(context.block());
        // }
        // _scope = _scope.Parent;
        // if (context.functionIdentifier() != null && context.typedFunctionDecl() == null)
        // {
        //     Nodes[context] = _scope.Lookup(context.functionIdentifier().functionName.Text);
        // }
        // else
        // {
        //     string name = "";

        //     if (!String.IsNullOrEmpty(context.functionIdentifier()?.functionName?.Text))
        //     {
        //         name = context.functionIdentifier()?.functionName?.Text;
        //     }

        //     if (string.IsNullOrEmpty(name))
        //     {
        //         name = typeFn;
        //     }
        //     
        //     var fn = _scope.Lookup(typeFn);
        //     if (typeFn == "mat4")
        //     {
        //         Console.WriteLine("dbeug");
        //     }

        //     var paramTypes = new List<CastSymbol>();
        //     if (fn.CastType == CastType.STRUCT) paramTypes.Add(fn.Constructor.ReturnType);
        //     foreach (var @param in context.paramList().typeDecl())
        //     {
        //         var typeName = param.type.Text;
        //         var t = Types.ResolveType(typeName);
        //         paramTypes.Add(t);
        //     }
        //     
        //     var type = string.IsNullOrEmpty(context.returnType?.Text) ? "void" : context.returnType.Text;
        //     var returnType = Types.ResolveType(type);

        //     var key = FunctionKey.Of(name, paramTypes);
        //     var function = CastSymbol.Function(typeFn, paramTypes, returnType);
        //     function.IsDeclaration = context.DECLARE() != null && context.DECLARE().GetText() == "declare";
        //     fn.Functions[key] = function;
        //     var c = function.Clone();
        //     c.Identifier =  name;
        //     Nodes[context] = c;
        //     return c;
        // }

        return CastSymbol.Void;
    }

    public CastSymbol VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        var functionName = context.name.Text;
        var fn = _scope.Lookup(functionName).Clone();

        var args = new List<CastSymbol>();
        // if (fn.CastType == CastType.STRUCT) args.Add(fn);

        if (context.args.simpleExpression() != null)
            foreach (var arg in context.args.simpleExpression())
            {
                args.Add(Visit(arg));
            }

        // use typed functions functions
        if (fn.Functions.Count > 0)
        {
            var key = FunctionKey.Of(functionName, args);
            if (fn.Functions.TryGetValue(key, out var typedFunction))
            {
                fn = typedFunction.ReturnType; 
            }
            else
            {
                String @params = string.Join(", ", key.Types.Select(c => c.CastType.ToString()));
                throw new Exception($"Function {functionName} does not exist with Parameters: {@params}");
            }
        }

        if (context.typeSpace() != null)
        {
            fn.TypeSpace = _scope.Lookup(context.typeSpace().spaceName.Text).Clone();
            fn.SpaceName = context.typeSpace().spaceName.Text;
        }

        return Nodes[context] = fn.Clone();
    }

    public CastSymbol VisitArgList(CastParser.ArgListContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitParamList(CastParser.ParamListContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitTypeSpace(CastParser.TypeSpaceContext context)
    {
        return CastSymbol.Space(context.spaceName.Text);
    }

    public CastSymbol VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        var type = Types.ResolveType(context.type.Text);
        if (context.typeSpace() != null)
        {
            type.TypeSpace = _scope.Lookup(context.typeSpace().spaceName.Text).Clone();
            type.SpaceName = context.typeSpace().spaceName.Text;
        }
        
        // _scope.Define(context.variable.Text, type);
        Nodes[context] = type;
        return type;
    }

    public CastSymbol VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        return _scope.Lookup(context.spaceName.Text).Clone();
    }

    public CastSymbol VisitSimpleExpression(CastParser.SimpleExpressionContext context)
    {
        var result = Visit(context.GetChild(0));
        Nodes[context] = result.Clone();
        return Nodes[context];
    }

    public CastSymbol VisitAtom(CastParser.AtomContext context)
    {
        var result = Visit(context.GetChild(0));
        Nodes[context] = result.Clone();
        return Nodes[context];
    }
}
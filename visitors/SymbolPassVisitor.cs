using Antlr4.Runtime.Tree;

namespace Cast.Visitors;

public class SymbolPassVisitor : ICastVisitor<CastSymbol>
{
    private Scope<CastSymbol> _scope = new();
    private bool isInUniformBlock = false;

    public SymbolPassVisitor()
    {
        AddPrimivites();
    }

    public Dictionary<IParseTree, CastSymbol> Nodes { get; } = new();

    public Scope<CastSymbol> Scope => _scope;

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

    public CastSymbol VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        foreach (var uniform in context.uniformTypeDecl())
        {
            CastSymbol uniformTypeSymbol = Visit(uniform);
            _scope.Define(uniform.name.Text, uniformTypeSymbol);
            Nodes[uniform] = uniformTypeSymbol;
        }
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        CastSymbol typeSymbol = Visit(context.typeDecl()).Clone();
        typeSymbol.IsUniform = true;
        _scope.Assign(context.typeDecl().variable.Text, typeSymbol);
        return Nodes[context] = typeSymbol;
    }

    public CastSymbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        var varName = context.typeDecl().variable.Text;
        var @type = "";
        var typeSpace = "";

        if (context.typeDecl().typeSpace() != null)
        {
            typeSpace = context.typeDecl().typeSpace().spaceName.Text;
        }

        if (context.typeDecl() != null && context.typeDecl().type != null)
            if (!string.IsNullOrEmpty(context.typeDecl().type.Text))
            {
                @type = context.typeDecl().type.Text;
                if (context.typeDecl().typeSpace() != null) typeSpace = context.typeDecl().typeSpace().spaceName.Text;
            }

        CastSymbol? typeSymbol = null;
        if (!string.IsNullOrEmpty(@type))
        {
            typeSymbol = Types.ResolveType(@type);
            if (!string.IsNullOrEmpty(@typeSpace))
            {
                typeSymbol.TypeSpace = _scope.Lookup(@typeSpace);
                typeSymbol.SpaceName = typeSymbol.TypeSpace.SpaceName;
            }
        }
        else
        {
            typeSymbol = Visit(context.simpleExpression());
        }

        if (context.typeDecl() == null)
        {
            typeSymbol = Visit(context.simpleExpression());
        }

        _scope.Define(varName, typeSymbol);

        return typeSymbol;
    }

    public CastSymbol VisitVarAssign(CastParser.VarAssignContext context)
    {
        var varName = context.varRef.Text;
        var varSymbol = _scope.Lookup(varName);
        if (varSymbol == null)
            throw new Exception($"No value found for var {varName}");

        var rhs = Visit(context.value);
        if (!Equals(varSymbol.CastType, rhs.CastType))
            throw new Exception($"Unable to assign {rhs.CastType} to  {varSymbol.CastType}");

        return rhs;
    }

    public CastSymbol VisitAddSub(CastParser.AddSubContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);

        if (!left.Equals(right))
            throw new Exception($"Type mismatch between: {context.left.GetText()} and {context.right.GetText()}");
        Nodes[context] = left;
        return left;
    }

    public CastSymbol VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitAtomExpr(CastParser.AtomExprContext context)
    {
        var atom = Visit(context.atom());
        Nodes[context] = atom;
        return atom;
    }

    public CastSymbol VisitMethodCallExpr(CastParser.MethodCallExprContext context)
    {
        var leftExpr = Visit(context.expr);
        if (leftExpr.CastType == CastType.STRUCT)
        {
            var left = _scope.Lookup(leftExpr.StructName);
            if (left == null) throw new Exception($"No struct with Name {leftExpr.StructName} defined");

            var argTypes = new List<CastSymbol>();
            foreach (var arg in context.args.simpleExpression()) argTypes.Add(Visit(arg));

            var functionKey = FunctionKey.Of(context.name.Text, argTypes);
            if (!left.Functions.ContainsKey(functionKey))
                throw new Exception($"'{context.name.Text}' is not a function of {left.CastType}");

            var fn = left.Functions[functionKey];
            if (fn == null)
                throw new Exception($"Struct {leftExpr.StructName} doesn't have  function {context.name.Text}");

            return fn.ReturnType;
        }

        return CastSymbol.Void;
    }

    public CastSymbol VisitMultDiv(CastParser.MultDivContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);

        if (!left.Equals(right))
            throw new Exception($"Type mismatch between: {context.left.GetText()} and {context.right.GetText()}");
        Nodes[context] = left;
        return left;
    }

    public CastSymbol VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public CastSymbol VisitIfStmt(CastParser.IfStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitExprStmt(CastParser.ExprStmtContext context)
    {
        return Visit(context.simpleExpression());
    }

    public CastSymbol VisitAssignStmt(CastParser.AssignStmtContext context)
    {
        return Visit(context.assignment());
    }

    public CastSymbol VisitBlockStmt(CastParser.BlockStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformStmtWrapper(CastParser.UniformStmtWrapperContext context)
    {
        return Visit(context.uniformStmt());
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
        return Visit(context.functionCall());
    }

    public CastSymbol VisitParenAtom(CastParser.ParenAtomContext context)
    {
        return Visit(context.simpleExpression());
    }

    public CastSymbol VisitVarAtom(CastParser.VarAtomContext context)
    {
        var symbol = _scope.Lookup(context.varRef.Text);
        if (symbol == null) throw new Exception($"'{context.varRef.Text}' not found");

        return symbol;
    }

    public CastSymbol VisitFloatAtom(CastParser.FloatAtomContext context)
    {
        return CastSymbol.Float;
    }

    public CastSymbol VisitIntAtom(CastParser.IntAtomContext context)
    {
        return CastSymbol.Int;
    }

    public CastSymbol VisitProgram(CastParser.ProgramContext context)
    {
        foreach (var statementCtx in context.statement()) Visit(statementCtx);

        return CastSymbol.Void;
    }

    public CastSymbol VisitStatement(CastParser.StatementContext context)
    {
        return Visit(context);
    }

    public CastSymbol VisitPrimitiveDecl(CastParser.PrimitiveDeclContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitInOut(CastParser.InOutContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitImportStmt(CastParser.ImportStmtContext context)
    {
        throw new NotImplementedException();
    }

    public CastSymbol VisitAssignment(CastParser.AssignmentContext context)
    {
        return Visit(context);
    }

    public CastSymbol VisitUniformStmt(CastParser.UniformStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        var resolveType = Types.ResolveType(context.type.Text);
        resolveType.IsUniform = true;
        return Nodes[context] = resolveType;
    }

    public CastSymbol VisitBlock(CastParser.BlockContext context)
    {
        foreach (var statementCtx in context.statement()) Visit(statementCtx);

        return CastSymbol.Void;
    }

    public CastSymbol VisitStructDecl(CastParser.StructDeclContext context)
    {
        var structName = context.name.Text;
        if (_scope.TryGetSymbol(structName, out CastSymbol result))
        {
            Nodes[context] = result;
            return result;
        }

        var fields = new Dictionary<string, CastSymbol>();

        foreach (var type in context.typeDecl())
        {
            var t = Types.ResolveType(type.type.Text);
            Nodes[type] = t;
            fields.Add(type.variable.Text, t);
        }

        var @struct = CastSymbol.Struct(structName, fields);
        _scope.Define(structName, @struct);

        // constructor
        var members = fields.Values.ToList();
        var ctor = CastSymbol.Function(structName, members, @struct);
        @struct.Constructor = ctor;
        @struct.ReturnType = @struct;
        @struct.IsDeclaration = context.DECLARE() != null && context.DECLARE().Symbol.Text == "declare";
        
        FunctionKey key = FunctionKey.Of(structName, members);
        @struct.Functions.Add(key, @struct);

        Nodes[context] = @struct;
        return @struct;
    }

    public CastSymbol VisitFunctionDecl(CastParser.FunctionDeclContext context)
    {
        var paramTypes = new List<CastSymbol>();
        if (context.paramList() != null)
            foreach (var @param in context.paramList().typeDecl())
            {
                var typeName = param.type.Text;
                var t = Types.ResolveType(typeName);
                paramTypes.Add(t);
            }

        if (context.typedFunctionDecl() == null && context.functionIdentifier() != null)
        {
            var functionName = context.functionIdentifier().GetText();
            var type = string.IsNullOrEmpty(context.returnType?.Text) ? "void" : context.returnType.Text;
            var returnType = Types.ResolveType(type);

            var function = CastSymbol.Function(functionName, paramTypes, returnType);
            _scope.Define(functionName, function);
            return Nodes[context] = function;
        }

        // typed functions
        if (context.typedFunctionDecl()?.typeFn != null && context.functionIdentifier() == null)
        {
            var type = context.typedFunctionDecl().typeFn.Text;
            if (string.IsNullOrEmpty(type)) throw new Exception($"Type is missing in {context.GetText()}");

            var typeSymbol = _scope.Lookup(type);
            var returnTypeText = string.IsNullOrEmpty(type) ? "void" : type;
            var returnType = Types.ResolveType(returnTypeText);
            
            var pTypes = new List<CastSymbol>();
            pTypes.Add(returnType);
            pTypes.AddRange(paramTypes);

            var funcInfo = CastSymbol.Function(type, pTypes, returnType);
            funcInfo.ReturnType = typeSymbol.Constructor.ReturnType;
            typeSymbol.IsDeclaration = context.DECLARE() != null && 
                                       context.DECLARE().Symbol.Text == "declare";

            var key = FunctionKey.Of(type, paramTypes);
            typeSymbol.Functions.Add(key, funcInfo);

            Nodes[context] = funcInfo;
            return funcInfo;
        }

        return CastSymbol.Void;
    }

    public CastSymbol VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        return CastSymbol.ID(context.GetText());
    }

    public CastSymbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        var functionName = context.typeFn.Text;
        return Nodes[context] = CastSymbol.ID(functionName);
    }

    public CastSymbol VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        var functionName = context.name.Text;
        var fn = _scope.Lookup(functionName);
        if (fn.CastType == CastType.STRUCT) fn = fn.Constructor;

        if (context.typeSpace() != null)
        {
            fn.TypeSpace = Visit(context.typeSpace());
            fn.SpaceName = context.typeSpace()?.spaceName?.Text;
        }

        // if (fn.Parameters.Count != context.args.simpleExpression().Length)
        // {
        //     Console.WriteLine($"{fn.Parameters.Count}, {context.args.simpleExpression().Length}");
        //     throw new Exception($"Insufficient amount of Parameters given.");
        // }

        var providedArgs = context.args != null
            ? context.args.simpleExpression()
            : new CastParser.SimpleExpressionContext[0];

        var argCount = providedArgs.Length;
        for (var i = 0; i < argCount; i++)
        {
            var arg = Visit(providedArgs[i]);
            var expected = fn.Parameters[i];

            if (!arg.Equals(expected))
                throw new Exception($"Incorrect type in functionCall {functionName} at position: {i + 1}");
        }

        Nodes[context] = fn.ReturnType;
        return fn.ReturnType;
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
        if (String.IsNullOrEmpty(context.variable?.Text))
            return CastSymbol.Void;
        
        if (_scope.TryGetSymbol(context.variable?.Text, out CastSymbol type))
        {
            type.IsUniform = isInUniformBlock;
            return Nodes[context] = type;
        }
        
        var resolveType = Types.ResolveType(context.type.Text);
        resolveType.IsUniform = isInUniformBlock;
        _scope.Define(context.variable.Text, resolveType);
        Nodes[context] = resolveType;
        return resolveType;
    }

    public CastSymbol VisitSpaceDecl(CastParser.SpaceDeclContext context)
    {
        var spaceName = context.spaceName.Text;
        var space = CastSymbol.Space(spaceName);
        Nodes[context] = space;
        _scope.Define(spaceName, space);
        return space;
    }

    public CastSymbol VisitSimpleExpression(CastParser.SimpleExpressionContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitAtom(CastParser.AtomContext context)
    {
        return Nodes[context] = Visit(context);
    }

    private void AddPrimivites()
    {
        _scope.Define("float", CastSymbol.Float);
        _scope.Define("int", CastSymbol.Int);
        _scope.Define("bool", CastSymbol.Bool);
    }
}
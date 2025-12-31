using System.Data;
using Antlr4.Runtime.Tree;
using Cast.core.exceptions;

namespace Cast.Visitors;

public class SymbolPassVisitor : ICastVisitor<CastSymbol>
{
    private Scope<CastSymbol> _scope = new();
    private bool isInUniformBlock = false;
    private Stage _stage = Stage.NONE;

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

    public CastSymbol VisitOutVarDecl(CastParser.OutVarDeclContext context)
    {
        return Visit(context.outTypeDecl());
    }

    public CastSymbol VisitUniformBlockDecl(CastParser.UniformBlockDeclContext context)
    {
        foreach (var uniform in context.uniformTypeDecl())
        {
            CastSymbol uniformTypeSymbol = Visit(uniform);
            _scope.Define(uniform.variable.Text, uniformTypeSymbol);
            Nodes[uniform] = uniformTypeSymbol;
        }
        return CastSymbol.Void;
    }

    public CastSymbol VisitUniformVarDecl(CastParser.UniformVarDeclContext context)
    {
        CastSymbol typeSymbol = Visit(context.uniformTypeDecl()).Clone();
        typeSymbol.IsUniform = true;
        _scope.Assign(context.uniformTypeDecl().variable.Text, typeSymbol);
        return Nodes[context] = typeSymbol;
    }

    // public CastSymbol VisitVarDecl(CastParser.VarDeclContext context)
    // {
    //     String name = context.typeDecl().variable.Text;
    //     CastSymbol symbol = Types.ResolveType(name);

    //     if (context.typeDecl().typeSpace()?.spaceName != null)
    //     {
    //         CastSymbol space = _scope.Lookup(context.typeDecl().typeSpace().spaceName.Text);
    //         if (space == null)
    //             throw new Exception($"Space not found '{context.typeDecl().typeSpace().spaceName.Text}'");
    //         space = space.Clone();
    //         symbol.TypeSpace = space;
    //         symbol.SpaceName = context.typeDecl().typeSpace().spaceName.Text;
    //     }

    //     symbol.IsDeclaration = context.DECLARE() != null && context.DECLARE().GetText() == "declare";
    //     
    //     _scope.Define(name, symbol);
    //     return symbol;
    // }

    public CastSymbol VisitStageStmt(CastParser.StageStmtContext context)
    {
        string stageName = context.stageName.GetText();
        this._stage = Enum.TryParse(stageName.ToUpper(), out Stage stage) ? stage : Stage.NONE;
        return CastSymbol.Void;
    }

    public CastSymbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        CastSymbol lhs = CastSymbol.Void;
        CastSymbol rhs = CastSymbol.Void;

        string varName = context.typeDecl().variable.Text;

        // infer on lhs
        if (context.typeDecl() != null)
        {
            if (context.typeDecl().type != null)
            {
                lhs = _scope.Lookup(context.typeDecl().type.Text);// Visit(context.typeDecl());
                if (context.typeDecl()?.typeSpace()?.spaceName != null)
                {
                    string spaceName = context.typeDecl().typeSpace()?.spaceName.Text;
                    if (!_scope.TryGetSymbol(spaceName, out CastSymbol space))
                    {
                        throw new Exception($"Space '{spaceName}' not found");
                    }

                    lhs.SpaceName = spaceName;
                    lhs.TypeSpace = space;
                }
            }
        }

        if (context.value != null)
        {
            rhs = Visit(context.simpleExpression());
            if (lhs.CastType == CastType.VOID)
            {
                lhs = rhs.Clone();
            }

            if (lhs.CastType != rhs.CastType || lhs.StructName != rhs.StructName || lhs.SpaceName !=  rhs.SpaceName)
            {
                throw new InvalidAssignmentException(lhs, rhs);
            }
        }

        lhs = lhs.Clone();
        Nodes[context] = lhs;
        _scope.Define(varName, lhs);

        return lhs;
    }

    public CastSymbol VisitVarDecl(CastParser.VarDeclContext context)
    {
        string name = context.typeDecl().variable.Text;
        string type = context.typeDecl().type.Text;

        if (!_scope.TryGetSymbol(type, out CastSymbol? typeSymbol))
        {
            throw new Exception($"Type not found '{type}'");
        }

        CastSymbol clone = typeSymbol.Clone();
        if (context.typeDecl().typeSpace() != null)
        {
            if (!_scope.TryGetSymbol(context.typeDecl().typeSpace().spaceName.Text, out CastSymbol space))
            {
                throw new Exception($"Space not found '{context.typeDecl().typeSpace().spaceName.Text}'");
            }

            clone.TypeSpace = space;
            clone.SpaceName = context.typeDecl().typeSpace().spaceName.Text;
        }

        if (context.typeDecl().typeSpaceConversion() != null)
        {
            string from = context.typeDecl().typeSpaceConversion().From.Text;
            string to = context.typeDecl().typeSpaceConversion().To.Text;

            if (from == to)
                throw new InvalidConversionMatrixException(from, to);
            
            CastSymbol fromSpace = CastSymbol.Void;
            CastSymbol toSpace = CastSymbol.Void;

            if (!_scope.TryGetSymbol(from, out fromSpace))
            {
                throw new SpaceNotFoundException(from);
            }
            if (!_scope.TryGetSymbol(to, out toSpace))
            {
                throw new SpaceNotFoundException(to);
            }

            clone.Conversion = (fromSpace, toSpace)!;
        }

        clone.IsDeclaration = context.DECLARE() != null && context.DECLARE().GetText() == "declare";
        _scope.Define(name, clone);
        Nodes[context] = clone;
        
        return clone;
    }

    public CastSymbol VisitVarAssign(CastParser.VarAssignContext context)
    {
        string name = context.varRef.Text;
        CastSymbol? lhs = _scope.Lookup(name);
        if (lhs == null)
        {
            throw new VariableNotFoundException(name);
        }
        
        CastSymbol rhs = Visit(context.value);

        if (lhs.CastType != rhs.CastType || lhs.StructName != rhs.StructName || lhs.SpaceName != rhs.SpaceName)
        {
            throw new InvalidAssignmentException(lhs, rhs);
        }

        return lhs;
    }

    public CastSymbol VisitInBlockDecl(CastParser.InBlockDeclContext context)
    {
        foreach (var typeDecl in context._members)
        {
            Nodes[typeDecl] = Visit(typeDecl);
        }
        return CastSymbol.Void;
    }

    public CastSymbol VisitInVarDecl(CastParser.InVarDeclContext context)
    {
        return Visit(context.inTypeDecl());
    }

    public CastSymbol VisitOutBlockDecl(CastParser.OutBlockDeclContext context)
    {
        foreach (var typeDecl in context._members)
        {
            Visit(typeDecl);
        }
        
        return CastSymbol.Void;
    }

    public CastSymbol VisitAddSub(CastParser.AddSubContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);
        
        // left to right op
        string functionName = context.op.Text == "+" ? "__add__" : "__sub__";
        
        var parameters = new List<CastSymbol>();

        string lookupName = "";
        if (left.IsStruct())
        {
            parameters.Add(left);
            lookupName = left.StructName;
        }
        else
        {
            lookupName = left.CastType.ToString().ToLower();
        }
        parameters.Add(right);
        
        FunctionKey key = FunctionKey.Of(functionName, parameters);
        CastSymbol? lhsTypeInfo = _scope.Lookup(lookupName);
        CastSymbol function = lhsTypeInfo.Functions[key];
        CastSymbol result = function.ReturnType.Clone();
        
        Nodes[context] = result;
        return result;
    }

    public CastSymbol VisitBooleanExpression(CastParser.BooleanExpressionContext context)
    {
        return CastSymbol.Bool;
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
            if (left.IsStruct())
            {
                argTypes.Add(left);
            }
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
        
        // handle matrix conversions
        if (left.Conversion != null)
        {
            string leftConversionSpace = left.Conversion.Value.from.SpaceName;
            string endSpace = left.Conversion.Value.to.SpaceName;

            if (right.SpaceName != leftConversionSpace)
            {
                throw new InvalidSpaceConversionException(context.left.GetText(), leftConversionSpace);
            }
            
            left = left.Conversion.Value.to.Clone();
            left.StructName = right.StructName;
            left.TypeSpace = _scope.Lookup(endSpace);
            left.SpaceName = endSpace;
            left.CastType = right.CastType;
        }

        left = left.Clone();
        Nodes[context] = left;
        return left;
    }

    public CastSymbol VisitConstructorFnDeclStmt(CastParser.ConstructorFnDeclStmtContext context)
    {
        return Visit(context.constructorFunctionDecl());
    }

    public CastSymbol VisitFnDeclStmt(CastParser.FnDeclStmtContext context)
    {
        return Visit(context.functionDecl());
    }

    public CastSymbol VisitTypedFnDeclStmt(CastParser.TypedFnDeclStmtContext context)
    {
        return Visit(context.typedFunctionDecl());
    }

    public CastSymbol VisitIfStmt(CastParser.IfStmtContext context)
    {
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

    public CastSymbol VisitInStmtWrapper(CastParser.InStmtWrapperContext context)
    {
        return Visit(context.inStmt());
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
        string stageName = context.GetText();
        Console.WriteLine(stageName);
        return CastSymbol.Void;
    }

    public CastSymbol VisitAssignment(CastParser.AssignmentContext context)
    {
        return Visit(context);
    }

    public CastSymbol VisitLocationDecl(CastParser.LocationDeclContext context)
    {
        return CastSymbol.Void;
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
        return CastSymbol.Void;
    }

    public CastSymbol VisitOutTypeDecl(CastParser.OutTypeDeclContext context)
    {
        string name = context.name.Text;
        Console.WriteLine(context.GetText());
        CastSymbol? symbol = Types.ResolveType(context.type.Text);
        
        if (context.typeSpace()?.spaceName != null)
        {
            CastSymbol space = _scope.Lookup(context.typeSpace().spaceName.Text);
            
            if (space == null)
            {
                throw new Exception($"Space not found: '{context.typeSpace().spaceName.Text}'");
            }

            space = space.Clone();
            symbol.TypeSpace = space;
            symbol.SpaceName = context.typeSpace().spaceName.Text;
        }
        
        _scope.Define(name, symbol);
        Nodes[context] = symbol;
        return symbol;
    }

    public CastSymbol VisitInTypeDecl(CastParser.InTypeDeclContext context)
    {
        string name = context.name.Text;
        CastSymbol? symbol = _scope.Lookup(context.type.Text);
        if (symbol == null) symbol = Types.ResolveType(context.type.Text);
        _scope.Define(name, symbol);
        Nodes[context] = symbol;
        return symbol;
    }

    public CastSymbol VisitUniformTypeDecl(CastParser.UniformTypeDeclContext context)
    {
        var resolveType = Types.ResolveType(context.type.Text);
        CastSymbol? fromSpace = null;
        CastSymbol? toSpace = null;
        
        if (context.typeSpaceConversion() != null)
        {
            CastParser.TypeSpaceConversionContext ctx =  context.typeSpaceConversion();
            string from = ctx.From.Text;
            string to = ctx.To.Text;
            
            if (!_scope.TryGetSymbol(from, out fromSpace))
            {
                throw new SpaceNotFoundException(from);
            }
            if (!_scope.TryGetSymbol(to, out toSpace))
            {
                throw new SpaceNotFoundException(to);
            }

            fromSpace.SpaceName = from;
            toSpace.SpaceName = to;
        }

        resolveType.Conversion = (fromSpace, toSpace)!;
        
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
        Console.WriteLine(context.GetText());
        string returnTypeText = context.returnType?.Text ?? "void";
        CastSymbol? returnType = _scope.Lookup(returnTypeText);
        string functionName = context.functionIdentifier().functionName.Text;
        
        var paramTypes = new List<CastSymbol>();
        if (context.paramList() != null)
        {
            foreach (var @param in context.paramList().typeDecl())
            {
                var typeName = param.type.Text;
                var t = Types.ResolveType(typeName);
                paramTypes.Add(t);
            }
        }
        
        CastSymbol function = CastSymbol.Function(functionName, paramTypes, returnType);
        function.IsDeclaration =  context.DECLARE() != null && context.DECLARE().Symbol.Text == "declare";
        _scope.Define(functionName, function);
        Nodes[context] = function;
        return function;
    }

    public CastSymbol VisitConstructorFunctionDecl(CastParser.ConstructorFunctionDeclContext context)
    {
        string constr = context.typeFn.Text;
        var paramTypes = new List<CastSymbol>();
        if (context.paramList() != null)
        {
            foreach (var @param in context.paramList().typeDecl())
            {
                var typeName = param.type.Text;
                var t = Types.ResolveType(typeName);
                paramTypes.Add(t);
            }
        }
        
        string returnTypeText = context.returnType?.Text ?? "void";
        CastSymbol? returnType = _scope.Lookup(returnTypeText);
        
        if(returnType == null)
            throw new Exception($"Return Type not found: {returnTypeText}");
        
        CastSymbol function = CastSymbol.Function(constr, paramTypes, returnType);
        FunctionKey key =  FunctionKey.Of(constr, paramTypes);

        if (!_scope.TryGetSymbol(constr, out CastSymbol type))
        {
            throw new TypeNotFoundException(constr);
        }
        
        type.Functions.Add(key, function);
        return function;
    }

    public CastSymbol VisitFunctionIdentifier(CastParser.FunctionIdentifierContext context)
    {
        return CastSymbol.ID(context.GetText());
    }

    public CastSymbol VisitTypedFunctionDecl(CastParser.TypedFunctionDeclContext context)
    {
        string returnTypeText = context.returnType?.Text ?? "void";
        CastSymbol? returnType = _scope.Lookup(returnTypeText);
        string typeFnText = context.typeFn.Text;

        string functionName = context.functionIdentifier() == null ? 
            typeFnText : 
            context.functionIdentifier().functionName.Text;
        
        var paramTypes = new List<CastSymbol>();
        if (context.typeVarName != null)
        {
            if (Types.ResolveType(typeFnText).IsStruct())
            {
                paramTypes.Add(Types.ResolveType(typeFnText));
            }
        }
        
        if (context.paramList() != null)
        {
            foreach (var @param in context.paramList().typeDecl())
            {
                var typeName = param.type.Text;
                var t = Types.ResolveType(typeName);
                paramTypes.Add(t);
            }
        }
        
        CastSymbol function = CastSymbol.Function(functionName, paramTypes, returnType);
        function.IsDeclaration =  context.DECLARE() != null && context.DECLARE().Symbol.Text == "declare";

        if (function.IsDeclaration)
        {
            CastSymbol? type = _scope.Lookup(typeFnText);
            FunctionKey key = FunctionKey.Of(functionName, paramTypes);
            type.Functions.TryAdd(key, function);
        }
        else
        {
            _scope.Define(functionName, function);
        }
        
        return Nodes[context] = function;
    }

    public CastSymbol VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        var functionName = context.name.Text;
        var fn = _scope.Lookup(functionName);
        if (fn == null)
        {
            throw new Exception($"Function {functionName} could not be found");
        }
        if (fn.CastType == CastType.STRUCT) fn = fn.Constructor;
        
        if (context.typeSpace() != null)
        {
            fn.TypeSpace = Visit(context.typeSpace());
            fn.SpaceName = context.typeSpace()?.spaceName?.Text;
        }

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

    public CastSymbol VisitTypeSpaceConversion(CastParser.TypeSpaceConversionContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitTypeDecl(CastParser.TypeDeclContext context)
    {
        if (String.IsNullOrEmpty(context.variable?.Text))
            return CastSymbol.Void;

        CastSymbol? fromSpace = null;
        CastSymbol? toSpace = null;
        
        if (context.typeSpaceConversion() != null)
        {
            CastParser.TypeSpaceConversionContext ctx =  context.typeSpaceConversion();
            string from = ctx.From.Text;
            string to = ctx.To.Text;
            
            if (!_scope.TryGetSymbol(from, out fromSpace))
            {
                throw new SpaceNotFoundException(from);
            }
            if (!_scope.TryGetSymbol(to, out fromSpace))
            {
                throw new SpaceNotFoundException(to);
            }
        }
        
        if (_scope.TryGetSymbol(context.variable?.Text, out CastSymbol type))
        {
            type.IsUniform = isInUniformBlock;
            return Nodes[context] = type;
        }

        var resolveType = Types.ResolveType(context.type.Text);
        resolveType.IsUniform = isInUniformBlock;
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
        _scope.Define("void", CastSymbol.Void);
    }
}
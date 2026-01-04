using Antlr4.Runtime.Tree;
using Cast.core.exceptions;

namespace Cast.Visitors;

public class SemanticPassVisitor : ICastVisitor<CastSymbol>
{
    public Dictionary<IParseTree, CastSymbol> Nodes { get; } = new();
    private Scope<CastSymbol> _scope = new();
    public Scope<CastSymbol> Scope => _scope;

    private bool _inLoop = false;

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

    public CastSymbol VisitStageStmt(CastParser.StageStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitVarDeclAssign(CastParser.VarDeclAssignContext context)
    {
        string variableName = context.typeDecl().variable.Text;
        CastSymbol lhs = CastSymbol.Void;
        CastSymbol rhs = CastSymbol.Void;

        // type based on lhs
        if (context.typeDecl() != null)
        {
            if (context.typeDecl().type != null)
            {
                string typeDeclName = context.typeDecl().type.Text;
                lhs = _scope.Lookup(typeDeclName);

                if (context.typeDecl().typeSpace() != null)
                {
                    string spaceName = context.typeDecl().typeSpace().spaceName.Text;
                    if (!_scope.TryGetSymbol(spaceName, out CastSymbol? space))
                    {
                        throw new SpaceNotFoundException(spaceName);
                    }

                    lhs.TypeSpace = space.Clone();
                    lhs.SpaceName = spaceName;
                }
            }
        }
        
        if (context.value != null)
        {
            rhs = Visit(context.simpleExpression()).Clone();
        }
        if (lhs.CastType == CastType.VOID)
        {
            lhs = rhs.Clone();
        }
        if (lhs.CastType != rhs.CastType || lhs.StructName != rhs.StructName || lhs.SpaceName !=  rhs.SpaceName)
        {
            throw new InvalidAssignmentException(context, lhs, rhs);
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

        if (lhs.SpaceName == "None")
        {
            lhs.SpaceName = rhs.SpaceName;
        }
        
        if (rhs.SpaceName == "None")
        {
            rhs.SpaceName = lhs.SpaceName;
        }
        
        if (lhs.SpaceName != rhs.SpaceName)
        {
            throw new Exception($"Incompatible space left: '{lhs.SpaceName}' and right: '{rhs.SpaceName}'");
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
        
        args.AddRange( left, right );

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
    
    private int GetSwizzleIndex(char c)
    {
        return c switch
        {
            'x' or 'r' or 's' => 0,
            'y' or 'g' or 't' => 1,
            'z' or 'b' or 'p' => 2,
            'w' or 'a' or 'q' => 3,
            _ => -1
        };
    }
    
    public CastSymbol VisitMemberAccessExpr(CastParser.MemberAccessExprContext context)
    {
        var parent = Visit(context.expr);
        
        if (parent.CastType != CastType.STRUCT)
            throw new Exception($"Cannot access Member: '{context.name.Text}' on non-struct type '{parent.CastType}'");

        var memberName = context.name.Text;
        CastSymbol? structSymbol = _scope.Lookup(parent.StructName);
        if (structSymbol == null)
        {
            throw new TypeNotFoundException(parent.StructName);
        }
        
        // swizzling currently only works for vector types
        // I am not sure if this would work for other types
        if (parent.ReturnType.AllowSwizzle || structSymbol.AllowSwizzle)
        {
            var match = System.Text.RegularExpressions.Regex.Match(parent.StructName, @"^([a-z]?vec)([234])$");
            if (!match.Success)
            {
                throw new Exception($"Error while swizzling on struct '{parent.StructName}'");
            }
            string prefix =  match.Groups[1].Value;
            int dim =  int.Parse(match.Groups[2].Value);

            int swizzleLength = memberName.Length;
            if (swizzleLength < 1 ||  swizzleLength > 4)
            {
                throw new Exception($"Unable to swizzle with length: {swizzleLength} on Struct '{parent.StructName}'");
            }

            foreach (char c in memberName)
            {
                int index = GetSwizzleIndex(c);
                if (index == -1)
                {
                    throw new Exception($"Swizzle '{c}' not found on Struct '{parent.StructName}'");
                }
                if (index >= dim)
                {
                    throw new Exception($"Swizzling out of bounds on  Struct '{parent.StructName}'");
                }
            }
            
            string name = prefix + swizzleLength;
            if (!_scope.TryGetSymbol(name, out CastSymbol swizzleType))
            {
                throw new Exception($"Unable to swizzle on Struct '{parent.StructName}', outgoing struct not found in scope.");
            }

            return swizzleType.Clone();
        }

        CastSymbol member = structSymbol.Fields[memberName];
        if (!string.IsNullOrEmpty(parent.SpaceName))
            if (member.CastType == CastType.STRUCT && string.IsNullOrEmpty(member.SpaceName))
                member.SpaceName = parent.SpaceName;
        Nodes[context] = member;
        return member;
    }

    public CastSymbol VisitUnaryMinusExpr(CastParser.UnaryMinusExprContext context)
    {
        return Visit(context.expr);
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

        string spaceName = "";
        string leftSpace = left.SpaceName;
        string rightSpace = right.SpaceName;
        if (leftSpace == "None")
        {
            leftSpace = right.SpaceName;
        }

        if (rightSpace == "None")
        {
            rightSpace =  left.SpaceName;
        }
        
        if (leftSpace != rightSpace)
            throw new Exception($"Space MISMATCH: Cannot combine Space '{left.SpaceName}' with '{right.SpaceName}'");
        spaceName = leftSpace;

        string opName = context.op.Text == "*" ? "__mul__" : "__div__";
        CastSymbol[] args = [left, right];

        string targetTypeName = left.IsStruct() 
            ? left.StructName 
            : left.CastType.ToString().ToLower();

        CastSymbol typeSymbol = _scope.Lookup(targetTypeName);
        if (typeSymbol == null)
        {
            throw new Exception($"Type definition for '{targetTypeName}' not found.");
        }

        FunctionKey key = FunctionKey.Of(opName, args.ToList());
        if (!typeSymbol.Functions.TryGetValue(key, out CastSymbol fn))
        {
            string leftName = GetReadableName(left);
            string rightName = GetReadableName(right);
            throw new Exception($"Operator '{context.op.Text}' ({opName}) not defined for '{leftName}' and '{rightName}'");
        }

        var clone = fn.ReturnType.Clone();
        clone.SpaceName = spaceName;
        clone.TypeSpace = _scope.Lookup(spaceName).Clone();

        for (int i = 0; i < fn.Parameters.Count; i++)
        {
            CastSymbol lhs = fn.Parameters[i];
            CastSymbol rhs = args[i];
            
            if (lhs.CastType != rhs.CastType || lhs.StructName != rhs.StructName || lhs.SpaceName !=  rhs.SpaceName)
            {
                throw new InvalidAssignmentException(context, lhs, rhs);
            }
        }
        
        return Nodes[context] = clone;
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
        return Visit(context.typedFunctionDecl());
    }

    public CastSymbol VisitForDeclStmt(CastParser.ForDeclStmtContext context)
    {
        try
        {
            _inLoop = true;
            Visit(context.forStmt());
        }
        finally
        {
            _inLoop = false;
        }
        
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

    public CastSymbol VisitContinueStmt(CastParser.ContinueStmtContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitBreakStmt(CastParser.BreakStmtContext context)
    {
        return CastSymbol.Void;
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
        
        throw new VariableNotFoundException(context, context.varRef.Text);
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

    public CastSymbol VisitLocationDecl(CastParser.LocationDeclContext context)
    {
        return CastSymbol.Void;
    }

    public CastSymbol VisitSwizzleDecl(CastParser.SwizzleDeclContext context)
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
    
    public CastSymbol VisitForStmt(CastParser.ForStmtContext context)
    {
        CastSymbol start = Visit(context.start);
        CastSymbol end = Visit(context.end);
        
        if (start.IsFunction)
            start = start.ReturnType;

        if (end.IsFunction)
            end = end.ReturnType;

        if (start.CastType != end.CastType)
        {
            throw new InvalidAssignmentException(context, start, end);
        }

        _scope = new Scope<CastSymbol>(_scope);
        string it = context.var.Text;
        _scope.Define(it, start);

        if (context.block() != null)
        {
            Visit(context.block());
        }

        // _scope = _scope.Parent;
        
        return CastSymbol.Void;
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
        if (context.block() != null)
        {
            foreach (var child in context.block().statement())
            {
                Visit(child);
            }
        }
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
        var fn = Nodes[context];

        _scope = new Scope<CastSymbol>(_scope);
        // if (context.typeVarName != null)
        // {
        //     CastSymbol type = _scope.Lookup(context.typeFn.Text).Clone();
        //     _scope.Define(context.typeVarName.Text, type);
        // }
        
        if (fn.IsDeclaration && context.block() != null)
        {
            Visit(context.block());
        }

        _scope = _scope.Parent;
        return fn;
    }

    public CastSymbol VisitFunctionCall(CastParser.FunctionCallContext context)
    {
        var functionName = context.name.Text;
        var fn = _scope.Lookup(functionName).Clone();

        var args = new List<CastSymbol>();
        if (context.args.simpleExpression() != null)
            foreach (var arg in context.args.simpleExpression())
            {
                CastSymbol argType = Visit(arg);
                if (argType.IsFunction)
                {
                    CastSymbol returnType = argType.ReturnType.Clone();
                    args.Add(returnType);
                }
                else
                {
                    args.Add(argType);
                }
            }

        // use typed functions functions
        if (fn.Functions.Count > 0)
        {
            var key = FunctionKey.Of(functionName, args);
            if (!fn.Functions.TryGetValue(key, out var typedFunction))
            {
                String @params = string.Join(", ", key.Types.Select(c => c.CastType.ToString()));
                throw new Exception($"Function {functionName} does not exist with Parameters: {@params}");
            }
            
            typedFunction = typedFunction.ReturnType.Clone();
            
            if (context.typeSpace() != null)
            {
                typedFunction.TypeSpace = _scope.Lookup(context.typeSpace().spaceName.Text).Clone();
                typedFunction.SpaceName = context.typeSpace().spaceName.Text;
            }
            return Nodes[context] = typedFunction;
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

    public CastSymbol VisitTypeSpaceConversion(CastParser.TypeSpaceConversionContext context)
    {
        throw new NotImplementedException();
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

    public CastSymbol VisitUnaryExpression(CastParser.UnaryExpressionContext context)
    {
        throw new NotImplementedException();
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
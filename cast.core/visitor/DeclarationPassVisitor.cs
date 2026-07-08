using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;

namespace cast.core.visitor
{
    public class DeclarationPassVisitor : ICastParserVisitor<AbstractSymbol>
    {
        private readonly Scope _scope;
        private readonly ErrorLogger _logger;
        private readonly Dictionary<string, string> _samplerPayloads;
        
        public List<VariableSymbol> Inputs = new List<VariableSymbol>();
        public List<VariableSymbol> Outputs = new List<VariableSymbol>();
        public List<VariableSymbol> Uniforms = new List<VariableSymbol>();
        public List<VariableSymbol> Textures = new List<VariableSymbol>();

        public DeclarationPassVisitor(Scope scope, ErrorLogger logger, Dictionary<string, string>? samplerPayloads = null)
        {
            _scope = scope;
            _logger = logger;
            _samplerPayloads = samplerPayloads ?? new Dictionary<string, string>();
        }
        
        public AbstractSymbol Visit(IParseTree tree)
        {
            return tree.Accept(this);
        }

        public AbstractSymbol VisitChildren(IRuleNode node)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitTerminal(ITerminalNode node)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitErrorNode(IErrorNode node)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitTranslation_unit(CastParser.Translation_unitContext context)
        {
            foreach (var externalDeclaration in context.external_declaration())
            {
                Visit(externalDeclaration);
            }

            return default;
        }

        public AbstractSymbol VisitExternal_declaration(CastParser.External_declarationContext context)
        {
            if (context.function_definition() != null)
            {
                Visit(context.function_definition());
            }
            
            if (context.declaration() != null)
            {
                Visit(context.declaration());
            }
            
            return default;
        }

        public AbstractSymbol VisitDeclaration_statement(CastParser.Declaration_statementContext context)
        {
            return Visit(context.declaration());
        }

        public AbstractSymbol VisitExpression_statement(CastParser.Expression_statementContext context)
        {
            return default;
        }

        public AbstractSymbol VisitSelection_rest_statement(CastParser.Selection_rest_statementContext context)
        {
            return default;
        }

        public AbstractSymbol VisitSelection_statement(CastParser.Selection_statementContext context)
        {
            return default;
        }

        public AbstractSymbol VisitCondition(CastParser.ConditionContext context)
        {
            return default;
        }

        public AbstractSymbol VisitSwitch_statement(CastParser.Switch_statementContext context)
        {
            return default;
        }

        public AbstractSymbol VisitCase_label(CastParser.Case_labelContext context)
        {
            return default;
        }

        public AbstractSymbol VisitIteration_statement(CastParser.Iteration_statementContext context)
        {
            if (context.WHILE() != null)
            {
                if (context.condition() != null)
                    Visit(context.condition());
                if (context.statement_no_new_scope() != null)
                    Visit(context.statement_no_new_scope());
            }
            else if (context.DO() != null)
            {
                if (context.statement() != null)
                    Visit(context.statement());
                if (context.expression() != null)
                    Visit(context.expression());
            }
            else if (context.FOR() != null)
            {
                if (context.for_init_statement() != null)
                    Visit(context.for_init_statement());
                if (context.for_rest_statement() != null)
                    Visit(context.for_rest_statement());
                if (context.statement_no_new_scope() != null)
                    Visit(context.statement_no_new_scope());
            }

            return default;
        }

        public AbstractSymbol VisitFor_init_statement(CastParser.For_init_statementContext context)
        {
            if (context.expression_statement() != null)
                return Visit(context.expression_statement());
            if (context.declaration_statement() != null)
                return Visit(context.declaration_statement());
            return default;
        }

        public AbstractSymbol VisitFor_rest_statement(CastParser.For_rest_statementContext context)
        {
            if (context.condition() != null)
                Visit(context.condition());
            if (context.expression() != null)
                Visit(context.expression());
            return default;
        }

        public AbstractSymbol VisitJump_statement(CastParser.Jump_statementContext context)
        {
            if (context.RETURN() != null)
            {
                return Visit(context.expression());
            }

            return default;
        }

        public AbstractSymbol VisitSimple_statement(CastParser.Simple_statementContext context)
        {
            if (context.declaration_statement() != null) 
            {
                Visit(context.declaration_statement());
            }

            if (context.expression_statement() != null)
            {
                Visit(context.expression_statement());
            }

            return default;
        }

        public AbstractSymbol VisitStatement_no_new_scope(CastParser.Statement_no_new_scopeContext context)
        {
            if (context.compound_statement_no_new_scope() != null)
                return Visit(context.compound_statement_no_new_scope());
            if (context.simple_statement() != null)
                return Visit(context.simple_statement());
            return default;
        }

        public AbstractSymbol VisitCompound_statement(CastParser.Compound_statementContext context)
        {
            if (context.statement_list() != null)
            {
                Visit(context.statement_list());
            }

            return default;
        }

        public AbstractSymbol VisitCompound_statement_no_new_scope(CastParser.Compound_statement_no_new_scopeContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStatement(CastParser.StatementContext context)
        {
            if (context.simple_statement() != null) 
            {
                Visit(context.simple_statement());
            }

            if (context.compound_statement() != null)
            {
                Visit(context.compound_statement());
            }
            
            return default;
        }

        public AbstractSymbol VisitStatement_list(CastParser.Statement_listContext context)
        {
            foreach (var statementContext in context.statement())
            {
                Visit(statementContext);
            }
            
            return default;
        }

        public AbstractSymbol VisitFunction_definition(CastParser.Function_definitionContext context)
        {
            Visit(context.function_prototype());
            return default;
        }

        public AbstractSymbol VisitVariable_identifier(CastParser.Variable_identifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_call(CastParser.Function_callContext context)
        {
            if (context.function_identifier() != null)
                Visit(context.function_identifier());
            if (context.function_call_parameters() != null)
                Visit(context.function_call_parameters());
            return default;
        }

        public AbstractSymbol VisitField_selection(CastParser.Field_selectionContext context)
        {
            if (context.function_call() != null)
                return Visit(context.function_call());
            if (context.variable_identifier() != null)
                return Visit(context.variable_identifier());
            return default;
        }

        public AbstractSymbol VisitFunction_identifier(CastParser.Function_identifierContext context)
        {
            if (context.type_specifier() != null)
                return Visit(context.type_specifier());
            if (context.postfix_expression() != null)
                return Visit(context.postfix_expression());
            return default;
        }

        public AbstractSymbol VisitDimension(CastParser.DimensionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitArray_specifier(CastParser.Array_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSpace_definition_parameters(CastParser.Space_definition_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSpace_specifier(CastParser.Space_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_specifier(CastParser.Type_specifierContext context)
        {
            if (context.type_specifier_nonarray() != null)
            {
                if (context.type_specifier_nonarray().struct_specifier() != null)
                {
                    return Visit(context.type_specifier_nonarray().struct_specifier());
                }

                return Visit(context.type_specifier_nonarray());
            }

            string typeName = context.GetText();
            return _scope[typeName] as TypeSymbol;
        }

        public AbstractSymbol VisitPrimary_expression(CastParser.Primary_expressionContext context)
        {
            if (context.variable_identifier() != null)
                return Visit(context.variable_identifier());
            if (context.expression() != null)
                return Visit(context.expression());
            if (context.UINTCONSTANT() != null)
                return _scope["uint"] as TypeSymbol;
            if (context.INTCONSTANT() != null)
                return _scope["int"] as TypeSymbol;
            if (context.FLOATCONSTANT() != null)
                return _scope["float"] as TypeSymbol;
            if (context.TRUE() != null || context.FALSE() != null)
                return _scope["bool"] as TypeSymbol;
            return default;
        }

        public AbstractSymbol VisitPostfix_expression(CastParser.Postfix_expressionContext context)
        {
            if (context.primary_expression() != null)
                return Visit(context.primary_expression());
            if (context.postfix_expression() != null)
            {
                Visit(context.postfix_expression());
                if (context.integer_expression() != null)
                    Visit(context.integer_expression());
            }
            if (context.field_selection() != null)
                return Visit(context.field_selection());
            if (context.function_call_parameters() != null)
                return Visit(context.function_call_parameters());
            return default;
        }

        public AbstractSymbol VisitInteger_expression(CastParser.Integer_expressionContext context)
        {
            if (context.expression() != null)
                return Visit(context.expression());
            return default;
        }

        public AbstractSymbol VisitFunction_call_parameters(CastParser.Function_call_parametersContext context)
        {
            if (context.assignment_expression() != null)
            {
                foreach (var expr in context.assignment_expression())
                    Visit(expr);
            }
            return default;
        }

        public AbstractSymbol VisitExpression(CastParser.ExpressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitUnary_expression(CastParser.Unary_expressionContext context)
        {
            if (context.postfix_expression() != null)
                return Visit(context.postfix_expression());
            if (context.unary_expression() != null)
                return Visit(context.unary_expression());
            return default;
        }

        public AbstractSymbol VisitUnary_operator(CastParser.Unary_operatorContext context)
        {
            return default;
        }

        public AbstractSymbol VisitConstant_expression(CastParser.Constant_expressionContext context)
        {
            if (context.QUESTION() != null)
            {
                Visit(context.binary_expression());
                Visit(context.expression());
                Visit(context.assignment_expression());
                return default;
            }
            if (context.binary_expression() != null)
                return Visit(context.binary_expression());
            return default;
        }

        public AbstractSymbol VisitAssignment_expression(CastParser.Assignment_expressionContext context)
        {
            if (context.unary_expression() != null)
                Visit(context.unary_expression());
            if (context.assignment_expression() != null)
                Visit(context.assignment_expression());
            if (context.constant_expression() != null)
                return Visit(context.constant_expression());
            return default;
        }

        public AbstractSymbol VisitBinary_expression(CastParser.Binary_expressionContext context)
        {
            if (context.binary_expression(0) != null)
                Visit(context.binary_expression(0));
            if (context.binary_expression(1) != null)
                Visit(context.binary_expression(1));
            if (context.unary_expression() != null)
                return Visit(context.unary_expression());
            return default;
        }

        public AbstractSymbol VisitAssignment_operator(CastParser.Assignment_operatorContext context)
        {
            return default;
        }

        public AbstractSymbol VisitDeclaration(CastParser.DeclarationContext context)
        {
            if (context.function_prototype() != null)
                return Visit(context.function_prototype());

            if (context.struct_declaration_list() != null)
            {
                string blockName = context.IDENTIFIER(0).GetText();
                var structSymbol = new StructSymbol(blockName);

                foreach (var decl in context.struct_declaration_list().struct_declaration())
                {
                    var fieldType = Visit(decl.type_specifier()) as TypeSymbol;
                    if (fieldType == null) continue;

                    foreach (var declarator in decl.struct_declarator_list().struct_declarator())
                    {
                        structSymbol.AddField(declarator.IDENTIFIER().GetText(), fieldType);
                    }
                }

                _scope.Define(structSymbol);

                if (context.IDENTIFIER().Length > 1)
                {
                    string varName = context.IDENTIFIER(1).GetText();
                    var varType = new CastType(structSymbol);
                    _scope.Define(new VariableSymbol(varName, varType));
                }

                return default;
            }

            if (context.init_declarator_list() != null)
                Visit(context.init_declarator_list());

            return default;
        }

        public AbstractSymbol VisitIdentifier_list(CastParser.Identifier_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_prototype(CastParser.Function_prototypeContext context)
        {
            string functionName = context.IDENTIFIER().GetText();
            string returnTypeName = context.fully_specified_type().type_specifier().GetChild(0).GetText();

            TypeSymbol? returnTypeSymbol = _scope[returnTypeName] as TypeSymbol;
            if (returnTypeSymbol == null)
            {
                string message = "Unknown Return Type: " + returnTypeName;
                _logger.Log(context.Start, message);
                return CastType.ErrorType.Type;
            }
            
            List<SpaceSymbol> spaceSymbols = new List<SpaceSymbol>();
            if (returnTypeSymbol.HasSpaces())
            {
                if (context.fully_specified_type().type_specifier().space_specifier() != null)
                {
                    CastParser.Space_specifierContext spaceContext = context.fully_specified_type().type_specifier().space_specifier();
                    
                    foreach (ITerminalNode spaceSymbol in spaceContext.space_definition_parameters().IDENTIFIER())
                    {
                        // TODO: look up actual spaces
                        spaceSymbols.Add(new SpaceSymbol(spaceSymbol.GetText()));
                    }
                }
            }

            CastType returnType = new CastType(returnTypeSymbol, spaceSymbols);
            FunctionSymbol function = new FunctionSymbol(functionName, returnType);
            
            _scope.Define(function);
            return default;
        }

        public AbstractSymbol VisitFunction_parameters(CastParser.Function_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitParameter_declarator(CastParser.Parameter_declaratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitParameter_declaration(CastParser.Parameter_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitParameter_type_specifier(CastParser.Parameter_type_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_qualifier(CastParser.Type_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInitializer_list(CastParser.Initializer_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInitializer(CastParser.InitializerContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInit_declarator_list(CastParser.Init_declarator_listContext context)
        {
            if (context.single_declaration() != null)
            {
                Visit(context.single_declaration());
            }
            
            if (context.typeless_declaration() != null)
            {
                foreach (var typelessDeclarationContext in context.typeless_declaration())
                {
                    Visit(typelessDeclarationContext);
                }
            }

            return null;
        }

        public AbstractSymbol VisitSingle_declaration(CastParser.Single_declarationContext context)
        {
            var typeSymbol = Visit(context.fully_specified_type()) as TypeSymbol;
            if (typeSymbol == null)
            {
                _logger.Log(context.Start, $"Type not found in declaration.");
                return null;
            }

            if (context.typeless_declaration() == null)
            {
                if (typeSymbol is StructSymbol)
                    return typeSymbol;
                return null;
            }

            string name = context.typeless_declaration().IDENTIFIER().GetText();

            List<SpaceSymbol> spaces = new List<SpaceSymbol>();
            var spaceSpec = context.fully_specified_type().type_specifier().space_specifier();
            if (!typeSymbol.Name.StartsWith("sampler"))
            {
                if (spaceSpec != null)
                {
                    foreach (ITerminalNode spaceId in spaceSpec.space_definition_parameters().IDENTIFIER())
                    {
                        var space = _scope[spaceId.GetText()] as SpaceSymbol;
                        if (space != null)
                            spaces.Add(space);
                    }
                }
            }

            Modifier modifier = Modifier.NONE;
            var typeQualifier = context.fully_specified_type()?.type_qualifier();
            if (typeQualifier != null)
            {
                foreach (var sq in typeQualifier.single_type_qualifier())
                {
                    var storage = sq.storage_qualifier();
                    if (storage != null)
                    {
                        string sqText = storage.GetText();
                        if (Enum.TryParse(sqText, true, out Modifier m))
                            modifier = m;
                        break;
                    }
                }
            }

            var type = new CastType(typeSymbol, spaces);
            var variable = new VariableSymbol(name, type, modifier);

            if (modifier == Modifier.IN) Inputs.Add(variable);
            if (modifier == Modifier.OUT) Outputs.Add(variable);
            if (modifier == Modifier.UNIFORM) Uniforms.Add(variable);

            if (modifier == Modifier.UNIFORM && typeSymbol.Name.Contains("sampler"))
            {
                if (_samplerPayloads.TryGetValue(name, out var payloadText))
                {
                    CastType payloadType = ParseCastType(payloadText, _scope);
                    variable = new VariableSymbol(name, new SamplerType(typeSymbol, payloadType), modifier);
                }
                Textures.Add(variable);
            }

            _scope.Define(variable);
            return variable;
        }

        private static CastType ParseCastType(string text, Scope scope)
        {
            text = text.Trim();
            int angleStart = text.IndexOf('<');
            if (angleStart < 0)
            {
                var ts = scope[text] as TypeSymbol;
                return ts != null ? new CastType(ts) : CastType.ErrorType;
            }

            string baseName = text.Substring(0, angleStart).Trim();
            string inner = text.Substring(angleStart + 1, text.Length - angleStart - 2).Trim();

            var baseType = scope[baseName] as TypeSymbol;
            if (baseType == null) return CastType.ErrorType;

            var innerParts = inner.Split(',');
            var spaces = new List<SpaceSymbol>();
            CastType? payload = null;

            foreach (var part in innerParts)
            {
                string trimmed = part.Trim();
                var space = scope[trimmed] as SpaceSymbol;
                if (space != null)
                    spaces.Add(space);
                else
                {
                    var innerType = ParseCastType(trimmed, scope);
                    if (innerType != CastType.ErrorType && !innerType.Type.Name.Contains("ERROR"))
                    {
                        if (innerType.Spaces.Count > 0 || innerType.Type.Name.Contains("vec") || innerType.Type.Name.Contains("mat") || innerType.Type.Name.Contains("float") || innerType.Type.Name.Contains("int"))
                            payload = innerType;
                    }
                }
            }

            if (baseName.Contains("sampler"))
                return new SamplerType(baseType, payload ?? new CastType(baseType));

            return new CastType(baseType, spaces);
        }

        public AbstractSymbol VisitTypeless_declaration(CastParser.Typeless_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStruct_declaration(CastParser.Struct_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStruct_declaration_list(CastParser.Struct_declaration_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStruct_declarator_list(CastParser.Struct_declarator_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStruct_declarator(CastParser.Struct_declaratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFully_specified_type(CastParser.Fully_specified_typeContext context)
        {
            if (context.type_specifier() != null)
            {
                return Visit(context.type_specifier());
            }

            return default;
        }

        public AbstractSymbol VisitSingle_type_qualifier(CastParser.Single_type_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitPrecise_qualifier(CastParser.Precise_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitPrecision_qualifier(CastParser.Precision_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStorage_qualifier(CastParser.Storage_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInvariant_qualifier(CastParser.Invariant_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInterpolation_qualifier(CastParser.Interpolation_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitLayout_qualifier_id_list(CastParser.Layout_qualifier_id_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitLayout_qualifier_id(CastParser.Layout_qualifier_idContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitLayout_qualifier(CastParser.Layout_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStruct_specifier(CastParser.Struct_specifierContext context)
        {
            string? structName = context.IDENTIFIER()?.GetText();
            var structSymbol = new StructSymbol(structName ?? "_anonymous");

            if (context.struct_declaration_list() != null)
            {
                foreach (var decl in context.struct_declaration_list().struct_declaration())
                {
                    var fieldType = Visit(decl.type_specifier()) as TypeSymbol;
                    if (fieldType == null) continue;

                    foreach (var declarator in decl.struct_declarator_list().struct_declarator())
                    {
                        structSymbol.AddField(declarator.IDENTIFIER().GetText(), fieldType);
                    }
                }
            }

            if (structName != null)
                _scope.Define(structSymbol);

            return structSymbol;
        }

        public AbstractSymbol VisitType_name(CastParser.Type_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_specifier_nonarray(CastParser.Type_specifier_nonarrayContext context)
        {
            if (context.struct_specifier() != null)
                return Visit(context.struct_specifier());

            string name;
            if (context.type_name() != null)
                name = context.type_name().GetText();
            else
                name = context.GetText();

            return _scope[name] as TypeSymbol;
        }
    }
}
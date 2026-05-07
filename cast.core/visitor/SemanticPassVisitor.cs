using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.registry;

namespace cast.core.visitor
{
    public class SemanticPassVisitor : ICastParserVisitor<CastType>
    {
        private Scope _scope;
        
        private readonly ErrorLogger _logger;
        
        public SemanticPassVisitor(Scope scope, ErrorLogger logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public CastType Visit(IParseTree tree)
        {
            return tree.Accept(this);
        }

        public CastType VisitChildren(IRuleNode node)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitTerminal(ITerminalNode node)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitErrorNode(IErrorNode node)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitTranslation_unit(CastParser.Translation_unitContext context)
        {
            foreach (var externalDeclaration in context.external_declaration())
            {
                Visit(externalDeclaration);
            }

            return default;
        }

        public CastType VisitExternal_declaration(CastParser.External_declarationContext context)
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

        public CastType VisitDeclaration_statement(CastParser.Declaration_statementContext context)
        {
            return Visit(context.declaration());
        }

        public CastType VisitExpression_statement(CastParser.Expression_statementContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }

            return default;
        }

        public CastType VisitSelection_rest_statement(CastParser.Selection_rest_statementContext context)
        {
            foreach (var statementContext in context.statement())
            {
                Visit(statementContext);
            }

            return default;
        }

        public CastType VisitSelection_statement(CastParser.Selection_statementContext context)
        {
            Visit(context.expression());
            
            if (context.selection_rest_statement() != null)
            {
                Visit(context.selection_rest_statement());
            }

            return default;
        }

        public CastType VisitCondition(CastParser.ConditionContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitSwitch_statement(CastParser.Switch_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitCase_label(CastParser.Case_labelContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitIteration_statement(CastParser.Iteration_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFor_init_statement(CastParser.For_init_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFor_rest_statement(CastParser.For_rest_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitJump_statement(CastParser.Jump_statementContext context)
        {
            if (context.RETURN() != null)
            {
                CastType returnValue = Visit(context.expression());
                returnValue.IsReturn = true;
                return returnValue;
            }

            return default;
        }

        public CastType VisitSimple_statement(CastParser.Simple_statementContext context)
        {
            if (context.declaration_statement() != null)
            {
                return Visit(context.declaration_statement());
            }

            if (context.jump_statement() != null)
            {
                return Visit(context.jump_statement());
            }

            if (context.selection_statement() != null)
            {
                return Visit(context.selection_statement());
            }

            if (context.expression_statement() != null)
            {
                return Visit(context.expression_statement());
            }

            return default;
        }

        public CastType VisitStatement_no_new_scope(CastParser.Statement_no_new_scopeContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitCompound_statement(CastParser.Compound_statementContext context)
        {
            if (context.statement_list() != null)
            {
                foreach (var statementContext in context.statement_list().statement())
                {
                    Visit(statementContext);
                }
            }

            return default;
        }

        public CastType VisitCompound_statement_no_new_scope(CastParser.Compound_statement_no_new_scopeContext context)
        {
            if (context.statement_list() != null)
            {
                Visit(context.statement_list());
            }
            return default;
        }

        public CastType VisitStatement(CastParser.StatementContext context)
        {
            if (context.compound_statement() != null)
            {
                Visit(context.compound_statement());
            }

            if (context.simple_statement() != null)
            {
                return Visit(context.simple_statement());
            }

            return default;
        }

        public CastType VisitStatement_list(CastParser.Statement_listContext context)
        {
            foreach (var statement in context.statement())
            {
                CastType? result = Visit(statement);
                if (result?.IsReturn == true)
                {
                    return result;
                }
            }
            
            return default;
        }

        public CastType VisitFunction_definition(CastParser.Function_definitionContext context)
        {
            string functionName = context.function_prototype().IDENTIFIER().GetText();

            FunctionSymbol? function = _scope[functionName] as FunctionSymbol;
            if (function == null)
            {
                string message = $"Function '{functionName}' not found.";
                _logger.Log(context.Start, message);
                return CastType.ErrorType;
            }
            
            Scope functionScope = new Scope(_scope);
            function.SetScope(functionScope);

            // set scope to that of the function
            _scope = functionScope;
            {
                if (context.function_prototype().function_parameters() != null)
                {
                    foreach (var parameterDeclarationContext in context.function_prototype()
                                 .function_parameters()
                                 .parameter_declaration())
                    {
                        string name = parameterDeclarationContext.parameter_declarator().IDENTIFIER().GetText();
                        // _scope.Define(new TypeSymbol(name))
                    }
                }
                
                if (context.compound_statement_no_new_scope() != null)
                {
                    CastParser.Statement_listContext statementList = context.compound_statement_no_new_scope().statement_list();
                    if (statementList != null)
                    {
                        foreach (var statementContext in statementList.statement())
                        {
                            CastType? type = Visit(statementContext);
                            if (type != null && type.IsReturn)
                            {
                                if (!type.Equals(function.ReturnType()))
                                {
                                    string message = $"Invalid return type '{type}', must be '{function.ReturnType()}'";
                                    _logger.Log(context.Start, message);
                                }
                            }
                        }
                    }
                }
            }
            _scope = functionScope.Parent;
            

            return default;
        }

        public CastType VisitVariable_identifier(CastParser.Variable_identifierContext context)
        {
            string identifier = context.IDENTIFIER().GetText();
            AbstractSymbol symbol = _scope[identifier];

            if (symbol is VariableSymbol variable) return variable.Type;
            if (symbol is FunctionSymbol function) return function.ReturnType();

            return CastType.ErrorType;
        }

        public CastType VisitFunction_call(CastParser.Function_callContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitField_selection(CastParser.Field_selectionContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFunction_identifier(CastParser.Function_identifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitDimension(CastParser.DimensionContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitArray_specifier(CastParser.Array_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitSpace_definition_parameters(CastParser.Space_definition_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitSpace_specifier(CastParser.Space_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitType_specifier(CastParser.Type_specifierContext context)
        {
            List<SpaceSymbol> spaces = new List<SpaceSymbol>();
            string typeName = context.type_specifier_nonarray().GetText();
            TypeSymbol type = _scope[typeName] as TypeSymbol;
            if (context.space_specifier() != null)
            {
                foreach (var space in context.space_specifier().space_definition_parameters().children)
                {
                    spaces.Add(_scope[space.GetText()] as SpaceSymbol);
                }
            }

            return new CastType(type, spaces);
        }

        public CastType VisitPrimary_expression(CastParser.Primary_expressionContext context)
        {
            if (context.variable_identifier() != null)
            {
                return Visit(context.variable_identifier());
            }

            if (context.INTCONSTANT() != null)
            {
                TypeSymbol s = _scope["int"] as TypeSymbol;
                return new CastType(s);
            }
            
            if (context.FLOATCONSTANT() != null)
            {
                TypeSymbol s = _scope["float"] as TypeSymbol;
                return new CastType(s);
            }
            
            if (context.UINTCONSTANT() != null)
            {
                TypeSymbol s = _scope["uint"] as TypeSymbol;
                return new CastType(s);
            }

            return null;
        }

        public CastType VisitPostfix_expression(CastParser.Postfix_expressionContext context)
        {
            if (context.primary_expression() != null)
            {
                return Visit(context.primary_expression());
            }

            if (context.type_specifier() != null)
            {
            }

            if (context.function_call_parameters() != null)
            {
                string functionName = context.type_specifier()?.type_specifier_nonarray()?.GetText();
                // interpret functionName as expression, since its not a type
                if (string.IsNullOrEmpty(functionName))
                {
                    functionName = context.postfix_expression().GetText();
                }
                List<CastType> parameters = new List<CastType>();
                foreach (var parameterContext in context.function_call_parameters().assignment_expression())
                {
                    parameters.Add(Visit(parameterContext));
                }
                
                return Registry.ResolveFunction(functionName, parameters, _logger, _scope);
            }
            
            return CastType.ErrorType;
        }

        public CastType VisitInteger_expression(CastParser.Integer_expressionContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }

            return CastType.ErrorType;
        }

        public CastType VisitFunction_call_parameters(CastParser.Function_call_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitExpression(CastParser.ExpressionContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }

            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }

            return default;
        }

        public CastType VisitUnary_expression(CastParser.Unary_expressionContext context)
        {
            if (context.postfix_expression() != null)
            {
                return Visit(context.postfix_expression());
            }

            if (context.unary_expression() != null)
            {
                return Visit(context.unary_expression());
            }

            if (context.unary_operator() != null)
            {
                return Visit(context.unary_operator());
            }

            return default;
        }

        public CastType VisitUnary_operator(CastParser.Unary_operatorContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitConstant_expression(CastParser.Constant_expressionContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }
            
            if (context.binary_expression() != null)
            {
                return Visit(context.binary_expression());
            }

            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }
            
            return default;
        }

        public CastType VisitAssignment_expression(CastParser.Assignment_expressionContext context)
        {
            if (context.assignment_expression() != null)
            {
                CastType left = Visit(context.children[0]);
                CastType right = Visit(context.children[2]);

                return CastType.ErrorType;
            }
            
            if (context.unary_expression() != null)
            {
                return Visit(context.unary_expression());
            }

            if (context.constant_expression() != null)
            {
                return Visit(context.constant_expression());
            }

            return default;
        }

        public CastType VisitBinary_expression(CastParser.Binary_expressionContext context)
        {
            if (context.ChildCount == 3)
            {
                CastType? left = Visit(context.children[0]);
                CastType? right = Visit(context.children[2]);
                
                string op = context.children[1].ToString();
                CastType? eval = Registry.ResolveOperator(context.Start, _scope, _logger, op, new List<CastType>(new[] { left, right }));
                if (eval == null) return CastType.ErrorType;
                return eval;
            }

            if (context.unary_expression() != null)
            {
                return Visit(context.unary_expression());
            }
            
            return Visit(context.children[0]);
        }

        public CastType VisitAssignment_operator(CastParser.Assignment_operatorContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitDeclaration(CastParser.DeclarationContext context)
        {
            if (context.init_declarator_list() != null)
            {
                CastParser.Single_declarationContext declaration = context.init_declarator_list().single_declaration();
                CastType type = Visit(declaration.fully_specified_type());

                string left = declaration.typeless_declaration().IDENTIFIER().GetText();
                if (declaration.typeless_declaration().initializer() != null)
                {
                    CastType initializer = Visit(declaration.typeless_declaration().initializer());
                    if (!initializer.IsAssignable(type))
                    {
                        _logger.Log(context.Start, $"Unable to assign {initializer} to {type}");
                        type = CastType.ErrorType;
                    }
                }

                _scope.Define(new VariableSymbol(left, type));
            }

            return default;
        }

        public CastType VisitIdentifier_list(CastParser.Identifier_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFunction_prototype(CastParser.Function_prototypeContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFunction_parameters(CastParser.Function_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitParameter_declarator(CastParser.Parameter_declaratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitParameter_declaration(CastParser.Parameter_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitParameter_type_specifier(CastParser.Parameter_type_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitType_qualifier(CastParser.Type_qualifierContext context)
        {
            Console.WriteLine(context.GetText());
            return default;
        }

        public CastType VisitInitializer_list(CastParser.Initializer_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitInitializer(CastParser.InitializerContext context)
        {
            // function call
            if (context.RIGHT_BRACE() != null && context.LEFT_BRACE() != null)
            {
                Console.WriteLine(context.assignment_expression().constant_expression());
            }
            
            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }
            
            return default;
        }

        public CastType VisitInit_declarator_list(CastParser.Init_declarator_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitSingle_declaration(CastParser.Single_declarationContext context)
        {
            string typeName = context.fully_specified_type().type_specifier().type_specifier_nonarray().GetText();
            string variableName = context.typeless_declaration().IDENTIFIER().GetText();

            if (_scope[variableName] != null)
            {
                VariableSymbol a = _scope[variableName] as VariableSymbol;
                return a.Type;
            }

            TypeSymbol? type = _scope[typeName] as TypeSymbol;
            if (type == null)
            {
                _logger.Log(context.Start, $"Type '{typeName}' not found.");
            }

            List<SpaceSymbol> spaces = new List<SpaceSymbol>();
            if (context.fully_specified_type().type_specifier().space_specifier() != null)
            {
                foreach (ITerminalNode typeSpace in context.fully_specified_type().type_specifier()
                             .space_specifier().space_definition_parameters().IDENTIFIER())
                {
                    string spaceName = typeSpace.GetText();
                    SpaceSymbol? space = _scope[spaceName] as SpaceSymbol;
                    spaces.Add(space);
                }
            }

            CastType variableType = new CastType(type, spaces);
            VariableSymbol variableSymbol = new VariableSymbol(variableName, variableType);
            _scope.Define(variableSymbol);

            if (context.typeless_declaration() != null)
            {
                CastType? eval = Visit(context.typeless_declaration());
                if (eval != null && !eval.Equals(variableType))
                {
                    if (!eval.IsAssignable(variableType))
                    {
                        string message = $"Unable to assign type '{eval.Type}' to '{context.fully_specified_type().GetText()}'";
                        _logger.Log(context.Start, message);
                    }
                }
            }

            return default;
        }

        public CastType VisitTypeless_declaration(CastParser.Typeless_declarationContext context)
        {
            if (context.initializer() != null)
            {
                return Visit(context.initializer());
            }
            
            return default;
        }

        public CastType VisitFully_specified_type(CastParser.Fully_specified_typeContext context)
        {
            string type = context.type_specifier().type_specifier_nonarray().GetText();
            if (_scope[type] == null)
            {
                _logger.Log(context.Start, $"Type '{type}' not found.");
                return CastType.ErrorType;
            }

            TypeSymbol typeSymbol = _scope[type] as TypeSymbol;
            List<SpaceSymbol> spaceSymbols = new List<SpaceSymbol>();
            if (context.type_specifier()?.space_specifier()?.space_definition_parameters() != null)
            {
                var spaces = context.type_specifier().space_specifier().space_definition_parameters().IDENTIFIER();
                foreach (ITerminalNode space in spaces)
                {
                    spaceSymbols.Add(_scope[space.GetText()] as SpaceSymbol);
                }
            }
            
            return new CastType(typeSymbol, spaceSymbols);
        }

        public CastType VisitSingle_type_qualifier(CastParser.Single_type_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitPrecise_qualifier(CastParser.Precise_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitPrecision_qualifier(CastParser.Precision_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStorage_qualifier(CastParser.Storage_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitInvariant_qualifier(CastParser.Invariant_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitInterpolation_qualifier(CastParser.Interpolation_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitLayout_qualifier_id_list(CastParser.Layout_qualifier_id_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitLayout_qualifier_id(CastParser.Layout_qualifier_idContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitLayout_qualifier(CastParser.Layout_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStruct_declaration(CastParser.Struct_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStruct_declaration_list(CastParser.Struct_declaration_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStruct_declarator_list(CastParser.Struct_declarator_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStruct_declarator(CastParser.Struct_declaratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitStruct_specifier(CastParser.Struct_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitType_name(CastParser.Type_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitType_specifier_nonarray(CastParser.Type_specifier_nonarrayContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
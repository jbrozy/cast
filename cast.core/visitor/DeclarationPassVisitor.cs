using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        
        public List<VariableSymbol> Inputs = new List<VariableSymbol>();
        public List<VariableSymbol> Outputs = new List<VariableSymbol>();
        public List<VariableSymbol> Uniforms = new List<VariableSymbol>();
        
        public DeclarationPassVisitor(Scope scope, ErrorLogger logger)
        {
            _scope = scope;
            _logger = logger;
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
            return default;
        }

        public AbstractSymbol VisitFor_init_statement(CastParser.For_init_statementContext context)
        {
            return default;
        }

        public AbstractSymbol VisitFor_rest_statement(CastParser.For_rest_statementContext context)
        {
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitField_selection(CastParser.Field_selectionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_identifier(CastParser.Function_identifierContext context)
        {
            throw new System.NotImplementedException();
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
                Console.WriteLine(context.type_specifier_nonarray().GetText().ToString());
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
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitPostfix_expression(CastParser.Postfix_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitInteger_expression(CastParser.Integer_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_call_parameters(CastParser.Function_call_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitExpression(CastParser.ExpressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitUnary_expression(CastParser.Unary_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitUnary_operator(CastParser.Unary_operatorContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitConstant_expression(CastParser.Constant_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitAssignment_expression(CastParser.Assignment_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitBinary_expression(CastParser.Binary_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitAssignment_operator(CastParser.Assignment_operatorContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitDeclaration(CastParser.DeclarationContext context)
        {
            if (context.function_prototype() != null)
            {
                return Visit(context.function_prototype());
            }

            if (context.identifier_list() != null)
            {
                Visit(context.identifier_list());
            }

            if (context.init_declarator_list() != null)
            {
                Visit(context.init_declarator_list());
            }

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
            AbstractSymbol t = Visit(context.fully_specified_type());
            string name = context.typeless_declaration().GetChild(0).GetText();

            if (t is StructSymbol structSymbol)
            {
                
            }

            if (t is VariableSymbol variableSymbol)
            {
                
            }

            // TypeSymbol? typeSymbol = _scope[typeName] as TypeSymbol;
            // if (typeSymbol == null)
            // {
            //     throw new Exception("Unknown Type: " + typeName);
            // }

            // List<SpaceSymbol> spaceSymbols = new List<SpaceSymbol>();
            // if (context.fully_specified_type().type_specifier().space_specifier() != null)
            // {
            //     CastParser.Space_definition_parametersContext typeSpaces = context.fully_specified_type()
            //         .type_specifier()
            //         .space_specifier().space_definition_parameters();

            //     foreach (ITerminalNode space in typeSpaces.IDENTIFIER())
            //     {
            //         string spaceName = space.GetText();
            //         SpaceSymbol? spaceSymbol = _scope[spaceName] as SpaceSymbol;

            //         if (spaceSymbol == null)
            //         {
            //             throw new Exception("Unknown Space: " + spaceName);
            //         }
            //         
            //         spaceSymbols.Add(spaceSymbol);
            //     }

            //     bool hasSpaces = typeSymbol.HasSpaces();
            //     bool hasOptionalSpaces = typeSymbol.HasSpaces();

            //     // if spaces are mandatory and no spaces were set
            //     if (!hasOptionalSpaces && !spaceSymbols.Any())
            //     {
            //         throw new Exception($"Spaces for '{typeName}' are mandatory.");
            //     }
            // }

            // string qualifier = context.fully_specified_type()?.type_qualifier()?.single_type_qualifier(0).GetText();
            // Modifier modifier = Modifier.NONE;
            // if (!string.IsNullOrEmpty(qualifier))
            // {
            //     if (!Enum.TryParse(qualifier, true, out modifier)) ;
            // }

            // CastType type = new CastType(typeSymbol, spaceSymbols);
            // VariableSymbol variableSymbol = new VariableSymbol(name, type, modifier);
            // 
            // if (modifier == Modifier.IN) Inputs.Add(variableSymbol);
            // if (modifier == Modifier.OUT) Outputs.Add(variableSymbol);
            // if (modifier == Modifier.UNIFORM) Uniforms.Add(variableSymbol);
        
            // _scope.Define(variableSymbol);

            return null;
        }

        public AbstractSymbol VisitTypeless_declaration(CastParser.Typeless_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFully_specified_type(CastParser.Fully_specified_typeContext context)
        {
            String? qualifier = String.Empty;
            
            if (context.type_qualifier() != null)
            {
                if (context.type_qualifier().single_type_qualifier() != null)
                {
                    qualifier = context.type_qualifier().single_type_qualifier().ToString();
                }
            }

            if (context.type_specifier() != null)
            {
                Visit(context.type_specifier());
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

        public AbstractSymbol VisitStruct_declaration(CastParser.Struct_declarationContext context)
        {
            TypeSymbol type = Visit(context.type_specifier()) as TypeSymbol;
            return type;
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

        public AbstractSymbol VisitStruct_specifier(CastParser.Struct_specifierContext context)
        {
            string structName = context.IDENTIFIER().GetText();
            StructSymbol structSymbol = new StructSymbol(structName);

            if (context.struct_declaration_list() != null)
            {
                foreach (var structDeclaration in context.struct_declaration_list().struct_declaration())
                {
                    string name = structDeclaration.type_specifier().GetChild(0).GetText();
                    TypeSymbol field = Visit(structDeclaration) as TypeSymbol;
                    structSymbol.AddField(name, field);
                }
            }
            return structSymbol;
        }

        public AbstractSymbol VisitType_name(CastParser.Type_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_specifier_nonarray(CastParser.Type_specifier_nonarrayContext context)
        {
            string typeName = context.type_name().GetText();
            return _scope[typeName] as TypeSymbol;
        }
    }
}
using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.registry;

namespace cast.core.visitor
{
    public class SemanticPassVisitor : ICastParserVisitor<CastType>
    {
        private Scope _scope;
        
        public SemanticPassVisitor(Scope scope)
        {
            _scope = scope;
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
            throw new System.NotImplementedException();
        }

        public CastType VisitSelection_rest_statement(CastParser.Selection_rest_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitSelection_statement(CastParser.Selection_statementContext context)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public CastType VisitSimple_statement(CastParser.Simple_statementContext context)
        {
            if (context.declaration_statement() != null)
            {
                return Visit(context.declaration_statement());
            }

            return default;
        }

        public CastType VisitStatement_no_new_scope(CastParser.Statement_no_new_scopeContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitCompound_statement(CastParser.Compound_statementContext context)
        {
            throw new System.NotImplementedException();
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
                Visit(context.simple_statement());
            }

            return default;
        }

        public CastType VisitStatement_list(CastParser.Statement_listContext context)
        {
            foreach (var statement in context.statement())
            {
                Visit(statement);
            }

            return default;
        }

        public CastType VisitFunction_definition(CastParser.Function_definitionContext context)
        {
            string functionName = context.function_prototype().IDENTIFIER().GetText();

            FunctionSymbol? function = _scope[functionName] as FunctionSymbol;
            if (function == null)
            {
                throw new Exception($"Function '{functionName}' not found.");
            }
            
            Scope functionScope = new Scope(_scope);
            function.SetScope(functionScope);

            // set scope to that of the function
            _scope = functionScope;
            {
                if (context.compound_statement_no_new_scope() != null)
                {
                    Visit(context.compound_statement_no_new_scope());
                }
            }
            _scope = functionScope.Parent;

            return default;
        }

        public CastType VisitVariable_identifier(CastParser.Variable_identifierContext context)
        {
            string identifier = context.IDENTIFIER().GetText();

            VariableSymbol variable = _scope[identifier] as VariableSymbol;
            return variable.Type;
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
            throw new System.NotImplementedException();
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
            if (context.integer_expression() != null)
            {
                return Visit(context.integer_expression());
            }

            if (context.primary_expression() != null)
            {
                return Visit(context.primary_expression());
            }

            return default;
        }

        public CastType VisitInteger_expression(CastParser.Integer_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitFunction_call_parameters(CastParser.Function_call_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitExpression(CastParser.ExpressionContext context)
        {
            throw new System.NotImplementedException();
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
            if (context.unary_expression() != null)
            {
                Visit(context.unary_expression());
                Console.WriteLine("context.unary_expression() != null");
            }

            if (context.assignment_expression() != null)
            {
                Visit(context.assignment_expression());
                Console.WriteLine("context.assignment_expression() != null");
            }

            if (context.constant_expression() != null)
            {
                return Visit(context.constant_expression());
                Console.WriteLine("context.constant_expression() != null");
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
                CastType eval = Registry.Resolve(_scope, op, new List<CastType>(new[] { left, right }));
                return eval;
            }
            
            return Visit(context.children[0]);
        }

        public CastType VisitAssignment_operator(CastParser.Assignment_operatorContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitDeclaration(CastParser.DeclarationContext context)
        {
            if (context.type_specifier() != null)
            {
                Console.WriteLine("context.type_specifier() != null");
            }
            if (context.identifier_list() != null)
            {
                Console.WriteLine("context.identifier_list() != null");
            }
            if (context.function_prototype() != null)
            {
                Console.WriteLine("context.function_prototype() != null");
            }
            if (context.init_declarator_list() != null)
            {
                Console.WriteLine("context.init_declarator_list() != null");
                if (context.init_declarator_list().single_declaration() != null)
                {
                    Console.WriteLine("context.init_declarator_list().single_declaration() != null");
                    return Visit(context.init_declarator_list().single_declaration());
                }
                if (context.init_declarator_list().typeless_declaration() != null)
                {
                    Console.WriteLine("context.init_declarator_list().typeless_declaration() != null");
                    // foreach (var typelessDeclarationContext in context.init_declarator_list().typeless_declaration())
                    // {
                    //     Visit(typelessDeclarationContext);
                    // }
                }
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
            throw new System.NotImplementedException();
        }

        public CastType VisitInitializer_list(CastParser.Initializer_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public CastType VisitInitializer(CastParser.InitializerContext context)
        {
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
                TypeSymbol a = _scope[variableName] as TypeSymbol;
                return new CastType(a);
            }

            TypeSymbol? type = _scope[typeName] as TypeSymbol;
            if (type == null)
                throw new Exception($"Type '{typeName}' not found.");

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
                return Visit(context.typeless_declaration());
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
            throw new System.NotImplementedException();
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
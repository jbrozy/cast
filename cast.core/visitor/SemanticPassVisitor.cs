using Antlr4.Runtime.Tree;
using cast.core.models;

namespace cast.core.visitor
{
    public class SemanticPassVisitor : ICastParserVisitor<AbstractSymbol>
    {
        private readonly Scope _scope;
        
        public SemanticPassVisitor(Scope scope)
        {
            _scope = scope;
        }

        public AbstractSymbol Visit(IParseTree tree)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitExternal_declaration(CastParser.External_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitDeclaration_statement(CastParser.Declaration_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitExpression_statement(CastParser.Expression_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSelection_rest_statement(CastParser.Selection_rest_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSelection_statement(CastParser.Selection_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitCondition(CastParser.ConditionContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSwitch_statement(CastParser.Switch_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitCase_label(CastParser.Case_labelContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitIteration_statement(CastParser.Iteration_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFor_init_statement(CastParser.For_init_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFor_rest_statement(CastParser.For_rest_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitJump_statement(CastParser.Jump_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSimple_statement(CastParser.Simple_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStatement_no_new_scope(CastParser.Statement_no_new_scopeContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitCompound_statement(CastParser.Compound_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitCompound_statement_no_new_scope(CastParser.Compound_statement_no_new_scopeContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStatement(CastParser.StatementContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitStatement_list(CastParser.Statement_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_definition(CastParser.Function_definitionContext context)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitIdentifier_list(CastParser.Identifier_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFunction_prototype(CastParser.Function_prototypeContext context)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitSingle_declaration(CastParser.Single_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitTypeless_declaration(CastParser.Typeless_declarationContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitFully_specified_type(CastParser.Fully_specified_typeContext context)
        {
            throw new System.NotImplementedException();
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

        public AbstractSymbol VisitStruct_specifier(CastParser.Struct_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_name(CastParser.Type_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public AbstractSymbol VisitType_specifier_nonarray(CastParser.Type_specifier_nonarrayContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
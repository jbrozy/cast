using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.utils;

namespace cast.core.visitor
{
    public class GlslPassVisitor : ICastParserVisitor<string>
    {
        private int _indent = 0;
        private readonly Scope _scope;
        private readonly CommonTokenStream _tokenStream;

        public GlslPassVisitor(Scope scope, CommonTokenStream tokenStream)
        {
            _scope = scope;
            _tokenStream = tokenStream;
        }

        private string GetIndentedText(string text)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _indent; i++)
            {
                builder.Append("  ");
            }

            builder.Append(text);
            return builder.ToString();
        }
        
        public string Visit(IParseTree tree)
        {
            return tree.Accept(this);
        }

        public string VisitChildren(IRuleNode node)
        {
            throw new System.NotImplementedException();
        }

        public string VisitTerminal(ITerminalNode node)
        {
            return node.GetText();
        }

        public string VisitErrorNode(IErrorNode node)
        {
            throw new System.NotImplementedException();
        }

        public string VisitTranslation_unit(CastParser.Translation_unitContext context)
        {
            StringBuilder builder = new StringBuilder();
            var declarations = context.external_declaration();

            for (int i = 0; i < declarations.Length; i++)
            {
                builder.AppendLine(Visit(declarations[i]));

                if (i < declarations.Length - 1)
                {
                    int startIdx = declarations[i].Stop.TokenIndex + 1;
                    int endIdx = declarations[i + 1].Start.TokenIndex - 1;
                    int newlines = 0;
                    for (int t = startIdx; t <= endIdx; t++)
                    {
                        string text = _tokenStream.Get(t).Text;
                        foreach (char c in text)
                            if (c == '\n') newlines++;
                    }
                    newlines--;
                    for (int j = 0; j < newlines; j++)
                        builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        public string VisitExternal_declaration(CastParser.External_declarationContext context)
        {
            if (context.declaration() != null)
            {
                return Visit(context.declaration());
            }
            
            if (context.function_definition() != null)
            {
                return Visit(context.function_definition());
            }

            return String.Empty;
        }

        public string VisitDeclaration_statement(CastParser.Declaration_statementContext context)
        {
            return Visit(context.declaration());
        }

        public string VisitExpression_statement(CastParser.Expression_statementContext context)
        {
            return $"{Visit(context.expression())};";
        }

        public string VisitSelection_rest_statement(CastParser.Selection_rest_statementContext context)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var statementContext in context.statement())
            {
                builder.Append(Visit(statementContext));
            }
            return builder.ToString();
        }

        public string VisitSelection_statement(CastParser.Selection_statementContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"if ({Visit(context.expression())})");
            _indent++;
            builder.Append(" {\n");
            builder.Append(Visit(context.selection_rest_statement()));
            _indent--;
            builder.Append(GetIndentedText("}"));
            return builder.ToString();
        }

        public string VisitCondition(CastParser.ConditionContext context)
        {
            if (context.expression() != null)
                return Visit(context.expression());
            return string.Empty;
        }

        public string VisitSwitch_statement(CastParser.Switch_statementContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitCase_label(CastParser.Case_labelContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitIteration_statement(CastParser.Iteration_statementContext context)
        {
            StringBuilder builder = new StringBuilder();

            if (context.WHILE() != null)
            {
                builder.Append($"while ({VisitCondition(context.condition())})");
                AppendBodyNoNewScope(builder, context.statement_no_new_scope());
            }
            else if (context.DO() != null)
            {
                builder.AppendLine("do {");
                _indent++;
                builder.Append(GetIndentedText(Visit(context.statement())));
                _indent--;
                builder.AppendLine();
                builder.Append(GetIndentedText($"}} while ({Visit(context.expression())});"));
            }
            else if (context.FOR() != null)
            {
                builder.Append("for (");
                builder.Append(Visit(context.for_init_statement()));
                builder.Append(" ");
                builder.Append(Visit(context.for_rest_statement()));
                builder.Append(")");
                AppendBodyNoNewScope(builder, context.statement_no_new_scope());
            }

            return builder.ToString();
        }

        private void AppendBodyNoNewScope(StringBuilder builder, CastParser.Statement_no_new_scopeContext body)
        {
            if (body.simple_statement() != null)
            {
                builder.AppendLine(" {");
                _indent++;
                builder.Append(GetIndentedText(Visit(body)));
                _indent--;
                builder.AppendLine();
                builder.Append(GetIndentedText("}"));
            }
            else
            {
                builder.Append(Visit(body));
            }
        }

        public string VisitFor_init_statement(CastParser.For_init_statementContext context)
        {
            if (context.expression_statement() != null)
                return Visit(context.expression_statement());
            if (context.declaration_statement() != null)
                return Visit(context.declaration_statement());
            return string.Empty;
        }

        public string VisitFor_rest_statement(CastParser.For_rest_statementContext context)
        {
            string result = string.Empty;
            if (context.condition() != null)
                result += VisitCondition(context.condition());
            result += ";";
            if (context.expression() != null)
                result += $" {Visit(context.expression())}";
            return result;
        }

        public string VisitJump_statement(CastParser.Jump_statementContext context)
        {
            if (context.RETURN() != null)
            {
                if (context.expression() != null)
                {
                    return $"return {Visit(context.expression())};";
                }

                return "return;";
            }

            if (context.CONTINUE() != null)
                return "continue;";

            if (context.BREAK() != null)
                return "break;";

            if (context.DISCARD() != null)
                return "discard;";

            return string.Empty;
        }

        public string VisitSimple_statement(CastParser.Simple_statementContext context)
        {
            if (context.expression_statement() != null)
            {
                return Visit(context.expression_statement());
            }
            
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

            if (context.iteration_statement() != null)
            {
                return Visit(context.iteration_statement());
            }

            if (context.switch_statement() != null)
            {
                return Visit(context.switch_statement());
            }

            if (context.case_label() != null)
            {
                return Visit(context.case_label());
            }

            return string.Empty;
        }

        public string VisitStatement_no_new_scope(CastParser.Statement_no_new_scopeContext context)
        {
            if (context.compound_statement_no_new_scope() != null)
                return Visit(context.compound_statement_no_new_scope());
            if (context.simple_statement() != null)
                return Visit(context.simple_statement());
            return string.Empty;
        }

        public string VisitCompound_statement(CastParser.Compound_statementContext context)
        {
            StringBuilder builder = new StringBuilder();
            if (context.statement_list() != null)
            {
                _indent++;
                foreach (var statementContext in context.statement_list().statement())
                {
                    builder.Append(GetIndentedText($"{Visit(statementContext)}\n"));
                } 
                _indent--;
            }

            return builder.ToString();
        }

        public string VisitCompound_statement_no_new_scope(CastParser.Compound_statement_no_new_scopeContext context)
        {
            StringBuilder builder = new StringBuilder();
            if (context.statement_list() != null && context.statement_list().statement() != null)
            {
                builder.AppendLine(" {");
                List<string> statements = new List<string>();
                _indent++;
                foreach (var statementContext in context.statement_list().statement())
                {
                    statements.Add(GetIndentedText(Visit(statementContext)));
                }
                builder.Append(string.Join("\n", statements));
                _indent--;
                builder.Append(GetIndentedText("\n}"));
            }
            else
            {
                builder.Append("{}");
            }
            return builder.ToString();
        }

        public string VisitStatement(CastParser.StatementContext context)
        {
            if (context.simple_statement() != null)
            {
                return Visit(context.simple_statement());
            }
            if (context.compound_statement() != null)
            {
                return Visit(context.compound_statement());
            }

            return string.Empty;
        }

        public string VisitStatement_list(CastParser.Statement_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitFunction_definition(CastParser.Function_definitionContext context)
        {
            string functionName = context.function_prototype().IDENTIFIER().GetText();
            string type = context.function_prototype().fully_specified_type().type_specifier().type_specifier_nonarray()
                .GetText();
            
            FunctionSymbol? function = _scope[functionName] as FunctionSymbol;
            StringBuilder builder = new StringBuilder();
            string parameters = string.Empty;
            if (context.function_prototype().function_parameters() != null)
            {
                parameters = Visit(context.function_prototype().function_parameters());
            }
            
            builder.Append($"{type} {functionName}({parameters})");
            if (context.compound_statement_no_new_scope() != null)
            {
                builder.Append(Visit(context.compound_statement_no_new_scope()));
            }
            else
            {
                builder.Append(";");
            }
            
            return builder.ToString();
        }

        public string VisitVariable_identifier(CastParser.Variable_identifierContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        public string VisitFunction_call(CastParser.Function_callContext context)
        {
            string functionName = Visit(context.function_identifier());
            string parameters = string.Empty;
            if (context.function_call_parameters() != null)
                parameters = Visit(context.function_call_parameters());
            return $"{functionName}({parameters})";
        }

        public string VisitField_selection(CastParser.Field_selectionContext context)
        {
            if (context.function_call() != null)
                return Visit(context.function_call());
            if (context.variable_identifier() != null)
                return Visit(context.variable_identifier());
            return string.Empty;
        }

        public string VisitFunction_identifier(CastParser.Function_identifierContext context)
        {
            if (context.type_specifier() != null)
                return Visit(context.type_specifier());
            if (context.postfix_expression() != null)
                return Visit(context.postfix_expression());
            return string.Empty;
        }

        public string VisitSpace_definition_parameters(CastParser.Space_definition_parametersContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitSpace_specifier(CastParser.Space_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitType_specifier(CastParser.Type_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitPrimary_expression(CastParser.Primary_expressionContext context)
        {
            if (context.expression() != null)
            {
                StringBuilder builder = new StringBuilder();
                if (context.LEFT_PAREN() != null) builder.Append("(");
                builder.Append(Visit(context.expression()));
                if (context.RIGHT_PAREN() != null) builder.Append(")");
                
                return builder.ToString();
            }
            if (context.INTCONSTANT() != null)
            {
                return Visit(context.INTCONSTANT());
            }

            if (context.FLOATCONSTANT() != null)
            {
                return Visit(context.FLOATCONSTANT());
            }

            if (context.variable_identifier() != null)
            {
                return Visit(context.variable_identifier());
            }

            return string.Empty;
        }

        public string VisitPostfix_expression(CastParser.Postfix_expressionContext context)
        {
            StringBuilder builder = new StringBuilder();
            if (context.primary_expression() != null)
            {
                return Visit(context.primary_expression());
            }

            if (context.integer_expression() != null && context.postfix_expression() != null)
            {
                builder.Append(Visit(context.postfix_expression()));
                builder.Append("[");
                builder.Append(Visit(context.integer_expression()));
                builder.Append("]");
                return builder.ToString();
            }

            if (context.DOT() != null)
            {
                var left = Visit(context.postfix_expression());
                var right = Visit(context.field_selection());

                return $"{left}.{right}";
            }

            if (context.function_call_parameters() != null)
            {
                string functionName = context.type_specifier()?.type_specifier_nonarray()?.GetText();
                if (string.IsNullOrEmpty(functionName))
                {
                    functionName = context.postfix_expression().GetText();
                }
                
                List<string> parameters = new List<string>();
                foreach (var parameterContext in context.function_call_parameters().assignment_expression())
                {
                    parameters.Add(Visit(parameterContext));
                }
                builder.Append($"{functionName}({string.Join(", ", parameters)})");
            }
            
            if (context.field_selection() != null)
            {
                builder.Append(Visit(context.field_selection()));
            }

            if (context.INC_OP() != null)
                return $"{Visit(context.postfix_expression())}++";

            if (context.DEC_OP() != null)
                return $"{Visit(context.postfix_expression())}--";

            return builder.ToString();
        }

        public string VisitInteger_expression(CastParser.Integer_expressionContext context)
        {
            if (context.expression() != null)
                return Visit(context.expression());
            return string.Empty;
        }

        public string VisitFunction_call_parameters(CastParser.Function_call_parametersContext context)
        {
            if (context.assignment_expression() != null)
            {
                List<string> args = new List<string>();
                foreach (var expr in context.assignment_expression())
                    args.Add(Visit(expr));
                return string.Join(", ", args);
            }
            return string.Empty;
        }

        public string VisitExpression(CastParser.ExpressionContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }

            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }
            
            return string.Empty;
        }

        public string VisitUnary_expression(CastParser.Unary_expressionContext context)
        {
            if (context.INC_OP() != null)
                return $"++{Visit(context.unary_expression())}";

            if (context.DEC_OP() != null)
                return $"--{Visit(context.unary_expression())}";

            if (context.unary_operator() != null)
                return $"{Visit(context.unary_operator())}{Visit(context.unary_expression())}";

            if (context.unary_expression() != null)
                return Visit(context.unary_expression());

            if (context.postfix_expression() != null)
                return Visit(context.postfix_expression());

            return string.Empty;
        }

        public string VisitUnary_operator(CastParser.Unary_operatorContext context)
        {
            return context.GetText();
        }

        public string VisitConstant_expression(CastParser.Constant_expressionContext context)
        {
            if (context.QUESTION() != null)
            {
                string cond = Visit(context.binary_expression());
                string trueExpr = Visit(context.expression());
                string falseExpr = Visit(context.assignment_expression());
                return $"{cond} ? {trueExpr} : {falseExpr}";
            }

            if (context.binary_expression() != null)
                return Visit(context.binary_expression());

            if (context.assignment_expression() != null)
                return Visit(context.assignment_expression());

            if (context.expression() != null)
                return Visit(context.expression());

            return string.Empty;
        }

        public string VisitAssignment_expression(CastParser.Assignment_expressionContext context)
        {
            if (context.assignment_operator() != null)
            {
                string left = Visit(context.unary_expression());
                string op = Visit(context.assignment_operator());
                string right = Visit(context.assignment_expression());
                
                return $"{left} {op} {right}";
            }
            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }
            if (context.constant_expression() != null)
            {
                return Visit(context.constant_expression());
            }
            
            if (context.unary_expression() != null)
            {
                return Visit(context.unary_expression());
            }

            return string.Empty;
        }

        public string VisitAssignment_operator(CastParser.Assignment_operatorContext context)
        {
            return context.GetText();
        }

        public string VisitBinary_expression(CastParser.Binary_expressionContext context)
        {
            if (context.unary_expression() != null)
            {
                return Visit(context.unary_expression());
            }
            
            if (context.binary_expression() != null && context.binary_expression().Length == 2)
            {
                string left = Visit(context.binary_expression(0));
                string right = Visit(context.binary_expression(1));

                string op = context.GetChild(1).GetText();

                return $"{left} {op} {right}";
            }

            return string.Empty;
        }

        public string VisitDeclaration(CastParser.DeclarationContext context)
        {
            StringBuilder builder = new StringBuilder();

            if (context.struct_declaration_list() != null)
            {
                string blockName = context.IDENTIFIER(0).GetText();
                string qualifier = context.type_qualifier().GetText();
                builder.Append($"{qualifier} {blockName} {{\n");
                _indent++;
                foreach (var decl in context.struct_declaration_list().struct_declaration())
                {
                    builder.Append(GetIndentedText(Visit(decl)));
                }
                _indent--;
                builder.Append(GetIndentedText("}"));

                if (context.IDENTIFIER().Length > 1)
                    builder.Append($" {context.IDENTIFIER(1).GetText()}");

                builder.Append(";");
                return builder.ToString();
            }

            if (context.init_declarator_list() != null)
            {
                var singleDecl = context.init_declarator_list().single_declaration();
                string type = Visit(singleDecl.fully_specified_type());

                if (singleDecl.typeless_declaration() != null)
                {
                    string identifier = Visit(singleDecl.typeless_declaration());
                    builder.Append($"{type} {identifier}");
                    if (singleDecl.typeless_declaration().initializer() != null)
                        builder.Append($" = {Visit(singleDecl.typeless_declaration().initializer())}");
                }
                else
                {
                    builder.Append(type);
                }

                return builder + ";";
            }

            return string.Empty;
        }

        public string VisitIdentifier_list(CastParser.Identifier_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitFunction_prototype(CastParser.Function_prototypeContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(context.fully_specified_type().type_specifier().type_specifier_nonarray());
            builder.Append($" {context.IDENTIFIER()}");
            builder.Append($"({Visit(context.function_parameters())})");
            return builder.ToString();
        }

        public string VisitFunction_parameters(CastParser.Function_parametersContext context)
        {
            List<string> parameters = new List<string>();
            foreach (var declarationContext in context.parameter_declaration())
            {
                parameters.Add(Visit(declarationContext));
            }

            return string.Join(", ", parameters);
        }

        public string VisitParameter_declarator(CastParser.Parameter_declaratorContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitParameter_declaration(CastParser.Parameter_declarationContext context)
        {
            string type = context.parameter_declarator().type_specifier().type_specifier_nonarray().GetText();
            string name = context.parameter_declarator().IDENTIFIER().GetText();
            
            return $"{type} {name}";
        }

        public string VisitParameter_type_specifier(CastParser.Parameter_type_specifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitType_qualifier(CastParser.Type_qualifierContext context)
        {
            List<string> qualifiers = new List<string>();
            foreach (var qualifier in context.single_type_qualifier())
            {
                qualifiers.Add(qualifier.GetText());
            }

            return string.Join(" ", qualifiers);
        }

        public string VisitInitializer_list(CastParser.Initializer_listContext context)
        {
            List<string> initializers = new List<string>();
            foreach (var initializer in context.initializer())
            {
                initializers.Add(Visit(initializer));
            }

            IEnumerable<List<string>> initBlocks = initializers.Partition(3);

            List<string> initBlockStrings = new List<string>();
            
            _indent++;
            foreach (var initBlock in initBlocks)
            {
                initBlockStrings.Add(string.Join(", ", initBlock.Select(GetIndentedText)));
            }
            _indent--;
            
            return "{ \n" + GetIndentedText(string.Join("\n", initBlockStrings.Select(GetIndentedText))) + "}";
        }

        public string VisitInitializer(CastParser.InitializerContext context)
        {
            if (context.initializer_list() != null)
            {
                return Visit(context.initializer_list());
            }

            if (context.assignment_expression() != null)
            {
                return Visit(context.assignment_expression());
            }
            
            return string.Empty;
        }

        public string VisitInit_declarator_list(CastParser.Init_declarator_listContext context)
        {
            if (context.single_declaration() != null)
            {
                return Visit(context.single_declaration());
            }
            
            return string.Empty;
        }

        public string VisitSingle_declaration(CastParser.Single_declarationContext context)
        {
            StringBuilder builder = new StringBuilder();

            string? qualifier = context.fully_specified_type()?.type_qualifier()?.GetText();
            if (qualifier != null) builder.Append($"{qualifier} ");

            string variableType = Visit(context.fully_specified_type().type_specifier().type_specifier_nonarray());

            if (context.typeless_declaration() != null)
            {
                string variableName = context.typeless_declaration().IDENTIFIER().GetText();
                builder.Append($"{variableType} {variableName}");

                if (context.typeless_declaration().initializer() != null)
                    builder.Append($" = {Visit(context.typeless_declaration().initializer())}");
            }
            else
            {
                builder.Append(variableType);
            }
            
            return builder + ";";
        }

        public string VisitTypeless_declaration(CastParser.Typeless_declarationContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(context.IDENTIFIER().GetText());

            if (context.array_specifier() != null)
                builder.Append(Visit(context.array_specifier()));

            return builder.ToString();
        }

        public string VisitArray_specifier(CastParser.Array_specifierContext context)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var dim in context.dimension())
                builder.Append(Visit(dim));
            return builder.ToString();
        }

        public string VisitDimension(CastParser.DimensionContext context)
        {
            string size = string.Empty;
            if (context.constant_expression() != null)
                size = Visit(context.constant_expression());
            return $"[{size}]";
        }

        public string VisitFully_specified_type(CastParser.Fully_specified_typeContext context)
        {
            StringBuilder builder = new StringBuilder();
            if (context.type_qualifier() != null)
            {
                builder.Append($"{Visit(context.type_qualifier())} ");
            }
            
            string type = Visit(context.type_specifier().type_specifier_nonarray());
            builder.Append($"{type}");
            return builder.ToString();
        }

        public string VisitSingle_type_qualifier(CastParser.Single_type_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitPrecise_qualifier(CastParser.Precise_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitPrecision_qualifier(CastParser.Precision_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitStorage_qualifier(CastParser.Storage_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitInvariant_qualifier(CastParser.Invariant_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitInterpolation_qualifier(CastParser.Interpolation_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitLayout_qualifier_id_list(CastParser.Layout_qualifier_id_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitLayout_qualifier_id(CastParser.Layout_qualifier_idContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitLayout_qualifier(CastParser.Layout_qualifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitStruct_declaration(CastParser.Struct_declarationContext context)
        {
            string type = context.type_specifier().type_specifier_nonarray().GetText();
            List<string> fields = new List<string>();
            foreach (var declarator in context.struct_declarator_list().struct_declarator())
            {
                fields.Add($"{type} {declarator.IDENTIFIER().GetText()}");
            }
            return string.Join(";\n", fields) + ";\n";
        }

        public string VisitStruct_declaration_list(CastParser.Struct_declaration_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitStruct_declarator_list(CastParser.Struct_declarator_listContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitStruct_declarator(CastParser.Struct_declaratorContext context)
        {
            return context.IDENTIFIER().GetText();
        }

        public string VisitStruct_specifier(CastParser.Struct_specifierContext context)
        {
            StringBuilder builder = new StringBuilder();
            string? name = context.IDENTIFIER()?.GetText();
            builder.Append($"struct {name ?? ""} {{\n");
            _indent++;
            foreach (var decl in context.struct_declaration_list().struct_declaration())
            {
                string type = decl.type_specifier().type_specifier_nonarray().GetText();
                foreach (var declarator in decl.struct_declarator_list().struct_declarator())
                {
                    builder.Append(GetIndentedText($"{type} {Visit(declarator)};\n"));
                }
            }
            _indent--;
            builder.Append(GetIndentedText("}"));
            return builder.ToString();
        }

        public string VisitType_name(CastParser.Type_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public string VisitType_specifier_nonarray(CastParser.Type_specifier_nonarrayContext context)
        {
            if (context.struct_specifier() != null)
                return Visit(context.struct_specifier());

            if (context.type_name() != null)
                return context.type_name().GetText();

            return context.GetText();
        }
    }
}
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace cast.core.visitor
{
    public class GlslMacroPreProcessor : ICastPreParserVisitor<object>
    {
        public string Version = string.Empty;
        public string Profile = string.Empty;
        
        public Dictionary<string, string> Macros = new Dictionary<string, string>();
        private readonly TokenStreamRewriter _rewriter;

        public GlslMacroPreProcessor(CommonTokenStream tokenStream)
        {
            _rewriter = new TokenStreamRewriter(tokenStream);
        }

        public string GetText()
        {
            return _rewriter.GetText();
        }
        
        public object Visit(IParseTree tree)
        {
            return tree.Accept(this);
        }

        public object VisitChildren(IRuleNode node)
        {
            throw new System.NotImplementedException();
        }

        public object VisitTerminal(ITerminalNode node)
        {
            throw new System.NotImplementedException();
        }

        public object VisitErrorNode(IErrorNode node)
        {
            throw new System.NotImplementedException();
        }

        public object VisitTranslation_unit(CastPreParser.Translation_unitContext context)
        {
            if (context.compiler_directive() != null)
            {
                foreach (var directiveContext in context.compiler_directive())
                {
                    if (directiveContext.version_directive() != null)
                    {
                        Visit(directiveContext.version_directive());
                    }
                    if (directiveContext.define_directive() != null)
                    {
                        Visit(directiveContext.define_directive());
                    }
                }
            }

            return default;
        }

        public object VisitCompiler_directive(CastPreParser.Compiler_directiveContext context)
        {
            if (context.version_directive() != null)
            {
                Visit(context.version_directive());
            }
            
            return default;
        }

        public object VisitBehavior(CastPreParser.BehaviorContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitConstant_expression(CastPreParser.Constant_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitDefine_directive(CastPreParser.Define_directiveContext context)
        {
            string macroName = context.macro_name().GetText();
            string macroValue = context.macro_text().GetText().Trim();
            Macros[macroName] = macroValue;
            
            _rewriter.Delete(context.Start, context.Stop);
            return default;
        }

        public object VisitElif_directive(CastPreParser.Elif_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitElse_directive(CastPreParser.Else_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitEndif_directive(CastPreParser.Endif_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitError_directive(CastPreParser.Error_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitError_message(CastPreParser.Error_messageContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitExtension_directive(CastPreParser.Extension_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitExtension_name(CastPreParser.Extension_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitGroup_of_lines(CastPreParser.Group_of_linesContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitIf_directive(CastPreParser.If_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitIfdef_directive(CastPreParser.Ifdef_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitIfndef_directive(CastPreParser.Ifndef_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitLine_directive(CastPreParser.Line_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitLine_expression(CastPreParser.Line_expressionContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitMacro_esc_newline(CastPreParser.Macro_esc_newlineContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitMacro_identifier(CastPreParser.Macro_identifierContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitMacro_name(CastPreParser.Macro_nameContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitMacro_text(CastPreParser.Macro_textContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitMacro_text_(CastPreParser.Macro_text_Context context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitNumber(CastPreParser.NumberContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitOff(CastPreParser.OffContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitOn(CastPreParser.OnContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPragma_debug(CastPreParser.Pragma_debugContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPragma_directive(CastPreParser.Pragma_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPragma_optimize(CastPreParser.Pragma_optimizeContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitProfile(CastPreParser.ProfileContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitProgram_text(CastPreParser.Program_textContext context)
        {
            return default;
        }

        public object VisitStdgl(CastPreParser.StdglContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitUndef_directive(CastPreParser.Undef_directiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public object VisitVersion_directive(CastPreParser.Version_directiveContext context)
        {
            if (context.profile() != null)
            {
                Version = context.number().GetText();
                Profile = context.profile().GetText();
            }
            else
            {
                Version = $"{context.number().GetText()}";
            }

            _rewriter.InsertAfter(context.number().Stop, "\n");
            return default;
        }
    }
}
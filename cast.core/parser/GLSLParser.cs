using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.parser.programs;
using cast.core.visitor;
using cast.core.visitor.configuration;

namespace cast.core.parser
{
    public class GlslParser
    {
        private readonly ErrorLogger _logger;
        private readonly Scope _scope;

        public GlslParser()
        {
            _scope = new Scope();
            _scope.Define(new SpaceSymbol("Screen"));
            _scope.Define(new SpaceSymbol("Local"));
            _scope.Define(new SpaceSymbol("Model"));
            _scope.Define(new SpaceSymbol("View"));
            _scope.Define(new SpaceSymbol("World"));
            _scope.Define(new SpaceSymbol("Projection"));
            _scope.Define(new SpaceSymbol("Color"));
            
            _scope.Define(new TypeSymbol("vec4", 1, true));
            _scope.Define(new TypeSymbol("vec3", 1, true));
            _scope.Define(new TypeSymbol("vec2", 1, true));
            _scope.Define(new TypeSymbol("mat4", 2, true));
            _scope.Define(new TypeSymbol("void", 0, false));
            _scope.Define(new TypeSymbol("int", 0, false));
            _scope.Define(new TypeSymbol("uint", 0, false));
            _scope.Define(new TypeSymbol("float", 0, false));
            _scope.Define(new TypeSymbol("sampler2D", 0, false));
            
            _logger = new ErrorLogger();
        }

        public List<string> GetLogs()
        {
            return _logger.Errors;
        }

        /// <summary>
        /// Reads Cast Input Source and transpiles to GLSL
        /// </summary>
        /// <returns></returns>
        public GlslShaderProgram Parse(string castInput)
        {
            ICharStream rawStream = CharStreams.fromString(castInput);
            CastLexer lexer = new CastLexer(rawStream);
            lexer.RemoveErrorListeners();
            // lexer.AddErrorListener(new CastErrorListener(_logger));
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer, CastLexer.DIRECTIVES);
            CastPreParser castPreParser = new CastPreParser(commonTokenStream);
            castPreParser.RemoveErrorListeners();
            castPreParser.AddErrorListener(new CastErrorListener(_logger));
            GlslMacroPreProcessor glslMacroPreProcessor = new GlslMacroPreProcessor(commonTokenStream);
            glslMacroPreProcessor.Visit(castPreParser.translation_unit());
            
            castInput = glslMacroPreProcessor.GetText();

            foreach (var macro in glslMacroPreProcessor.Macros)
            {
                string pattern = $@"\b{Regex.Escape(macro.Key)}\b";
                castInput = Regex.Replace(castInput, pattern, macro.Value);
            }
            
            ICharStream mainStream = CharStreams.fromString(castInput);
            CastLexer mainLexer = new CastLexer(mainStream);
            mainLexer.RemoveErrorListeners();
            // mainLexer.AddErrorListener(new CastErrorListener(_logger));
            
            CommonTokenStream mainTokens = new CommonTokenStream(mainLexer);

            CastParser mainParser = new CastParser(mainTokens);
            mainParser.RemoveErrorListeners();
            mainParser.AddErrorListener(new CastErrorListener(_logger));
            var translationUnit = mainParser.translation_unit();

            DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(_scope, _logger);
            declarationPassVisitor.Visit(translationUnit);

            SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(_scope, _logger);
            semanticPassVisitor.Visit(translationUnit);

            if (_logger.HasErrors)
            {
                _logger.Print();
            }

            GLSLShaderConfiguration configuration = new GLSLShaderConfiguration
            {
                Version = int.Parse(glslMacroPreProcessor.Version)
            };
            
            GlslPassVisitor glslPassVisitor = new GlslPassVisitor(_scope);
            return new GlslShaderProgram()
            {
                Configuration = configuration,
                Shader = glslPassVisitor.Visit(translationUnit),
                Inputs = declarationPassVisitor.Inputs,
                Outputs = declarationPassVisitor.Outputs,
                Uniforms = declarationPassVisitor.Uniforms,
            };
        }
    }
}
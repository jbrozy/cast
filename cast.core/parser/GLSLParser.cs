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

        public bool PreserveLineBreaks { get; set; } = true;

        public GlslParser()
        {
            _scope = new Scope();
            _scope.Define(new SpaceSymbol("Local"));
            _scope.Define(new SpaceSymbol("Model"));
            _scope.Define(new SpaceSymbol("World"));
            _scope.Define(new SpaceSymbol("View"));
            _scope.Define(new SpaceSymbol("Tangent"));
            _scope.Define(new SpaceSymbol("Clip"));
            _scope.Define(new SpaceSymbol("NDC"));
            _scope.Define(new SpaceSymbol("Screen"));
            
            _scope.Define(new SpaceSymbol("Color"));
            _scope.Define(new SpaceSymbol("UV"));
            
            _scope.Define(new TypeSymbol("point2", "vec2", 1, true));
            _scope.Define(new TypeSymbol("point3", "vec3", 1, true));
            _scope.Define(new TypeSymbol("point4", "vec4", 1, true));
            
            _scope.Define(new TypeSymbol("vec4", 1, true));
            _scope.Define(new TypeSymbol("vec3", 1, true));
            _scope.Define(new TypeSymbol("vec2", 1, true));
            
            _scope.Define(new TypeSymbol("mat4", 2, true));
            _scope.Define(new TypeSymbol("mat3", 1, true));
            _scope.Define(new TypeSymbol("mat2", 0, true));
            _scope.Define(new TypeSymbol("void", 0, false));
            _scope.Define(new TypeSymbol("bool", 0, false));
            _scope.Define(new TypeSymbol("int", 0, false));
            _scope.Define(new TypeSymbol("uint", 0, false));
            _scope.Define(new TypeSymbol("float", 0, false));
            _scope.Define(new TypeSymbol("bvec2", 0, true));
            _scope.Define(new TypeSymbol("bvec3", 0, true));
            _scope.Define(new TypeSymbol("bvec4", 0, true));
            _scope.Define(new TypeSymbol("sampler2D", 0, false));
            _scope.Define(new TypeSymbol("sampler3D", 0, false));
            _scope.Define(new TypeSymbol("samplerCube", 0, false));
            
            RegisterConstant("gl_Position", "vec4", "Clip");
            RegisterConstant("gl_PointSize",  "float");
            RegisterConstant("gl_VertexID",   "int");
            RegisterConstant("gl_InstanceID", "int");
            
            RegisterConstant("gl_FragCoord",  "vec4",  "Screen");
            RegisterConstant("gl_FrontFacing","bool");
            RegisterConstant("gl_PointCoord", "vec2", "Local");
            RegisterConstant("gl_FragDepth",  "float");
            
            _logger = new ErrorLogger();
        }

        private void RegisterConstant(string name, string type, params string[] spaces)
        {
            TypeSymbol typeSymbol = _scope[type] as TypeSymbol;
            List<SpaceSymbol> spaceSymbols = new List<SpaceSymbol>();
            
            foreach (string space in spaces)
            {
                spaceSymbols.Add(_scope[space] as SpaceSymbol);
            }

            CastType internalType = new CastType(typeSymbol, spaceSymbols);
            _scope.Define(new VariableSymbol(name, internalType));
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

            // extract sampler payload types before stripping brackets
            var samplerPayloads = new Dictionary<string, string>();
            castInput = Regex.Replace(castInput,
                @"\b(sampler2D|sampler3D|samplerCube)<([^>]+(?:<[^>]+>)?)>\s+(\w+)",
                m =>
                {
                    string samplerType = m.Groups[1].Value;
                    string payload = m.Groups[2].Value;
                    string varName = m.Groups[3].Value;
                    samplerPayloads[varName] = payload;
                    return $"{samplerType} {varName}";
                });
            
            _logger.SetSource(castInput);
            ICharStream mainStream = CharStreams.fromString(castInput);
            CastLexer mainLexer = new CastLexer(mainStream);
            mainLexer.RemoveErrorListeners();
            
            CommonTokenStream mainTokens = new CommonTokenStream(mainLexer);

            CastParser mainParser = new CastParser(mainTokens);
            mainParser.RemoveErrorListeners();
            mainParser.AddErrorListener(new CastErrorListener(_logger));
            var translationUnit = mainParser.translation_unit();

            DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(_scope, _logger, samplerPayloads);
            declarationPassVisitor.Visit(translationUnit);

            SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(_scope, _logger);
            semanticPassVisitor.Visit(translationUnit);

            GLSLShaderConfiguration configuration = new GLSLShaderConfiguration
            {
                Version = int.Parse(glslMacroPreProcessor.Version),
                Profile = glslMacroPreProcessor.Profile,
                PreserveLineBreaks = PreserveLineBreaks
            };
            
            GlslPassVisitor glslPassVisitor = new GlslPassVisitor(_scope, mainTokens, configuration);
            return new GlslShaderProgram()
            {
                Configuration = configuration,
                Shader = glslPassVisitor.Visit(translationUnit),
                Inputs = declarationPassVisitor.Inputs,
                Outputs = declarationPassVisitor.Outputs,
                Uniforms = declarationPassVisitor.Uniforms,
                Textures = declarationPassVisitor.Textures,
            };
        }
    }
}
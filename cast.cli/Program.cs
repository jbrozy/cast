using System.Text.RegularExpressions;
using Antlr4.Runtime;
using cast.core;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.registry;
using cast.core.visitor;

string input = """
               #version 330 core
               
               in vec3 a;
               out vec3 b;
               
               void main() {
                    a = 6;
               }
               
               """;

ICharStream rawStream = CharStreams.fromString(input);
CastLexer lexer = new CastLexer(rawStream);
CommonTokenStream commonTokenStream = new CommonTokenStream(lexer, CastLexer.DIRECTIVES);
CastPreParser castPreParser = new CastPreParser(commonTokenStream);
MacroPreProcessor macroPreProcessor = new MacroPreProcessor(commonTokenStream);
macroPreProcessor.Visit(castPreParser.translation_unit());

input = macroPreProcessor.GetText();

foreach (var macro in macroPreProcessor.Macros)
{
    string pattern = $@"\b{Regex.Escape(macro.Key)}\b";
    input = Regex.Replace(input, pattern, macro.Value);
}

ICharStream mainStream = CharStreams.fromString(input);
CastLexer mainLexer = new CastLexer(mainStream);
CommonTokenStream mainTokens = new CommonTokenStream(mainLexer);

Registry.Setup();
CastParser mainParser = new CastParser(mainTokens);
// 'sampler';
// 'sampler1D';
// 'sampler1DArray';
// 'sampler1DArrayShadow';
// 'sampler1DShadow';
// 'sampler2D';
// 'sampler2DArray';
// 'sampler2DArrayShadow';
// 'sampler2DMS';
// 'sampler2DMSArray';
// 'sampler2DRect';
// 'sampler2DRectShadow';
// 'sampler2DShadow';
// 'sampler3D';
// 'samplerBuffer';
// 'samplerCube';
// 'samplerCubeArray';
// 'samplerCubeArrayShadow';
// 'samplerCubeShadow';
// 'samplerShadow';

Scope scope = new Scope();
scope.Define(new SpaceSymbol("Model"));
scope.Define(new SpaceSymbol("View"));
scope.Define(new SpaceSymbol("World"));
scope.Define(new SpaceSymbol("Color"));

scope.Define(new TypeSymbol("vec4", 1, true));
scope.Define(new TypeSymbol("vec3", 1, true));
scope.Define(new TypeSymbol("vec2", 1, true));
scope.Define(new TypeSymbol("mat4", 2, true));
scope.Define(new TypeSymbol("void", 0, false));
scope.Define(new TypeSymbol("int", 0, false));
scope.Define(new TypeSymbol("uint", 0, false));
scope.Define(new TypeSymbol("float", 0, false));
scope.Define(new TypeSymbol("sampler2D", 0, false));

var translationUnit = mainParser.translation_unit();

ErrorLogger logger = new ErrorLogger();

DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(scope, logger);
declarationPassVisitor.Visit(translationUnit);

SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(scope, logger);
semanticPassVisitor.Visit(translationUnit);

if (logger.HasErrors)
{
    logger.Print();
}

GlslPassVisitor glslPassVisitor = new GlslPassVisitor(scope);
Console.WriteLine(glslPassVisitor.Visit(translationUnit));
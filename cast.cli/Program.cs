using Antlr4.Runtime;
using cast.core;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.registry;
using cast.core.visitor;

string input = """
               void main() {
                    vec4<Model> a;
                    vec4<View> b;
                    vec4<Model> c = a * b;
               }
               
               """;

Registry.Setup();

AntlrInputStream inputStream = new AntlrInputStream(input);
CastLexer lexer = new CastLexer(inputStream);
CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
CastParser parser = new CastParser(commonTokenStream);

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

scope.Define(new TypeSymbol("vec4", 1, true));
scope.Define(new TypeSymbol("vec3", 1, true));
scope.Define(new TypeSymbol("vec2", 1, true));
scope.Define(new TypeSymbol("mat4", 2, true));
scope.Define(new TypeSymbol("void", 0, false));
scope.Define(new TypeSymbol("int", 0, false));
scope.Define(new TypeSymbol("uint", 0, false));
scope.Define(new TypeSymbol("float", 0, false));

var translationUnit = parser.translation_unit();

ErrorLogger logger = new ErrorLogger();

DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(scope, logger);
declarationPassVisitor.Visit(translationUnit);

SemanticPassVisitor semanticPassVisitor = new SemanticPassVisitor(scope, logger);
semanticPassVisitor.Visit(translationUnit);

if (logger.HasErrors)
{
    logger.Print();
}
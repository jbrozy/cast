using Antlr4.Runtime;
using cast.core;
using cast.core.models;
using cast.core.visitor;

string input = File.ReadAllText("input.txt");
AntlrInputStream inputStream = new AntlrInputStream(input);
CastLexer lexer = new CastLexer(inputStream);
CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
CastParser parser = new CastParser(commonTokenStream);

Scope scope = new Scope();
scope.Add(new TypeSymbol("vec4", 1, true));
scope.Add(new TypeSymbol("vec3", 1, true));
scope.Add(new TypeSymbol("vec2", 1, true));
scope.Add(new TypeSymbol("mat4", 2, true));
scope.Add(new TypeSymbol("void", 0, false));
scope.Add(new TypeSymbol("int", 0, false));
scope.Add(new TypeSymbol("uint", 0, false));
scope.Add(new TypeSymbol("float", 0, false));

DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(scope);
declarationPassVisitor.Visit(parser.translation_unit());
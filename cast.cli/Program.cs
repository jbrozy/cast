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
DeclarationPassVisitor declarationPassVisitor = new DeclarationPassVisitor(scope);
declarationPassVisitor.Visit(parser.translation_unit());
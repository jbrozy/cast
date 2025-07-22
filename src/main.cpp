#include "ANTLRInputStream.h"
#include <iostream>

#include "CastLexer.h"
#include "CastParser.h"
#include "CastParserBaseVisitor.h"

std::string unescape_newlines(std::string s) {
    size_t pos = 0;
    while ((pos = s.find("\\n", pos)) != std::string::npos) {
        s.replace(pos, 2, "\n");
        ++pos;
    }
    return s;
}

int main(int argc, char **argv) {
  std::string file {argv[1]};
  std::ifstream input;
  input.open(file);
  antlr4::ANTLRInputStream stream(input);

  CastLexer lexer{&stream};
  antlr4::CommonTokenStream tokenStream(&lexer);
  CastParser parser(&tokenStream);


  antlr4::tree::ParseTree *tree = parser.program();
  std::string treeview = unescape_newlines(tree->toStringTree(&parser));
  std::cout << treeview << std::endl;

  CastParserBaseVisitor visitor;
  visitor.visit(tree);
}

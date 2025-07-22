
// Generated from CastLexer.g4 by ANTLR 4.13.2

#pragma once


#include "antlr4-runtime.h"




class  CastLexer : public antlr4::Lexer {
public:
  enum {
    INPUT_KW = 1, OUTPUT_KW = 2, INTERNAL = 3, STRUCT = 4, FN = 5, INT_KW = 6, 
    FLOAT_KW = 7, RETURN = 8, ARROW = 9, LPAREN = 10, RPAREN = 11, LBRACE = 12, 
    RBRACE = 13, COMMA = 14, EQ = 15, OR = 16, AND = 17, XOR = 18, NOR = 19, 
    MULTIPLY = 20, DIVIDE = 21, ADDITION = 22, SUBTRACTION = 23, LBRACKET = 24, 
    RBRACKET = 25, OPERATOR = 26, BIN_OPERATOR = 27, INTEGER = 28, FLOAT = 29, 
    NEW_LINE = 30, WS = 31, IDENTIFIER = 32
  };

  explicit CastLexer(antlr4::CharStream *input);

  ~CastLexer() override;


  std::string getGrammarFileName() const override;

  const std::vector<std::string>& getRuleNames() const override;

  const std::vector<std::string>& getChannelNames() const override;

  const std::vector<std::string>& getModeNames() const override;

  const antlr4::dfa::Vocabulary& getVocabulary() const override;

  antlr4::atn::SerializedATNView getSerializedATN() const override;

  const antlr4::atn::ATN& getATN() const override;

  // By default the static state used to implement the lexer is lazily initialized during the first
  // call to the constructor. You can call this function if you wish to initialize the static state
  // ahead of time.
  static void initialize();

private:

  // Individual action functions triggered by action() above.

  // Individual semantic predicate functions triggered by sempred() above.

};


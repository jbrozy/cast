
// Generated from CastLexer.g4 by ANTLR 4.13.2


#include "CastLexer.h"


using namespace antlr4;



using namespace antlr4;

namespace {

struct CastLexerStaticData final {
  CastLexerStaticData(std::vector<std::string> ruleNames,
                          std::vector<std::string> channelNames,
                          std::vector<std::string> modeNames,
                          std::vector<std::string> literalNames,
                          std::vector<std::string> symbolicNames)
      : ruleNames(std::move(ruleNames)), channelNames(std::move(channelNames)),
        modeNames(std::move(modeNames)), literalNames(std::move(literalNames)),
        symbolicNames(std::move(symbolicNames)),
        vocabulary(this->literalNames, this->symbolicNames) {}

  CastLexerStaticData(const CastLexerStaticData&) = delete;
  CastLexerStaticData(CastLexerStaticData&&) = delete;
  CastLexerStaticData& operator=(const CastLexerStaticData&) = delete;
  CastLexerStaticData& operator=(CastLexerStaticData&&) = delete;

  std::vector<antlr4::dfa::DFA> decisionToDFA;
  antlr4::atn::PredictionContextCache sharedContextCache;
  const std::vector<std::string> ruleNames;
  const std::vector<std::string> channelNames;
  const std::vector<std::string> modeNames;
  const std::vector<std::string> literalNames;
  const std::vector<std::string> symbolicNames;
  const antlr4::dfa::Vocabulary vocabulary;
  antlr4::atn::SerializedATNView serializedATN;
  std::unique_ptr<antlr4::atn::ATN> atn;
};

::antlr4::internal::OnceFlag castlexerLexerOnceFlag;
#if ANTLR4_USE_THREAD_LOCAL_CACHE
static thread_local
#endif
std::unique_ptr<CastLexerStaticData> castlexerLexerStaticData = nullptr;

void castlexerLexerInitialize() {
#if ANTLR4_USE_THREAD_LOCAL_CACHE
  if (castlexerLexerStaticData != nullptr) {
    return;
  }
#else
  assert(castlexerLexerStaticData == nullptr);
#endif
  auto staticData = std::make_unique<CastLexerStaticData>(
    std::vector<std::string>{
      "INPUT_KW", "OUTPUT_KW", "INTERNAL", "STRUCT", "FN", "INT_KW", "FLOAT_KW", 
      "RETURN", "ARROW", "LPAREN", "RPAREN", "LBRACE", "RBRACE", "COMMA", 
      "EQ", "OR", "AND", "XOR", "NOR", "MULTIPLY", "DIVIDE", "ADDITION", 
      "SUBTRACTION", "LBRACKET", "RBRACKET", "OPERATOR", "BIN_OPERATOR", 
      "INTEGER", "FLOAT", "NEW_LINE", "WS", "IDENTIFIER"
    },
    std::vector<std::string>{
      "DEFAULT_TOKEN_CHANNEL", "HIDDEN"
    },
    std::vector<std::string>{
      "DEFAULT_MODE"
    },
    std::vector<std::string>{
      "", "'@input'", "'@output'", "'@internal'", "'struct'", "'fn'", "'int'", 
      "'float'", "'return'", "'->'", "'('", "')'", "'{'", "'}'", "','", 
      "'='", "'|'", "'&'", "'^'", "'^|'", "'*'", "'/'", "'+'", "'-'", "'['", 
      "']'"
    },
    std::vector<std::string>{
      "", "INPUT_KW", "OUTPUT_KW", "INTERNAL", "STRUCT", "FN", "INT_KW", 
      "FLOAT_KW", "RETURN", "ARROW", "LPAREN", "RPAREN", "LBRACE", "RBRACE", 
      "COMMA", "EQ", "OR", "AND", "XOR", "NOR", "MULTIPLY", "DIVIDE", "ADDITION", 
      "SUBTRACTION", "LBRACKET", "RBRACKET", "OPERATOR", "BIN_OPERATOR", 
      "INTEGER", "FLOAT", "NEW_LINE", "WS", "IDENTIFIER"
    }
  );
  static const int32_t serializedATNSegment[] = {
  	4,0,32,214,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
  	6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
  	7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
  	7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
  	7,28,2,29,7,29,2,30,7,30,2,31,7,31,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,
  	1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,3,
  	1,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,
  	6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,9,1,9,1,10,1,10,1,11,
  	1,11,1,12,1,12,1,13,1,13,1,14,1,14,1,15,1,15,1,16,1,16,1,17,1,17,1,18,
  	1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,22,1,22,1,23,1,23,1,24,1,24,
  	1,25,1,25,1,25,1,25,3,25,158,8,25,1,26,1,26,1,26,1,26,3,26,164,8,26,1,
  	27,1,27,4,27,168,8,27,11,27,12,27,169,1,28,4,28,173,8,28,11,28,12,28,
  	174,1,28,1,28,4,28,179,8,28,11,28,12,28,180,3,28,183,8,28,1,28,1,28,3,
  	28,187,8,28,1,28,4,28,190,8,28,11,28,12,28,191,3,28,194,8,28,1,29,3,29,
  	197,8,29,1,29,1,29,1,30,4,30,202,8,30,11,30,12,30,203,1,30,1,30,1,31,
  	1,31,5,31,210,8,31,10,31,12,31,213,9,31,0,0,32,1,1,3,2,5,3,7,4,9,5,11,
  	6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,
  	37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,
  	30,61,31,63,32,1,0,7,1,0,49,57,1,0,48,57,2,0,69,69,101,101,2,0,43,43,
  	45,45,2,0,9,9,32,32,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,
  	229,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,
  	1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,
  	0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,
  	0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,
  	1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,
  	0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,
  	1,65,1,0,0,0,3,72,1,0,0,0,5,80,1,0,0,0,7,90,1,0,0,0,9,97,1,0,0,0,11,100,
  	1,0,0,0,13,104,1,0,0,0,15,110,1,0,0,0,17,117,1,0,0,0,19,120,1,0,0,0,21,
  	122,1,0,0,0,23,124,1,0,0,0,25,126,1,0,0,0,27,128,1,0,0,0,29,130,1,0,0,
  	0,31,132,1,0,0,0,33,134,1,0,0,0,35,136,1,0,0,0,37,138,1,0,0,0,39,141,
  	1,0,0,0,41,143,1,0,0,0,43,145,1,0,0,0,45,147,1,0,0,0,47,149,1,0,0,0,49,
  	151,1,0,0,0,51,157,1,0,0,0,53,163,1,0,0,0,55,165,1,0,0,0,57,172,1,0,0,
  	0,59,196,1,0,0,0,61,201,1,0,0,0,63,207,1,0,0,0,65,66,5,64,0,0,66,67,5,
  	105,0,0,67,68,5,110,0,0,68,69,5,112,0,0,69,70,5,117,0,0,70,71,5,116,0,
  	0,71,2,1,0,0,0,72,73,5,64,0,0,73,74,5,111,0,0,74,75,5,117,0,0,75,76,5,
  	116,0,0,76,77,5,112,0,0,77,78,5,117,0,0,78,79,5,116,0,0,79,4,1,0,0,0,
  	80,81,5,64,0,0,81,82,5,105,0,0,82,83,5,110,0,0,83,84,5,116,0,0,84,85,
  	5,101,0,0,85,86,5,114,0,0,86,87,5,110,0,0,87,88,5,97,0,0,88,89,5,108,
  	0,0,89,6,1,0,0,0,90,91,5,115,0,0,91,92,5,116,0,0,92,93,5,114,0,0,93,94,
  	5,117,0,0,94,95,5,99,0,0,95,96,5,116,0,0,96,8,1,0,0,0,97,98,5,102,0,0,
  	98,99,5,110,0,0,99,10,1,0,0,0,100,101,5,105,0,0,101,102,5,110,0,0,102,
  	103,5,116,0,0,103,12,1,0,0,0,104,105,5,102,0,0,105,106,5,108,0,0,106,
  	107,5,111,0,0,107,108,5,97,0,0,108,109,5,116,0,0,109,14,1,0,0,0,110,111,
  	5,114,0,0,111,112,5,101,0,0,112,113,5,116,0,0,113,114,5,117,0,0,114,115,
  	5,114,0,0,115,116,5,110,0,0,116,16,1,0,0,0,117,118,5,45,0,0,118,119,5,
  	62,0,0,119,18,1,0,0,0,120,121,5,40,0,0,121,20,1,0,0,0,122,123,5,41,0,
  	0,123,22,1,0,0,0,124,125,5,123,0,0,125,24,1,0,0,0,126,127,5,125,0,0,127,
  	26,1,0,0,0,128,129,5,44,0,0,129,28,1,0,0,0,130,131,5,61,0,0,131,30,1,
  	0,0,0,132,133,5,124,0,0,133,32,1,0,0,0,134,135,5,38,0,0,135,34,1,0,0,
  	0,136,137,5,94,0,0,137,36,1,0,0,0,138,139,5,94,0,0,139,140,5,124,0,0,
  	140,38,1,0,0,0,141,142,5,42,0,0,142,40,1,0,0,0,143,144,5,47,0,0,144,42,
  	1,0,0,0,145,146,5,43,0,0,146,44,1,0,0,0,147,148,5,45,0,0,148,46,1,0,0,
  	0,149,150,5,91,0,0,150,48,1,0,0,0,151,152,5,93,0,0,152,50,1,0,0,0,153,
  	158,3,39,19,0,154,158,3,41,20,0,155,158,3,43,21,0,156,158,3,45,22,0,157,
  	153,1,0,0,0,157,154,1,0,0,0,157,155,1,0,0,0,157,156,1,0,0,0,158,52,1,
  	0,0,0,159,164,3,31,15,0,160,164,3,33,16,0,161,164,3,35,17,0,162,164,3,
  	37,18,0,163,159,1,0,0,0,163,160,1,0,0,0,163,161,1,0,0,0,163,162,1,0,0,
  	0,164,54,1,0,0,0,165,167,7,0,0,0,166,168,7,1,0,0,167,166,1,0,0,0,168,
  	169,1,0,0,0,169,167,1,0,0,0,169,170,1,0,0,0,170,56,1,0,0,0,171,173,7,
  	1,0,0,172,171,1,0,0,0,173,174,1,0,0,0,174,172,1,0,0,0,174,175,1,0,0,0,
  	175,182,1,0,0,0,176,178,5,46,0,0,177,179,7,1,0,0,178,177,1,0,0,0,179,
  	180,1,0,0,0,180,178,1,0,0,0,180,181,1,0,0,0,181,183,1,0,0,0,182,176,1,
  	0,0,0,182,183,1,0,0,0,183,193,1,0,0,0,184,186,7,2,0,0,185,187,7,3,0,0,
  	186,185,1,0,0,0,186,187,1,0,0,0,187,189,1,0,0,0,188,190,7,1,0,0,189,188,
  	1,0,0,0,190,191,1,0,0,0,191,189,1,0,0,0,191,192,1,0,0,0,192,194,1,0,0,
  	0,193,184,1,0,0,0,193,194,1,0,0,0,194,58,1,0,0,0,195,197,5,13,0,0,196,
  	195,1,0,0,0,196,197,1,0,0,0,197,198,1,0,0,0,198,199,5,10,0,0,199,60,1,
  	0,0,0,200,202,7,4,0,0,201,200,1,0,0,0,202,203,1,0,0,0,203,201,1,0,0,0,
  	203,204,1,0,0,0,204,205,1,0,0,0,205,206,6,30,0,0,206,62,1,0,0,0,207,211,
  	7,5,0,0,208,210,7,6,0,0,209,208,1,0,0,0,210,213,1,0,0,0,211,209,1,0,0,
  	0,211,212,1,0,0,0,212,64,1,0,0,0,213,211,1,0,0,0,13,0,157,163,169,174,
  	180,182,186,191,193,196,203,211,1,6,0,0
  };
  staticData->serializedATN = antlr4::atn::SerializedATNView(serializedATNSegment, sizeof(serializedATNSegment) / sizeof(serializedATNSegment[0]));

  antlr4::atn::ATNDeserializer deserializer;
  staticData->atn = deserializer.deserialize(staticData->serializedATN);

  const size_t count = staticData->atn->getNumberOfDecisions();
  staticData->decisionToDFA.reserve(count);
  for (size_t i = 0; i < count; i++) { 
    staticData->decisionToDFA.emplace_back(staticData->atn->getDecisionState(i), i);
  }
  castlexerLexerStaticData = std::move(staticData);
}

}

CastLexer::CastLexer(CharStream *input) : Lexer(input) {
  CastLexer::initialize();
  _interpreter = new atn::LexerATNSimulator(this, *castlexerLexerStaticData->atn, castlexerLexerStaticData->decisionToDFA, castlexerLexerStaticData->sharedContextCache);
}

CastLexer::~CastLexer() {
  delete _interpreter;
}

std::string CastLexer::getGrammarFileName() const {
  return "CastLexer.g4";
}

const std::vector<std::string>& CastLexer::getRuleNames() const {
  return castlexerLexerStaticData->ruleNames;
}

const std::vector<std::string>& CastLexer::getChannelNames() const {
  return castlexerLexerStaticData->channelNames;
}

const std::vector<std::string>& CastLexer::getModeNames() const {
  return castlexerLexerStaticData->modeNames;
}

const dfa::Vocabulary& CastLexer::getVocabulary() const {
  return castlexerLexerStaticData->vocabulary;
}

antlr4::atn::SerializedATNView CastLexer::getSerializedATN() const {
  return castlexerLexerStaticData->serializedATN;
}

const atn::ATN& CastLexer::getATN() const {
  return *castlexerLexerStaticData->atn;
}




void CastLexer::initialize() {
#if ANTLR4_USE_THREAD_LOCAL_CACHE
  castlexerLexerInitialize();
#else
  ::antlr4::internal::call_once(castlexerLexerOnceFlag, castlexerLexerInitialize);
#endif
}

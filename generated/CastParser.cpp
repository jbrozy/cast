
// Generated from CastParser.g4 by ANTLR 4.13.2


#include "CastParserListener.h"
#include "CastParserVisitor.h"

#include "CastParser.h"


using namespace antlrcpp;

using namespace antlr4;

namespace {

struct CastParserStaticData final {
  CastParserStaticData(std::vector<std::string> ruleNames,
                        std::vector<std::string> literalNames,
                        std::vector<std::string> symbolicNames)
      : ruleNames(std::move(ruleNames)), literalNames(std::move(literalNames)),
        symbolicNames(std::move(symbolicNames)),
        vocabulary(this->literalNames, this->symbolicNames) {}

  CastParserStaticData(const CastParserStaticData&) = delete;
  CastParserStaticData(CastParserStaticData&&) = delete;
  CastParserStaticData& operator=(const CastParserStaticData&) = delete;
  CastParserStaticData& operator=(CastParserStaticData&&) = delete;

  std::vector<antlr4::dfa::DFA> decisionToDFA;
  antlr4::atn::PredictionContextCache sharedContextCache;
  const std::vector<std::string> ruleNames;
  const std::vector<std::string> literalNames;
  const std::vector<std::string> symbolicNames;
  const antlr4::dfa::Vocabulary vocabulary;
  antlr4::atn::SerializedATNView serializedATN;
  std::unique_ptr<antlr4::atn::ATN> atn;
};

::antlr4::internal::OnceFlag castparserParserOnceFlag;
#if ANTLR4_USE_THREAD_LOCAL_CACHE
static thread_local
#endif
std::unique_ptr<CastParserStaticData> castparserParserStaticData = nullptr;

void castparserParserInitialize() {
#if ANTLR4_USE_THREAD_LOCAL_CACHE
  if (castparserParserStaticData != nullptr) {
    return;
  }
#else
  assert(castparserParserStaticData == nullptr);
#endif
  auto staticData = std::make_unique<CastParserStaticData>(
    std::vector<std::string>{
      "program", "stageIO", "params", "functionCall", "functionDecl", "typeDecl", 
      "dataDecl", "structDecl", "stageDecl", "expression", "additiveExpression", 
      "multiplicativeExpression", "primary", "body", "type", "assignment", 
      "statement"
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
  	4,1,32,203,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,
  	7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,7,
  	14,2,15,7,15,2,16,7,16,1,0,3,0,36,8,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,
  	1,0,1,0,1,0,1,0,5,0,50,8,0,10,0,12,0,53,9,0,1,0,4,0,56,8,0,11,0,12,0,
  	57,1,0,3,0,61,8,0,1,1,1,1,1,2,1,2,1,2,3,2,68,8,2,1,2,1,2,1,2,5,2,73,8,
  	2,10,2,12,2,76,9,2,1,3,1,3,1,3,1,3,1,3,5,3,83,8,3,10,3,12,3,86,9,3,3,
  	3,88,8,3,1,3,1,3,1,4,1,4,1,4,1,4,3,4,96,8,4,1,4,1,4,1,4,3,4,101,8,4,1,
  	4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,1,5,5,5,112,8,5,10,5,12,5,115,9,5,1,5,3,
  	5,118,8,5,1,5,1,5,1,6,1,6,1,6,5,6,125,8,6,10,6,12,6,128,9,6,1,6,1,6,1,
  	7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,10,1,10,1,10,5,10,147,
  	8,10,10,10,12,10,150,9,10,1,11,1,11,1,11,5,11,155,8,11,10,11,12,11,158,
  	9,11,1,12,1,12,1,12,1,12,1,12,1,12,1,12,1,12,3,12,168,8,12,1,13,1,13,
  	3,13,172,8,13,1,13,1,13,1,13,5,13,177,8,13,10,13,12,13,180,9,13,1,13,
  	1,13,3,13,184,8,13,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,16,1,16,1,16,
  	1,16,3,16,197,8,16,3,16,199,8,16,3,16,201,8,16,1,16,0,0,17,0,2,4,6,8,
  	10,12,14,16,18,20,22,24,26,28,30,32,0,4,1,0,1,2,1,0,22,23,1,0,20,21,1,
  	0,6,7,215,0,60,1,0,0,0,2,62,1,0,0,0,4,64,1,0,0,0,6,77,1,0,0,0,8,91,1,
  	0,0,0,10,104,1,0,0,0,12,121,1,0,0,0,14,131,1,0,0,0,16,135,1,0,0,0,18,
  	141,1,0,0,0,20,143,1,0,0,0,22,151,1,0,0,0,24,167,1,0,0,0,26,169,1,0,0,
  	0,28,185,1,0,0,0,30,187,1,0,0,0,32,200,1,0,0,0,34,36,5,3,0,0,35,34,1,
  	0,0,0,35,36,1,0,0,0,36,51,1,0,0,0,37,38,3,8,4,0,38,39,5,30,0,0,39,50,
  	1,0,0,0,40,41,3,32,16,0,41,42,5,30,0,0,42,50,1,0,0,0,43,44,3,16,8,0,44,
  	45,5,30,0,0,45,50,1,0,0,0,46,47,3,14,7,0,47,48,5,30,0,0,48,50,1,0,0,0,
  	49,37,1,0,0,0,49,40,1,0,0,0,49,43,1,0,0,0,49,46,1,0,0,0,50,53,1,0,0,0,
  	51,49,1,0,0,0,51,52,1,0,0,0,52,61,1,0,0,0,53,51,1,0,0,0,54,56,5,30,0,
  	0,55,54,1,0,0,0,56,57,1,0,0,0,57,55,1,0,0,0,57,58,1,0,0,0,58,61,1,0,0,
  	0,59,61,5,0,0,1,60,35,1,0,0,0,60,55,1,0,0,0,60,59,1,0,0,0,61,1,1,0,0,
  	0,62,63,7,0,0,0,63,3,1,0,0,0,64,65,3,28,14,0,65,74,5,32,0,0,66,68,5,14,
  	0,0,67,66,1,0,0,0,67,68,1,0,0,0,68,69,1,0,0,0,69,70,3,28,14,0,70,71,5,
  	32,0,0,71,73,1,0,0,0,72,67,1,0,0,0,73,76,1,0,0,0,74,72,1,0,0,0,74,75,
  	1,0,0,0,75,5,1,0,0,0,76,74,1,0,0,0,77,78,5,32,0,0,78,87,5,10,0,0,79,84,
  	3,18,9,0,80,81,5,14,0,0,81,83,3,18,9,0,82,80,1,0,0,0,83,86,1,0,0,0,84,
  	82,1,0,0,0,84,85,1,0,0,0,85,88,1,0,0,0,86,84,1,0,0,0,87,79,1,0,0,0,87,
  	88,1,0,0,0,88,89,1,0,0,0,89,90,5,11,0,0,90,7,1,0,0,0,91,92,5,5,0,0,92,
  	93,5,32,0,0,93,95,5,10,0,0,94,96,3,4,2,0,95,94,1,0,0,0,95,96,1,0,0,0,
  	96,97,1,0,0,0,97,100,5,11,0,0,98,99,5,9,0,0,99,101,3,28,14,0,100,98,1,
  	0,0,0,100,101,1,0,0,0,101,102,1,0,0,0,102,103,3,26,13,0,103,9,1,0,0,0,
  	104,105,3,28,14,0,105,117,5,32,0,0,106,107,5,9,0,0,107,108,5,24,0,0,108,
  	113,5,32,0,0,109,110,5,14,0,0,110,112,5,32,0,0,111,109,1,0,0,0,112,115,
  	1,0,0,0,113,111,1,0,0,0,113,114,1,0,0,0,114,116,1,0,0,0,115,113,1,0,0,
  	0,116,118,5,25,0,0,117,106,1,0,0,0,117,118,1,0,0,0,118,119,1,0,0,0,119,
  	120,5,30,0,0,120,11,1,0,0,0,121,122,5,12,0,0,122,126,5,30,0,0,123,125,
  	3,10,5,0,124,123,1,0,0,0,125,128,1,0,0,0,126,124,1,0,0,0,126,127,1,0,
  	0,0,127,129,1,0,0,0,128,126,1,0,0,0,129,130,5,13,0,0,130,13,1,0,0,0,131,
  	132,5,4,0,0,132,133,5,32,0,0,133,134,3,12,6,0,134,15,1,0,0,0,135,136,
  	3,2,1,0,136,137,5,10,0,0,137,138,5,32,0,0,138,139,5,11,0,0,139,140,3,
  	12,6,0,140,17,1,0,0,0,141,142,3,20,10,0,142,19,1,0,0,0,143,148,3,22,11,
  	0,144,145,7,1,0,0,145,147,3,22,11,0,146,144,1,0,0,0,147,150,1,0,0,0,148,
  	146,1,0,0,0,148,149,1,0,0,0,149,21,1,0,0,0,150,148,1,0,0,0,151,156,3,
  	24,12,0,152,153,7,2,0,0,153,155,3,24,12,0,154,152,1,0,0,0,155,158,1,0,
  	0,0,156,154,1,0,0,0,156,157,1,0,0,0,157,23,1,0,0,0,158,156,1,0,0,0,159,
  	168,3,6,3,0,160,168,5,32,0,0,161,168,5,28,0,0,162,168,5,29,0,0,163,164,
  	5,10,0,0,164,165,3,18,9,0,165,166,5,11,0,0,166,168,1,0,0,0,167,159,1,
  	0,0,0,167,160,1,0,0,0,167,161,1,0,0,0,167,162,1,0,0,0,167,163,1,0,0,0,
  	168,25,1,0,0,0,169,171,5,12,0,0,170,172,5,30,0,0,171,170,1,0,0,0,171,
  	172,1,0,0,0,172,178,1,0,0,0,173,174,3,32,16,0,174,175,5,30,0,0,175,177,
  	1,0,0,0,176,173,1,0,0,0,177,180,1,0,0,0,178,176,1,0,0,0,178,179,1,0,0,
  	0,179,181,1,0,0,0,180,178,1,0,0,0,181,183,5,13,0,0,182,184,5,30,0,0,183,
  	182,1,0,0,0,183,184,1,0,0,0,184,27,1,0,0,0,185,186,7,3,0,0,186,29,1,0,
  	0,0,187,188,3,28,14,0,188,189,5,32,0,0,189,190,5,15,0,0,190,191,3,18,
  	9,0,191,31,1,0,0,0,192,201,3,30,15,0,193,201,3,6,3,0,194,196,5,8,0,0,
  	195,197,3,24,12,0,196,195,1,0,0,0,196,197,1,0,0,0,197,199,1,0,0,0,198,
  	194,1,0,0,0,198,199,1,0,0,0,199,201,1,0,0,0,200,192,1,0,0,0,200,193,1,
  	0,0,0,200,198,1,0,0,0,201,33,1,0,0,0,23,35,49,51,57,60,67,74,84,87,95,
  	100,113,117,126,148,156,167,171,178,183,196,198,200
  };
  staticData->serializedATN = antlr4::atn::SerializedATNView(serializedATNSegment, sizeof(serializedATNSegment) / sizeof(serializedATNSegment[0]));

  antlr4::atn::ATNDeserializer deserializer;
  staticData->atn = deserializer.deserialize(staticData->serializedATN);

  const size_t count = staticData->atn->getNumberOfDecisions();
  staticData->decisionToDFA.reserve(count);
  for (size_t i = 0; i < count; i++) { 
    staticData->decisionToDFA.emplace_back(staticData->atn->getDecisionState(i), i);
  }
  castparserParserStaticData = std::move(staticData);
}

}

CastParser::CastParser(TokenStream *input) : CastParser(input, antlr4::atn::ParserATNSimulatorOptions()) {}

CastParser::CastParser(TokenStream *input, const antlr4::atn::ParserATNSimulatorOptions &options) : Parser(input) {
  CastParser::initialize();
  _interpreter = new atn::ParserATNSimulator(this, *castparserParserStaticData->atn, castparserParserStaticData->decisionToDFA, castparserParserStaticData->sharedContextCache, options);
}

CastParser::~CastParser() {
  delete _interpreter;
}

const atn::ATN& CastParser::getATN() const {
  return *castparserParserStaticData->atn;
}

std::string CastParser::getGrammarFileName() const {
  return "CastParser.g4";
}

const std::vector<std::string>& CastParser::getRuleNames() const {
  return castparserParserStaticData->ruleNames;
}

const dfa::Vocabulary& CastParser::getVocabulary() const {
  return castparserParserStaticData->vocabulary;
}

antlr4::atn::SerializedATNView CastParser::getSerializedATN() const {
  return castparserParserStaticData->serializedATN;
}


//----------------- ProgramContext ------------------------------------------------------------------

CastParser::ProgramContext::ProgramContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::ProgramContext::INTERNAL() {
  return getToken(CastParser::INTERNAL, 0);
}

std::vector<CastParser::FunctionDeclContext *> CastParser::ProgramContext::functionDecl() {
  return getRuleContexts<CastParser::FunctionDeclContext>();
}

CastParser::FunctionDeclContext* CastParser::ProgramContext::functionDecl(size_t i) {
  return getRuleContext<CastParser::FunctionDeclContext>(i);
}

std::vector<tree::TerminalNode *> CastParser::ProgramContext::NEW_LINE() {
  return getTokens(CastParser::NEW_LINE);
}

tree::TerminalNode* CastParser::ProgramContext::NEW_LINE(size_t i) {
  return getToken(CastParser::NEW_LINE, i);
}

std::vector<CastParser::StatementContext *> CastParser::ProgramContext::statement() {
  return getRuleContexts<CastParser::StatementContext>();
}

CastParser::StatementContext* CastParser::ProgramContext::statement(size_t i) {
  return getRuleContext<CastParser::StatementContext>(i);
}

std::vector<CastParser::StageDeclContext *> CastParser::ProgramContext::stageDecl() {
  return getRuleContexts<CastParser::StageDeclContext>();
}

CastParser::StageDeclContext* CastParser::ProgramContext::stageDecl(size_t i) {
  return getRuleContext<CastParser::StageDeclContext>(i);
}

std::vector<CastParser::StructDeclContext *> CastParser::ProgramContext::structDecl() {
  return getRuleContexts<CastParser::StructDeclContext>();
}

CastParser::StructDeclContext* CastParser::ProgramContext::structDecl(size_t i) {
  return getRuleContext<CastParser::StructDeclContext>(i);
}

tree::TerminalNode* CastParser::ProgramContext::EOF() {
  return getToken(CastParser::EOF, 0);
}


size_t CastParser::ProgramContext::getRuleIndex() const {
  return CastParser::RuleProgram;
}

void CastParser::ProgramContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterProgram(this);
}

void CastParser::ProgramContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitProgram(this);
}


std::any CastParser::ProgramContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitProgram(this);
  else
    return visitor->visitChildren(this);
}

CastParser::ProgramContext* CastParser::program() {
  ProgramContext *_localctx = _tracker.createInstance<ProgramContext>(_ctx, getState());
  enterRule(_localctx, 0, CastParser::RuleProgram);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    setState(60);
    _errHandler->sync(this);
    switch (getInterpreter<atn::ParserATNSimulator>()->adaptivePredict(_input, 4, _ctx)) {
    case 1: {
      enterOuterAlt(_localctx, 1);
      setState(35);
      _errHandler->sync(this);

      _la = _input->LA(1);
      if (_la == CastParser::INTERNAL) {
        setState(34);
        match(CastParser::INTERNAL);
      }
      setState(51);
      _errHandler->sync(this);
      _la = _input->LA(1);
      while ((((_la & ~ 0x3fULL) == 0) &&
        ((1ULL << _la) & 5368709622) != 0)) {
        setState(49);
        _errHandler->sync(this);
        switch (_input->LA(1)) {
          case CastParser::FN: {
            setState(37);
            functionDecl();
            setState(38);
            match(CastParser::NEW_LINE);
            break;
          }

          case CastParser::INT_KW:
          case CastParser::FLOAT_KW:
          case CastParser::RETURN:
          case CastParser::NEW_LINE:
          case CastParser::IDENTIFIER: {
            setState(40);
            statement();
            setState(41);
            match(CastParser::NEW_LINE);
            break;
          }

          case CastParser::INPUT_KW:
          case CastParser::OUTPUT_KW: {
            setState(43);
            stageDecl();
            setState(44);
            match(CastParser::NEW_LINE);
            break;
          }

          case CastParser::STRUCT: {
            setState(46);
            structDecl();
            setState(47);
            match(CastParser::NEW_LINE);
            break;
          }

        default:
          throw NoViableAltException(this);
        }
        setState(53);
        _errHandler->sync(this);
        _la = _input->LA(1);
      }
      break;
    }

    case 2: {
      enterOuterAlt(_localctx, 2);
      setState(55); 
      _errHandler->sync(this);
      _la = _input->LA(1);
      do {
        setState(54);
        match(CastParser::NEW_LINE);
        setState(57); 
        _errHandler->sync(this);
        _la = _input->LA(1);
      } while (_la == CastParser::NEW_LINE);
      break;
    }

    case 3: {
      enterOuterAlt(_localctx, 3);
      setState(59);
      match(CastParser::EOF);
      break;
    }

    default:
      break;
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- StageIOContext ------------------------------------------------------------------

CastParser::StageIOContext::StageIOContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::StageIOContext::INPUT_KW() {
  return getToken(CastParser::INPUT_KW, 0);
}

tree::TerminalNode* CastParser::StageIOContext::OUTPUT_KW() {
  return getToken(CastParser::OUTPUT_KW, 0);
}


size_t CastParser::StageIOContext::getRuleIndex() const {
  return CastParser::RuleStageIO;
}

void CastParser::StageIOContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterStageIO(this);
}

void CastParser::StageIOContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitStageIO(this);
}


std::any CastParser::StageIOContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitStageIO(this);
  else
    return visitor->visitChildren(this);
}

CastParser::StageIOContext* CastParser::stageIO() {
  StageIOContext *_localctx = _tracker.createInstance<StageIOContext>(_ctx, getState());
  enterRule(_localctx, 2, CastParser::RuleStageIO);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(62);
    _la = _input->LA(1);
    if (!(_la == CastParser::INPUT_KW

    || _la == CastParser::OUTPUT_KW)) {
    _errHandler->recoverInline(this);
    }
    else {
      _errHandler->reportMatch(this);
      consume();
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- ParamsContext ------------------------------------------------------------------

CastParser::ParamsContext::ParamsContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

std::vector<CastParser::TypeContext *> CastParser::ParamsContext::type() {
  return getRuleContexts<CastParser::TypeContext>();
}

CastParser::TypeContext* CastParser::ParamsContext::type(size_t i) {
  return getRuleContext<CastParser::TypeContext>(i);
}

std::vector<tree::TerminalNode *> CastParser::ParamsContext::IDENTIFIER() {
  return getTokens(CastParser::IDENTIFIER);
}

tree::TerminalNode* CastParser::ParamsContext::IDENTIFIER(size_t i) {
  return getToken(CastParser::IDENTIFIER, i);
}

std::vector<tree::TerminalNode *> CastParser::ParamsContext::COMMA() {
  return getTokens(CastParser::COMMA);
}

tree::TerminalNode* CastParser::ParamsContext::COMMA(size_t i) {
  return getToken(CastParser::COMMA, i);
}


size_t CastParser::ParamsContext::getRuleIndex() const {
  return CastParser::RuleParams;
}

void CastParser::ParamsContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterParams(this);
}

void CastParser::ParamsContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitParams(this);
}


std::any CastParser::ParamsContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitParams(this);
  else
    return visitor->visitChildren(this);
}

CastParser::ParamsContext* CastParser::params() {
  ParamsContext *_localctx = _tracker.createInstance<ParamsContext>(_ctx, getState());
  enterRule(_localctx, 4, CastParser::RuleParams);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(64);
    type();
    setState(65);
    match(CastParser::IDENTIFIER);
    setState(74);
    _errHandler->sync(this);
    _la = _input->LA(1);
    while ((((_la & ~ 0x3fULL) == 0) &&
      ((1ULL << _la) & 16576) != 0)) {
      setState(67);
      _errHandler->sync(this);

      _la = _input->LA(1);
      if (_la == CastParser::COMMA) {
        setState(66);
        match(CastParser::COMMA);
      }
      setState(69);
      type();
      setState(70);
      match(CastParser::IDENTIFIER);
      setState(76);
      _errHandler->sync(this);
      _la = _input->LA(1);
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- FunctionCallContext ------------------------------------------------------------------

CastParser::FunctionCallContext::FunctionCallContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::FunctionCallContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

tree::TerminalNode* CastParser::FunctionCallContext::LPAREN() {
  return getToken(CastParser::LPAREN, 0);
}

tree::TerminalNode* CastParser::FunctionCallContext::RPAREN() {
  return getToken(CastParser::RPAREN, 0);
}

std::vector<CastParser::ExpressionContext *> CastParser::FunctionCallContext::expression() {
  return getRuleContexts<CastParser::ExpressionContext>();
}

CastParser::ExpressionContext* CastParser::FunctionCallContext::expression(size_t i) {
  return getRuleContext<CastParser::ExpressionContext>(i);
}

std::vector<tree::TerminalNode *> CastParser::FunctionCallContext::COMMA() {
  return getTokens(CastParser::COMMA);
}

tree::TerminalNode* CastParser::FunctionCallContext::COMMA(size_t i) {
  return getToken(CastParser::COMMA, i);
}


size_t CastParser::FunctionCallContext::getRuleIndex() const {
  return CastParser::RuleFunctionCall;
}

void CastParser::FunctionCallContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterFunctionCall(this);
}

void CastParser::FunctionCallContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitFunctionCall(this);
}


std::any CastParser::FunctionCallContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitFunctionCall(this);
  else
    return visitor->visitChildren(this);
}

CastParser::FunctionCallContext* CastParser::functionCall() {
  FunctionCallContext *_localctx = _tracker.createInstance<FunctionCallContext>(_ctx, getState());
  enterRule(_localctx, 6, CastParser::RuleFunctionCall);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(77);
    match(CastParser::IDENTIFIER);
    setState(78);
    match(CastParser::LPAREN);
    setState(87);
    _errHandler->sync(this);

    _la = _input->LA(1);
    if ((((_la & ~ 0x3fULL) == 0) &&
      ((1ULL << _la) & 5100274688) != 0)) {
      setState(79);
      expression();
      setState(84);
      _errHandler->sync(this);
      _la = _input->LA(1);
      while (_la == CastParser::COMMA) {
        setState(80);
        match(CastParser::COMMA);
        setState(81);
        expression();
        setState(86);
        _errHandler->sync(this);
        _la = _input->LA(1);
      }
    }
    setState(89);
    match(CastParser::RPAREN);
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- FunctionDeclContext ------------------------------------------------------------------

CastParser::FunctionDeclContext::FunctionDeclContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::FunctionDeclContext::FN() {
  return getToken(CastParser::FN, 0);
}

tree::TerminalNode* CastParser::FunctionDeclContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

tree::TerminalNode* CastParser::FunctionDeclContext::LPAREN() {
  return getToken(CastParser::LPAREN, 0);
}

tree::TerminalNode* CastParser::FunctionDeclContext::RPAREN() {
  return getToken(CastParser::RPAREN, 0);
}

CastParser::BodyContext* CastParser::FunctionDeclContext::body() {
  return getRuleContext<CastParser::BodyContext>(0);
}

CastParser::ParamsContext* CastParser::FunctionDeclContext::params() {
  return getRuleContext<CastParser::ParamsContext>(0);
}

tree::TerminalNode* CastParser::FunctionDeclContext::ARROW() {
  return getToken(CastParser::ARROW, 0);
}

CastParser::TypeContext* CastParser::FunctionDeclContext::type() {
  return getRuleContext<CastParser::TypeContext>(0);
}


size_t CastParser::FunctionDeclContext::getRuleIndex() const {
  return CastParser::RuleFunctionDecl;
}

void CastParser::FunctionDeclContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterFunctionDecl(this);
}

void CastParser::FunctionDeclContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitFunctionDecl(this);
}


std::any CastParser::FunctionDeclContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitFunctionDecl(this);
  else
    return visitor->visitChildren(this);
}

CastParser::FunctionDeclContext* CastParser::functionDecl() {
  FunctionDeclContext *_localctx = _tracker.createInstance<FunctionDeclContext>(_ctx, getState());
  enterRule(_localctx, 8, CastParser::RuleFunctionDecl);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(91);
    match(CastParser::FN);
    setState(92);
    match(CastParser::IDENTIFIER);
    setState(93);
    match(CastParser::LPAREN);
    setState(95);
    _errHandler->sync(this);

    _la = _input->LA(1);
    if (_la == CastParser::INT_KW

    || _la == CastParser::FLOAT_KW) {
      setState(94);
      params();
    }
    setState(97);
    match(CastParser::RPAREN);
    setState(100);
    _errHandler->sync(this);

    _la = _input->LA(1);
    if (_la == CastParser::ARROW) {
      setState(98);
      match(CastParser::ARROW);
      setState(99);
      type();
    }
    setState(102);
    body();
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- TypeDeclContext ------------------------------------------------------------------

CastParser::TypeDeclContext::TypeDeclContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

CastParser::TypeContext* CastParser::TypeDeclContext::type() {
  return getRuleContext<CastParser::TypeContext>(0);
}

std::vector<tree::TerminalNode *> CastParser::TypeDeclContext::IDENTIFIER() {
  return getTokens(CastParser::IDENTIFIER);
}

tree::TerminalNode* CastParser::TypeDeclContext::IDENTIFIER(size_t i) {
  return getToken(CastParser::IDENTIFIER, i);
}

tree::TerminalNode* CastParser::TypeDeclContext::NEW_LINE() {
  return getToken(CastParser::NEW_LINE, 0);
}

tree::TerminalNode* CastParser::TypeDeclContext::ARROW() {
  return getToken(CastParser::ARROW, 0);
}

tree::TerminalNode* CastParser::TypeDeclContext::LBRACKET() {
  return getToken(CastParser::LBRACKET, 0);
}

tree::TerminalNode* CastParser::TypeDeclContext::RBRACKET() {
  return getToken(CastParser::RBRACKET, 0);
}

std::vector<tree::TerminalNode *> CastParser::TypeDeclContext::COMMA() {
  return getTokens(CastParser::COMMA);
}

tree::TerminalNode* CastParser::TypeDeclContext::COMMA(size_t i) {
  return getToken(CastParser::COMMA, i);
}


size_t CastParser::TypeDeclContext::getRuleIndex() const {
  return CastParser::RuleTypeDecl;
}

void CastParser::TypeDeclContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterTypeDecl(this);
}

void CastParser::TypeDeclContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitTypeDecl(this);
}


std::any CastParser::TypeDeclContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitTypeDecl(this);
  else
    return visitor->visitChildren(this);
}

CastParser::TypeDeclContext* CastParser::typeDecl() {
  TypeDeclContext *_localctx = _tracker.createInstance<TypeDeclContext>(_ctx, getState());
  enterRule(_localctx, 10, CastParser::RuleTypeDecl);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(104);
    type();
    setState(105);
    match(CastParser::IDENTIFIER);
    setState(117);
    _errHandler->sync(this);

    _la = _input->LA(1);
    if (_la == CastParser::ARROW) {
      setState(106);
      match(CastParser::ARROW);
      setState(107);
      match(CastParser::LBRACKET);
      setState(108);
      match(CastParser::IDENTIFIER);
      setState(113);
      _errHandler->sync(this);
      _la = _input->LA(1);
      while (_la == CastParser::COMMA) {
        setState(109);
        match(CastParser::COMMA);
        setState(110);
        match(CastParser::IDENTIFIER);
        setState(115);
        _errHandler->sync(this);
        _la = _input->LA(1);
      }
      setState(116);
      match(CastParser::RBRACKET);
    }
    setState(119);
    match(CastParser::NEW_LINE);
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- DataDeclContext ------------------------------------------------------------------

CastParser::DataDeclContext::DataDeclContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::DataDeclContext::LBRACE() {
  return getToken(CastParser::LBRACE, 0);
}

tree::TerminalNode* CastParser::DataDeclContext::NEW_LINE() {
  return getToken(CastParser::NEW_LINE, 0);
}

tree::TerminalNode* CastParser::DataDeclContext::RBRACE() {
  return getToken(CastParser::RBRACE, 0);
}

std::vector<CastParser::TypeDeclContext *> CastParser::DataDeclContext::typeDecl() {
  return getRuleContexts<CastParser::TypeDeclContext>();
}

CastParser::TypeDeclContext* CastParser::DataDeclContext::typeDecl(size_t i) {
  return getRuleContext<CastParser::TypeDeclContext>(i);
}


size_t CastParser::DataDeclContext::getRuleIndex() const {
  return CastParser::RuleDataDecl;
}

void CastParser::DataDeclContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterDataDecl(this);
}

void CastParser::DataDeclContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitDataDecl(this);
}


std::any CastParser::DataDeclContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitDataDecl(this);
  else
    return visitor->visitChildren(this);
}

CastParser::DataDeclContext* CastParser::dataDecl() {
  DataDeclContext *_localctx = _tracker.createInstance<DataDeclContext>(_ctx, getState());
  enterRule(_localctx, 12, CastParser::RuleDataDecl);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(121);
    match(CastParser::LBRACE);
    setState(122);
    match(CastParser::NEW_LINE);
    setState(126);
    _errHandler->sync(this);
    _la = _input->LA(1);
    while (_la == CastParser::INT_KW

    || _la == CastParser::FLOAT_KW) {
      setState(123);
      typeDecl();
      setState(128);
      _errHandler->sync(this);
      _la = _input->LA(1);
    }
    setState(129);
    match(CastParser::RBRACE);
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- StructDeclContext ------------------------------------------------------------------

CastParser::StructDeclContext::StructDeclContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::StructDeclContext::STRUCT() {
  return getToken(CastParser::STRUCT, 0);
}

tree::TerminalNode* CastParser::StructDeclContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

CastParser::DataDeclContext* CastParser::StructDeclContext::dataDecl() {
  return getRuleContext<CastParser::DataDeclContext>(0);
}


size_t CastParser::StructDeclContext::getRuleIndex() const {
  return CastParser::RuleStructDecl;
}

void CastParser::StructDeclContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterStructDecl(this);
}

void CastParser::StructDeclContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitStructDecl(this);
}


std::any CastParser::StructDeclContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitStructDecl(this);
  else
    return visitor->visitChildren(this);
}

CastParser::StructDeclContext* CastParser::structDecl() {
  StructDeclContext *_localctx = _tracker.createInstance<StructDeclContext>(_ctx, getState());
  enterRule(_localctx, 14, CastParser::RuleStructDecl);

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(131);
    match(CastParser::STRUCT);
    setState(132);
    match(CastParser::IDENTIFIER);
    setState(133);
    dataDecl();
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- StageDeclContext ------------------------------------------------------------------

CastParser::StageDeclContext::StageDeclContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

CastParser::StageIOContext* CastParser::StageDeclContext::stageIO() {
  return getRuleContext<CastParser::StageIOContext>(0);
}

tree::TerminalNode* CastParser::StageDeclContext::LPAREN() {
  return getToken(CastParser::LPAREN, 0);
}

tree::TerminalNode* CastParser::StageDeclContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

tree::TerminalNode* CastParser::StageDeclContext::RPAREN() {
  return getToken(CastParser::RPAREN, 0);
}

CastParser::DataDeclContext* CastParser::StageDeclContext::dataDecl() {
  return getRuleContext<CastParser::DataDeclContext>(0);
}


size_t CastParser::StageDeclContext::getRuleIndex() const {
  return CastParser::RuleStageDecl;
}

void CastParser::StageDeclContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterStageDecl(this);
}

void CastParser::StageDeclContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitStageDecl(this);
}


std::any CastParser::StageDeclContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitStageDecl(this);
  else
    return visitor->visitChildren(this);
}

CastParser::StageDeclContext* CastParser::stageDecl() {
  StageDeclContext *_localctx = _tracker.createInstance<StageDeclContext>(_ctx, getState());
  enterRule(_localctx, 16, CastParser::RuleStageDecl);

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(135);
    stageIO();
    setState(136);
    match(CastParser::LPAREN);
    setState(137);
    match(CastParser::IDENTIFIER);
    setState(138);
    match(CastParser::RPAREN);
    setState(139);
    dataDecl();
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- ExpressionContext ------------------------------------------------------------------

CastParser::ExpressionContext::ExpressionContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

CastParser::AdditiveExpressionContext* CastParser::ExpressionContext::additiveExpression() {
  return getRuleContext<CastParser::AdditiveExpressionContext>(0);
}


size_t CastParser::ExpressionContext::getRuleIndex() const {
  return CastParser::RuleExpression;
}

void CastParser::ExpressionContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterExpression(this);
}

void CastParser::ExpressionContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitExpression(this);
}


std::any CastParser::ExpressionContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitExpression(this);
  else
    return visitor->visitChildren(this);
}

CastParser::ExpressionContext* CastParser::expression() {
  ExpressionContext *_localctx = _tracker.createInstance<ExpressionContext>(_ctx, getState());
  enterRule(_localctx, 18, CastParser::RuleExpression);

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(141);
    additiveExpression();
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- AdditiveExpressionContext ------------------------------------------------------------------

CastParser::AdditiveExpressionContext::AdditiveExpressionContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

std::vector<CastParser::MultiplicativeExpressionContext *> CastParser::AdditiveExpressionContext::multiplicativeExpression() {
  return getRuleContexts<CastParser::MultiplicativeExpressionContext>();
}

CastParser::MultiplicativeExpressionContext* CastParser::AdditiveExpressionContext::multiplicativeExpression(size_t i) {
  return getRuleContext<CastParser::MultiplicativeExpressionContext>(i);
}

std::vector<tree::TerminalNode *> CastParser::AdditiveExpressionContext::ADDITION() {
  return getTokens(CastParser::ADDITION);
}

tree::TerminalNode* CastParser::AdditiveExpressionContext::ADDITION(size_t i) {
  return getToken(CastParser::ADDITION, i);
}

std::vector<tree::TerminalNode *> CastParser::AdditiveExpressionContext::SUBTRACTION() {
  return getTokens(CastParser::SUBTRACTION);
}

tree::TerminalNode* CastParser::AdditiveExpressionContext::SUBTRACTION(size_t i) {
  return getToken(CastParser::SUBTRACTION, i);
}


size_t CastParser::AdditiveExpressionContext::getRuleIndex() const {
  return CastParser::RuleAdditiveExpression;
}

void CastParser::AdditiveExpressionContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterAdditiveExpression(this);
}

void CastParser::AdditiveExpressionContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitAdditiveExpression(this);
}


std::any CastParser::AdditiveExpressionContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitAdditiveExpression(this);
  else
    return visitor->visitChildren(this);
}

CastParser::AdditiveExpressionContext* CastParser::additiveExpression() {
  AdditiveExpressionContext *_localctx = _tracker.createInstance<AdditiveExpressionContext>(_ctx, getState());
  enterRule(_localctx, 20, CastParser::RuleAdditiveExpression);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(143);
    multiplicativeExpression();
    setState(148);
    _errHandler->sync(this);
    _la = _input->LA(1);
    while (_la == CastParser::ADDITION

    || _la == CastParser::SUBTRACTION) {
      setState(144);
      _la = _input->LA(1);
      if (!(_la == CastParser::ADDITION

      || _la == CastParser::SUBTRACTION)) {
      _errHandler->recoverInline(this);
      }
      else {
        _errHandler->reportMatch(this);
        consume();
      }
      setState(145);
      multiplicativeExpression();
      setState(150);
      _errHandler->sync(this);
      _la = _input->LA(1);
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- MultiplicativeExpressionContext ------------------------------------------------------------------

CastParser::MultiplicativeExpressionContext::MultiplicativeExpressionContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

std::vector<CastParser::PrimaryContext *> CastParser::MultiplicativeExpressionContext::primary() {
  return getRuleContexts<CastParser::PrimaryContext>();
}

CastParser::PrimaryContext* CastParser::MultiplicativeExpressionContext::primary(size_t i) {
  return getRuleContext<CastParser::PrimaryContext>(i);
}

std::vector<tree::TerminalNode *> CastParser::MultiplicativeExpressionContext::MULTIPLY() {
  return getTokens(CastParser::MULTIPLY);
}

tree::TerminalNode* CastParser::MultiplicativeExpressionContext::MULTIPLY(size_t i) {
  return getToken(CastParser::MULTIPLY, i);
}

std::vector<tree::TerminalNode *> CastParser::MultiplicativeExpressionContext::DIVIDE() {
  return getTokens(CastParser::DIVIDE);
}

tree::TerminalNode* CastParser::MultiplicativeExpressionContext::DIVIDE(size_t i) {
  return getToken(CastParser::DIVIDE, i);
}


size_t CastParser::MultiplicativeExpressionContext::getRuleIndex() const {
  return CastParser::RuleMultiplicativeExpression;
}

void CastParser::MultiplicativeExpressionContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterMultiplicativeExpression(this);
}

void CastParser::MultiplicativeExpressionContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitMultiplicativeExpression(this);
}


std::any CastParser::MultiplicativeExpressionContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitMultiplicativeExpression(this);
  else
    return visitor->visitChildren(this);
}

CastParser::MultiplicativeExpressionContext* CastParser::multiplicativeExpression() {
  MultiplicativeExpressionContext *_localctx = _tracker.createInstance<MultiplicativeExpressionContext>(_ctx, getState());
  enterRule(_localctx, 22, CastParser::RuleMultiplicativeExpression);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(151);
    primary();
    setState(156);
    _errHandler->sync(this);
    _la = _input->LA(1);
    while (_la == CastParser::MULTIPLY

    || _la == CastParser::DIVIDE) {
      setState(152);
      _la = _input->LA(1);
      if (!(_la == CastParser::MULTIPLY

      || _la == CastParser::DIVIDE)) {
      _errHandler->recoverInline(this);
      }
      else {
        _errHandler->reportMatch(this);
        consume();
      }
      setState(153);
      primary();
      setState(158);
      _errHandler->sync(this);
      _la = _input->LA(1);
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- PrimaryContext ------------------------------------------------------------------

CastParser::PrimaryContext::PrimaryContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}


size_t CastParser::PrimaryContext::getRuleIndex() const {
  return CastParser::RulePrimary;
}

void CastParser::PrimaryContext::copyFrom(PrimaryContext *ctx) {
  ParserRuleContext::copyFrom(ctx);
}

//----------------- FloatPrimaryContext ------------------------------------------------------------------

tree::TerminalNode* CastParser::FloatPrimaryContext::FLOAT() {
  return getToken(CastParser::FLOAT, 0);
}

CastParser::FloatPrimaryContext::FloatPrimaryContext(PrimaryContext *ctx) { copyFrom(ctx); }

void CastParser::FloatPrimaryContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterFloatPrimary(this);
}
void CastParser::FloatPrimaryContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitFloatPrimary(this);
}

std::any CastParser::FloatPrimaryContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitFloatPrimary(this);
  else
    return visitor->visitChildren(this);
}
//----------------- IntPrimaryContext ------------------------------------------------------------------

tree::TerminalNode* CastParser::IntPrimaryContext::INTEGER() {
  return getToken(CastParser::INTEGER, 0);
}

CastParser::IntPrimaryContext::IntPrimaryContext(PrimaryContext *ctx) { copyFrom(ctx); }

void CastParser::IntPrimaryContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterIntPrimary(this);
}
void CastParser::IntPrimaryContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitIntPrimary(this);
}

std::any CastParser::IntPrimaryContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitIntPrimary(this);
  else
    return visitor->visitChildren(this);
}
//----------------- IdPrimaryContext ------------------------------------------------------------------

tree::TerminalNode* CastParser::IdPrimaryContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

CastParser::IdPrimaryContext::IdPrimaryContext(PrimaryContext *ctx) { copyFrom(ctx); }

void CastParser::IdPrimaryContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterIdPrimary(this);
}
void CastParser::IdPrimaryContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitIdPrimary(this);
}

std::any CastParser::IdPrimaryContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitIdPrimary(this);
  else
    return visitor->visitChildren(this);
}
//----------------- FuncCallPrimaryContext ------------------------------------------------------------------

CastParser::FunctionCallContext* CastParser::FuncCallPrimaryContext::functionCall() {
  return getRuleContext<CastParser::FunctionCallContext>(0);
}

CastParser::FuncCallPrimaryContext::FuncCallPrimaryContext(PrimaryContext *ctx) { copyFrom(ctx); }

void CastParser::FuncCallPrimaryContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterFuncCallPrimary(this);
}
void CastParser::FuncCallPrimaryContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitFuncCallPrimary(this);
}

std::any CastParser::FuncCallPrimaryContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitFuncCallPrimary(this);
  else
    return visitor->visitChildren(this);
}
//----------------- ParenPrimaryContext ------------------------------------------------------------------

tree::TerminalNode* CastParser::ParenPrimaryContext::LPAREN() {
  return getToken(CastParser::LPAREN, 0);
}

CastParser::ExpressionContext* CastParser::ParenPrimaryContext::expression() {
  return getRuleContext<CastParser::ExpressionContext>(0);
}

tree::TerminalNode* CastParser::ParenPrimaryContext::RPAREN() {
  return getToken(CastParser::RPAREN, 0);
}

CastParser::ParenPrimaryContext::ParenPrimaryContext(PrimaryContext *ctx) { copyFrom(ctx); }

void CastParser::ParenPrimaryContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterParenPrimary(this);
}
void CastParser::ParenPrimaryContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitParenPrimary(this);
}

std::any CastParser::ParenPrimaryContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitParenPrimary(this);
  else
    return visitor->visitChildren(this);
}
CastParser::PrimaryContext* CastParser::primary() {
  PrimaryContext *_localctx = _tracker.createInstance<PrimaryContext>(_ctx, getState());
  enterRule(_localctx, 24, CastParser::RulePrimary);

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    setState(167);
    _errHandler->sync(this);
    switch (getInterpreter<atn::ParserATNSimulator>()->adaptivePredict(_input, 16, _ctx)) {
    case 1: {
      _localctx = _tracker.createInstance<CastParser::FuncCallPrimaryContext>(_localctx);
      enterOuterAlt(_localctx, 1);
      setState(159);
      functionCall();
      break;
    }

    case 2: {
      _localctx = _tracker.createInstance<CastParser::IdPrimaryContext>(_localctx);
      enterOuterAlt(_localctx, 2);
      setState(160);
      match(CastParser::IDENTIFIER);
      break;
    }

    case 3: {
      _localctx = _tracker.createInstance<CastParser::IntPrimaryContext>(_localctx);
      enterOuterAlt(_localctx, 3);
      setState(161);
      match(CastParser::INTEGER);
      break;
    }

    case 4: {
      _localctx = _tracker.createInstance<CastParser::FloatPrimaryContext>(_localctx);
      enterOuterAlt(_localctx, 4);
      setState(162);
      match(CastParser::FLOAT);
      break;
    }

    case 5: {
      _localctx = _tracker.createInstance<CastParser::ParenPrimaryContext>(_localctx);
      enterOuterAlt(_localctx, 5);
      setState(163);
      match(CastParser::LPAREN);
      setState(164);
      expression();
      setState(165);
      match(CastParser::RPAREN);
      break;
    }

    default:
      break;
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- BodyContext ------------------------------------------------------------------

CastParser::BodyContext::BodyContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::BodyContext::LBRACE() {
  return getToken(CastParser::LBRACE, 0);
}

tree::TerminalNode* CastParser::BodyContext::RBRACE() {
  return getToken(CastParser::RBRACE, 0);
}

std::vector<tree::TerminalNode *> CastParser::BodyContext::NEW_LINE() {
  return getTokens(CastParser::NEW_LINE);
}

tree::TerminalNode* CastParser::BodyContext::NEW_LINE(size_t i) {
  return getToken(CastParser::NEW_LINE, i);
}

std::vector<CastParser::StatementContext *> CastParser::BodyContext::statement() {
  return getRuleContexts<CastParser::StatementContext>();
}

CastParser::StatementContext* CastParser::BodyContext::statement(size_t i) {
  return getRuleContext<CastParser::StatementContext>(i);
}


size_t CastParser::BodyContext::getRuleIndex() const {
  return CastParser::RuleBody;
}

void CastParser::BodyContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterBody(this);
}

void CastParser::BodyContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitBody(this);
}


std::any CastParser::BodyContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitBody(this);
  else
    return visitor->visitChildren(this);
}

CastParser::BodyContext* CastParser::body() {
  BodyContext *_localctx = _tracker.createInstance<BodyContext>(_ctx, getState());
  enterRule(_localctx, 26, CastParser::RuleBody);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(169);
    match(CastParser::LBRACE);
    setState(171);
    _errHandler->sync(this);

    switch (getInterpreter<atn::ParserATNSimulator>()->adaptivePredict(_input, 17, _ctx)) {
    case 1: {
      setState(170);
      match(CastParser::NEW_LINE);
      break;
    }

    default:
      break;
    }
    setState(178);
    _errHandler->sync(this);
    _la = _input->LA(1);
    while ((((_la & ~ 0x3fULL) == 0) &&
      ((1ULL << _la) & 5368709568) != 0)) {
      setState(173);
      statement();
      setState(174);
      match(CastParser::NEW_LINE);
      setState(180);
      _errHandler->sync(this);
      _la = _input->LA(1);
    }
    setState(181);
    match(CastParser::RBRACE);
    setState(183);
    _errHandler->sync(this);

    switch (getInterpreter<atn::ParserATNSimulator>()->adaptivePredict(_input, 19, _ctx)) {
    case 1: {
      setState(182);
      match(CastParser::NEW_LINE);
      break;
    }

    default:
      break;
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- TypeContext ------------------------------------------------------------------

CastParser::TypeContext::TypeContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

tree::TerminalNode* CastParser::TypeContext::INT_KW() {
  return getToken(CastParser::INT_KW, 0);
}

tree::TerminalNode* CastParser::TypeContext::FLOAT_KW() {
  return getToken(CastParser::FLOAT_KW, 0);
}


size_t CastParser::TypeContext::getRuleIndex() const {
  return CastParser::RuleType;
}

void CastParser::TypeContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterType(this);
}

void CastParser::TypeContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitType(this);
}


std::any CastParser::TypeContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitType(this);
  else
    return visitor->visitChildren(this);
}

CastParser::TypeContext* CastParser::type() {
  TypeContext *_localctx = _tracker.createInstance<TypeContext>(_ctx, getState());
  enterRule(_localctx, 28, CastParser::RuleType);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(185);
    _la = _input->LA(1);
    if (!(_la == CastParser::INT_KW

    || _la == CastParser::FLOAT_KW)) {
    _errHandler->recoverInline(this);
    }
    else {
      _errHandler->reportMatch(this);
      consume();
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- AssignmentContext ------------------------------------------------------------------

CastParser::AssignmentContext::AssignmentContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

CastParser::TypeContext* CastParser::AssignmentContext::type() {
  return getRuleContext<CastParser::TypeContext>(0);
}

tree::TerminalNode* CastParser::AssignmentContext::IDENTIFIER() {
  return getToken(CastParser::IDENTIFIER, 0);
}

tree::TerminalNode* CastParser::AssignmentContext::EQ() {
  return getToken(CastParser::EQ, 0);
}

CastParser::ExpressionContext* CastParser::AssignmentContext::expression() {
  return getRuleContext<CastParser::ExpressionContext>(0);
}


size_t CastParser::AssignmentContext::getRuleIndex() const {
  return CastParser::RuleAssignment;
}

void CastParser::AssignmentContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterAssignment(this);
}

void CastParser::AssignmentContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitAssignment(this);
}


std::any CastParser::AssignmentContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitAssignment(this);
  else
    return visitor->visitChildren(this);
}

CastParser::AssignmentContext* CastParser::assignment() {
  AssignmentContext *_localctx = _tracker.createInstance<AssignmentContext>(_ctx, getState());
  enterRule(_localctx, 30, CastParser::RuleAssignment);

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    enterOuterAlt(_localctx, 1);
    setState(187);
    type();
    setState(188);
    match(CastParser::IDENTIFIER);
    setState(189);
    match(CastParser::EQ);
    setState(190);
    expression();
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

//----------------- StatementContext ------------------------------------------------------------------

CastParser::StatementContext::StatementContext(ParserRuleContext *parent, size_t invokingState)
  : ParserRuleContext(parent, invokingState) {
}

CastParser::AssignmentContext* CastParser::StatementContext::assignment() {
  return getRuleContext<CastParser::AssignmentContext>(0);
}

CastParser::FunctionCallContext* CastParser::StatementContext::functionCall() {
  return getRuleContext<CastParser::FunctionCallContext>(0);
}

tree::TerminalNode* CastParser::StatementContext::RETURN() {
  return getToken(CastParser::RETURN, 0);
}

CastParser::PrimaryContext* CastParser::StatementContext::primary() {
  return getRuleContext<CastParser::PrimaryContext>(0);
}


size_t CastParser::StatementContext::getRuleIndex() const {
  return CastParser::RuleStatement;
}

void CastParser::StatementContext::enterRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->enterStatement(this);
}

void CastParser::StatementContext::exitRule(tree::ParseTreeListener *listener) {
  auto parserListener = dynamic_cast<CastParserListener *>(listener);
  if (parserListener != nullptr)
    parserListener->exitStatement(this);
}


std::any CastParser::StatementContext::accept(tree::ParseTreeVisitor *visitor) {
  if (auto parserVisitor = dynamic_cast<CastParserVisitor*>(visitor))
    return parserVisitor->visitStatement(this);
  else
    return visitor->visitChildren(this);
}

CastParser::StatementContext* CastParser::statement() {
  StatementContext *_localctx = _tracker.createInstance<StatementContext>(_ctx, getState());
  enterRule(_localctx, 32, CastParser::RuleStatement);
  size_t _la = 0;

#if __cplusplus > 201703L
  auto onExit = finally([=, this] {
#else
  auto onExit = finally([=] {
#endif
    exitRule();
  });
  try {
    setState(200);
    _errHandler->sync(this);
    switch (_input->LA(1)) {
      case CastParser::INT_KW:
      case CastParser::FLOAT_KW: {
        enterOuterAlt(_localctx, 1);
        setState(192);
        assignment();
        break;
      }

      case CastParser::IDENTIFIER: {
        enterOuterAlt(_localctx, 2);
        setState(193);
        functionCall();
        break;
      }

      case CastParser::RETURN:
      case CastParser::NEW_LINE: {
        enterOuterAlt(_localctx, 3);
        setState(198);
        _errHandler->sync(this);

        _la = _input->LA(1);
        if (_la == CastParser::RETURN) {
          setState(194);
          match(CastParser::RETURN);
          setState(196);
          _errHandler->sync(this);

          _la = _input->LA(1);
          if ((((_la & ~ 0x3fULL) == 0) &&
            ((1ULL << _la) & 5100274688) != 0)) {
            setState(195);
            primary();
          }
        }
        break;
      }

    default:
      throw NoViableAltException(this);
    }
   
  }
  catch (RecognitionException &e) {
    _errHandler->reportError(this, e);
    _localctx->exception = std::current_exception();
    _errHandler->recover(this, _localctx->exception);
  }

  return _localctx;
}

void CastParser::initialize() {
#if ANTLR4_USE_THREAD_LOCAL_CACHE
  castparserParserInitialize();
#else
  ::antlr4::internal::call_once(castparserParserOnceFlag, castparserParserInitialize);
#endif
}


// Generated from CastParser.g4 by ANTLR 4.13.2

#pragma once


#include "antlr4-runtime.h"
#include "CastParserListener.h"


/**
 * This class provides an empty implementation of CastParserListener,
 * which can be extended to create a listener which only needs to handle a subset
 * of the available methods.
 */
class  CastParserBaseListener : public CastParserListener {
public:

  virtual void enterProgram(CastParser::ProgramContext * /*ctx*/) override { }
  virtual void exitProgram(CastParser::ProgramContext * /*ctx*/) override { }

  virtual void enterStageIO(CastParser::StageIOContext * /*ctx*/) override { }
  virtual void exitStageIO(CastParser::StageIOContext * /*ctx*/) override { }

  virtual void enterParams(CastParser::ParamsContext * /*ctx*/) override { }
  virtual void exitParams(CastParser::ParamsContext * /*ctx*/) override { }

  virtual void enterFunctionCall(CastParser::FunctionCallContext * /*ctx*/) override { }
  virtual void exitFunctionCall(CastParser::FunctionCallContext * /*ctx*/) override { }

  virtual void enterFunctionDecl(CastParser::FunctionDeclContext * /*ctx*/) override { }
  virtual void exitFunctionDecl(CastParser::FunctionDeclContext * /*ctx*/) override { }

  virtual void enterTypeDecl(CastParser::TypeDeclContext * /*ctx*/) override { }
  virtual void exitTypeDecl(CastParser::TypeDeclContext * /*ctx*/) override { }

  virtual void enterDataDecl(CastParser::DataDeclContext * /*ctx*/) override { }
  virtual void exitDataDecl(CastParser::DataDeclContext * /*ctx*/) override { }

  virtual void enterStructDecl(CastParser::StructDeclContext * /*ctx*/) override { }
  virtual void exitStructDecl(CastParser::StructDeclContext * /*ctx*/) override { }

  virtual void enterStageDecl(CastParser::StageDeclContext * /*ctx*/) override { }
  virtual void exitStageDecl(CastParser::StageDeclContext * /*ctx*/) override { }

  virtual void enterExpression(CastParser::ExpressionContext * /*ctx*/) override { }
  virtual void exitExpression(CastParser::ExpressionContext * /*ctx*/) override { }

  virtual void enterAdditiveExpression(CastParser::AdditiveExpressionContext * /*ctx*/) override { }
  virtual void exitAdditiveExpression(CastParser::AdditiveExpressionContext * /*ctx*/) override { }

  virtual void enterMultiplicativeExpression(CastParser::MultiplicativeExpressionContext * /*ctx*/) override { }
  virtual void exitMultiplicativeExpression(CastParser::MultiplicativeExpressionContext * /*ctx*/) override { }

  virtual void enterFuncCallPrimary(CastParser::FuncCallPrimaryContext * /*ctx*/) override { }
  virtual void exitFuncCallPrimary(CastParser::FuncCallPrimaryContext * /*ctx*/) override { }

  virtual void enterIdPrimary(CastParser::IdPrimaryContext * /*ctx*/) override { }
  virtual void exitIdPrimary(CastParser::IdPrimaryContext * /*ctx*/) override { }

  virtual void enterIntPrimary(CastParser::IntPrimaryContext * /*ctx*/) override { }
  virtual void exitIntPrimary(CastParser::IntPrimaryContext * /*ctx*/) override { }

  virtual void enterFloatPrimary(CastParser::FloatPrimaryContext * /*ctx*/) override { }
  virtual void exitFloatPrimary(CastParser::FloatPrimaryContext * /*ctx*/) override { }

  virtual void enterParenPrimary(CastParser::ParenPrimaryContext * /*ctx*/) override { }
  virtual void exitParenPrimary(CastParser::ParenPrimaryContext * /*ctx*/) override { }

  virtual void enterBody(CastParser::BodyContext * /*ctx*/) override { }
  virtual void exitBody(CastParser::BodyContext * /*ctx*/) override { }

  virtual void enterType(CastParser::TypeContext * /*ctx*/) override { }
  virtual void exitType(CastParser::TypeContext * /*ctx*/) override { }

  virtual void enterAssignment(CastParser::AssignmentContext * /*ctx*/) override { }
  virtual void exitAssignment(CastParser::AssignmentContext * /*ctx*/) override { }

  virtual void enterStatement(CastParser::StatementContext * /*ctx*/) override { }
  virtual void exitStatement(CastParser::StatementContext * /*ctx*/) override { }


  virtual void enterEveryRule(antlr4::ParserRuleContext * /*ctx*/) override { }
  virtual void exitEveryRule(antlr4::ParserRuleContext * /*ctx*/) override { }
  virtual void visitTerminal(antlr4::tree::TerminalNode * /*node*/) override { }
  virtual void visitErrorNode(antlr4::tree::ErrorNode * /*node*/) override { }

};


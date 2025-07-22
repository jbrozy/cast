
// Generated from CastParser.g4 by ANTLR 4.13.2

#pragma once


#include "antlr4-runtime.h"
#include "CastParser.h"


/**
 * This interface defines an abstract listener for a parse tree produced by CastParser.
 */
class  CastParserListener : public antlr4::tree::ParseTreeListener {
public:

  virtual void enterProgram(CastParser::ProgramContext *ctx) = 0;
  virtual void exitProgram(CastParser::ProgramContext *ctx) = 0;

  virtual void enterStageIO(CastParser::StageIOContext *ctx) = 0;
  virtual void exitStageIO(CastParser::StageIOContext *ctx) = 0;

  virtual void enterParams(CastParser::ParamsContext *ctx) = 0;
  virtual void exitParams(CastParser::ParamsContext *ctx) = 0;

  virtual void enterFunctionCall(CastParser::FunctionCallContext *ctx) = 0;
  virtual void exitFunctionCall(CastParser::FunctionCallContext *ctx) = 0;

  virtual void enterFunctionDecl(CastParser::FunctionDeclContext *ctx) = 0;
  virtual void exitFunctionDecl(CastParser::FunctionDeclContext *ctx) = 0;

  virtual void enterTypeDecl(CastParser::TypeDeclContext *ctx) = 0;
  virtual void exitTypeDecl(CastParser::TypeDeclContext *ctx) = 0;

  virtual void enterDataDecl(CastParser::DataDeclContext *ctx) = 0;
  virtual void exitDataDecl(CastParser::DataDeclContext *ctx) = 0;

  virtual void enterStructDecl(CastParser::StructDeclContext *ctx) = 0;
  virtual void exitStructDecl(CastParser::StructDeclContext *ctx) = 0;

  virtual void enterStageDecl(CastParser::StageDeclContext *ctx) = 0;
  virtual void exitStageDecl(CastParser::StageDeclContext *ctx) = 0;

  virtual void enterExpression(CastParser::ExpressionContext *ctx) = 0;
  virtual void exitExpression(CastParser::ExpressionContext *ctx) = 0;

  virtual void enterAdditiveExpression(CastParser::AdditiveExpressionContext *ctx) = 0;
  virtual void exitAdditiveExpression(CastParser::AdditiveExpressionContext *ctx) = 0;

  virtual void enterMultiplicativeExpression(CastParser::MultiplicativeExpressionContext *ctx) = 0;
  virtual void exitMultiplicativeExpression(CastParser::MultiplicativeExpressionContext *ctx) = 0;

  virtual void enterFuncCallPrimary(CastParser::FuncCallPrimaryContext *ctx) = 0;
  virtual void exitFuncCallPrimary(CastParser::FuncCallPrimaryContext *ctx) = 0;

  virtual void enterIdPrimary(CastParser::IdPrimaryContext *ctx) = 0;
  virtual void exitIdPrimary(CastParser::IdPrimaryContext *ctx) = 0;

  virtual void enterIntPrimary(CastParser::IntPrimaryContext *ctx) = 0;
  virtual void exitIntPrimary(CastParser::IntPrimaryContext *ctx) = 0;

  virtual void enterFloatPrimary(CastParser::FloatPrimaryContext *ctx) = 0;
  virtual void exitFloatPrimary(CastParser::FloatPrimaryContext *ctx) = 0;

  virtual void enterParenPrimary(CastParser::ParenPrimaryContext *ctx) = 0;
  virtual void exitParenPrimary(CastParser::ParenPrimaryContext *ctx) = 0;

  virtual void enterBody(CastParser::BodyContext *ctx) = 0;
  virtual void exitBody(CastParser::BodyContext *ctx) = 0;

  virtual void enterType(CastParser::TypeContext *ctx) = 0;
  virtual void exitType(CastParser::TypeContext *ctx) = 0;

  virtual void enterAssignment(CastParser::AssignmentContext *ctx) = 0;
  virtual void exitAssignment(CastParser::AssignmentContext *ctx) = 0;

  virtual void enterStatement(CastParser::StatementContext *ctx) = 0;
  virtual void exitStatement(CastParser::StatementContext *ctx) = 0;


};


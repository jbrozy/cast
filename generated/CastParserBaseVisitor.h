
// Generated from CastParser.g4 by ANTLR 4.13.2

#pragma once


#include "antlr4-runtime.h"
#include "CastParserVisitor.h"


/**
 * This class provides an empty implementation of CastParserVisitor, which can be
 * extended to create a visitor which only needs to handle a subset of the available methods.
 */
class  CastParserBaseVisitor : public CastParserVisitor {
public:

  virtual std::any visitProgram(CastParser::ProgramContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitStageIO(CastParser::StageIOContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitParams(CastParser::ParamsContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitFunctionCall(CastParser::FunctionCallContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitFunctionDecl(CastParser::FunctionDeclContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitTypeDecl(CastParser::TypeDeclContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitDataDecl(CastParser::DataDeclContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitStructDecl(CastParser::StructDeclContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitStageDecl(CastParser::StageDeclContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitExpression(CastParser::ExpressionContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitAdditiveExpression(CastParser::AdditiveExpressionContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitMultiplicativeExpression(CastParser::MultiplicativeExpressionContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitFuncCallPrimary(CastParser::FuncCallPrimaryContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitIdPrimary(CastParser::IdPrimaryContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitIntPrimary(CastParser::IntPrimaryContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitFloatPrimary(CastParser::FloatPrimaryContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitParenPrimary(CastParser::ParenPrimaryContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitBody(CastParser::BodyContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitType(CastParser::TypeContext *ctx) override {
    return visitChildren(ctx);
  }

  virtual std::any visitAssignment(CastParser::AssignmentContext *ctx) override {
    std::string type = ctx->type()->getText();
    std::string identifier = ctx->IDENTIFIER()->getText();

    std::cout << "Assigning " << identifier << " of type " << type << "\n";
    return visitExpression(ctx->expression());
  }

  virtual std::any visitStatement(CastParser::StatementContext *ctx) override {
    return visitChildren(ctx);
  }
};


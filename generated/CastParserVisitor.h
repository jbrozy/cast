
// Generated from CastParser.g4 by ANTLR 4.13.2

#pragma once


#include "antlr4-runtime.h"
#include "CastParser.h"



/**
 * This class defines an abstract visitor for a parse tree
 * produced by CastParser.
 */
class  CastParserVisitor : public antlr4::tree::AbstractParseTreeVisitor {
public:

  /**
   * Visit parse trees produced by CastParser.
   */
    virtual std::any visitProgram(CastParser::ProgramContext *context) = 0;

    virtual std::any visitStageIO(CastParser::StageIOContext *context) = 0;

    virtual std::any visitParams(CastParser::ParamsContext *context) = 0;

    virtual std::any visitFunctionCall(CastParser::FunctionCallContext *context) = 0;

    virtual std::any visitFunctionDecl(CastParser::FunctionDeclContext *context) = 0;

    virtual std::any visitTypeDecl(CastParser::TypeDeclContext *context) = 0;

    virtual std::any visitDataDecl(CastParser::DataDeclContext *context) = 0;

    virtual std::any visitStructDecl(CastParser::StructDeclContext *context) = 0;

    virtual std::any visitStageDecl(CastParser::StageDeclContext *context) = 0;

    virtual std::any visitExpression(CastParser::ExpressionContext *context) = 0;

    virtual std::any visitAdditiveExpression(CastParser::AdditiveExpressionContext *context) = 0;

    virtual std::any visitMultiplicativeExpression(CastParser::MultiplicativeExpressionContext *context) = 0;

    virtual std::any visitFuncCallPrimary(CastParser::FuncCallPrimaryContext *context) = 0;

    virtual std::any visitIdPrimary(CastParser::IdPrimaryContext *context) = 0;

    virtual std::any visitIntPrimary(CastParser::IntPrimaryContext *context) = 0;

    virtual std::any visitFloatPrimary(CastParser::FloatPrimaryContext *context) = 0;

    virtual std::any visitParenPrimary(CastParser::ParenPrimaryContext *context) = 0;

    virtual std::any visitBody(CastParser::BodyContext *context) = 0;

    virtual std::any visitType(CastParser::TypeContext *context) = 0;

    virtual std::any visitAssignment(CastParser::AssignmentContext *context) = 0;

    virtual std::any visitStatement(CastParser::StatementContext *context) = 0;


};


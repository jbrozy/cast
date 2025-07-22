parser grammar CastParser;
options { tokenVocab=CastLexer; }

program
    :  ('@internal')? (functionDecl NEW_LINE | statement NEW_LINE | stageDecl NEW_LINE | structDecl NEW_LINE)*
    |   NEW_LINE+
    |   EOF
    ;

stageIO
    : ('@input' | '@output')
    ;

params
    : type IDENTIFIER ( COMMA? type IDENTIFIER )*
    ;

functionCall
    : IDENTIFIER
      LPAREN
         ( expression ( COMMA expression )* )?   // zero or more args
      RPAREN
    ;

functionDecl
    :   'fn'
        IDENTIFIER
        LPAREN params? RPAREN
        (ARROW type)?
        body
    ;

typeDecl
    : type IDENTIFIER (ARROW LBRACKET IDENTIFIER (COMMA IDENTIFIER)* RBRACKET)? NEW_LINE
    ;

dataDecl
    : LBRACE NEW_LINE (typeDecl)* RBRACE
    ;

structDecl
    : STRUCT IDENTIFIER dataDecl
    ;

stageDecl
    : stageIO LPAREN IDENTIFIER RPAREN dataDecl
    ;

expression
    : additiveExpression
    ;

additiveExpression
    : multiplicativeExpression
      ( (ADDITION | SUBTRACTION) multiplicativeExpression )*
    ;

multiplicativeExpression
    : primary
      ( (MULTIPLY | DIVIDE) primary )*
    ;

primary
    : functionCall               # FuncCallPrimary
    | IDENTIFIER                 # IdPrimary
    | INTEGER                    # IntPrimary
    | FLOAT                      # FloatPrimary
    | LPAREN expression RPAREN   # ParenPrimary
    ;

body
    :   LBRACE
        NEW_LINE?
        (statement NEW_LINE)*
        RBRACE
        NEW_LINE?
    ;

type
    :   INT_KW | FLOAT_KW
    ;

assignment
    :   type
        IDENTIFIER
        EQ
        expression
    ;

statement
    :   assignment
    |   functionCall
    |   (RETURN (primary)?)?
    ;

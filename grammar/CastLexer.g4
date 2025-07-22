lexer grammar CastLexer;

// 1. Keywords
INPUT_KW     : '@input';
OUTPUT_KW    : '@output';
INTERNAL  : '@internal';

STRUCT    : 'struct';
FN        : 'fn' ;
INT_KW    : 'int' ;
FLOAT_KW  : 'float' ;
RETURN    : 'return';

// 2. Symbols
ARROW     : '->' ;
LPAREN    : '(' ;
RPAREN    : ')' ;
LBRACE    : '{' ;
RBRACE    : '}' ;
COMMA     : ',' ;
EQ        : '=' ;

OR        : '|';
AND       : '&';
XOR       : '^';
NOR       : '^|';

// 3. Operators
MULTIPLY    : '*';
DIVIDE      : '/';
ADDITION    : '+';
SUBTRACTION : '-';

LBRACKET    : '[';
RBRACKET    : ']';

OPERATOR  : (MULTIPLY | DIVIDE | ADDITION | SUBTRACTION);
BIN_OPERATOR  : (OR | AND | XOR | NOR);

INTEGER
    : [1-9] [0-9]+
    ;

FLOAT
    : [0-9]+ ('.' [0-9]+)? ([eE] [+\-]? [0-9]+)?
    ;

NEW_LINE  : '\r'? '\n' ;
WS        : [ \t]+ -> skip ;

IDENTIFIER
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;

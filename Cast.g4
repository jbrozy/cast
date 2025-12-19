grammar Cast;

program: 
	| importStmt* statement+;

statement
	: structDecl                    # StructDeclStmt
    | functionDecl                  # FnDeclStmt
    | block                         # BlockStmt
	| uniformStmt                 	# UniformStmtWrapper
    | assignment ';'                # AssignStmt
    | spaceDecl ';'                 # SpaceDeclStmt
    | RETURN simpleExpression ';'   # ReturnStmt    
    | () simpleExpression ';'       # ExprStmt
    ;

primitiveDecl
	:	
	;

inOut
	: IN
	| OUT
	;

importStmt
    : INCLUDE path=STRING ';'
    ;
	
assignment
    : LET typeDecl EQUAL value=simpleExpression      # VarDeclAssign
    | varRef=ID EQUAL value=simpleExpression         # VarAssign
    ;
    
uniformStmt
    : UNIFORM '{' (members+=uniformTypeDecl (',' members+=uniformTypeDecl)*)? '}' 	# UniformBlockDecl
    | UNIFORM typeDecl ';'                                           			  	# UniformVarDecl
    ;
	
uniformTypeDecl
	: name=ID ':' type=ID
	;
	
block
    : OPEN_CURLY  statement* CLOSE_CURLY
    ;

structDecl
    : (DECLARE)? STRUCT name=ID OPEN_CURLY (members+=typeDecl (',' members+=typeDecl)*)? CLOSE_CURLY
    ;
    
functionDecl
    : (DECLARE)? FN typedFunctionDecl? functionIdentifier? OPEN_PAR params=paramList? CLOSE_PAR (':' returnType=ID)? block
    ;
	
functionIdentifier
	: functionName=ID
	;
	
typedFunctionDecl
    : OPEN_PAR typeFn=ID CLOSE_PAR
    ;

functionCall
    : name=ID typeSpace? OPEN_PAR args=argList CLOSE_PAR
    ;
    
argList
    : (simpleExpression (',' simpleExpression)*)?
    ;
    
paramList
    : (typeDecl (',' typeDecl)*)?
    ;

typeSpace
    : LT spaceName=ID GT
    ;
	
typeDecl
    : variable=ID ':' type=ID typeSpace?
    ;
    
spaceDecl
    : DECLARE SPACE spaceName=ID;

simpleExpression
    : expr=simpleExpression '.' name=ID OPEN_PAR args=argList CLOSE_PAR   # MethodCallExpr
    | expr=simpleExpression '.' name=ID                                   # MemberAccessExpr
    | left=simpleExpression op=(MULTIPLY | DIVIDE) right=simpleExpression # MultDiv
    | left=simpleExpression op=(PLUS | MINUS) right=simpleExpression      # AddSub
    | atom                                                                # AtomExpr
    ;

atom
    : INT                               # IntAtom
    | FLOAT                             # FloatAtom
    | functionCall                      # CallAtom
    | varRef=ID                         # VarAtom
    | '(' simpleExpression ')'          # ParenAtom
    ;

TYPE : 'type';
INCLUDE : 'include';
STRING : '"' (~'"')* '"';
DECLARE : 'declare';
SPACE   : 'space';
STRUCT  : 'struct';
UNIFORM : 'uniform';
RETURN  : 'return';
LET     : 'let';
VAR     : 'var';
FN      : 'fn';

IN		: 'in';
OUT		: 'out';

PLUS    : '+';
MINUS   : '-';
MULTIPLY: '*';
DIVIDE  : '/';
EQUAL   : '=';
COLON   : ':';
OPEN_PAR: '(';
CLOSE_PAR: ')';
OPEN_CURLY: '{';
CLOSE_CURLY: '}';
LT      : '<';
GT      : '>';
DOT     : '.';
COMMA   : ',';

INT     : [0-9]+;
FLOAT   : [0-9]+ '.' [0-9]+;

ID      : [a-zA-Z_] [a-zA-Z0-9_]*;

WS      : [ \t\r\n]+ -> skip;
grammar Cast;

program
	: AT 'stage' OPEN_PAR stageName=stages CLOSE_PAR
	| statement+
	| EOF
	;

statement
	: IF OPEN_PAR simpleExpression CLOSE_PAR block  # IfStmt
	| structDecl                    				# StructDeclStmt
    | functionDecl                  				# FnDeclStmt
	| constructorFunctionDecl						# ConstructorFnDeclStmt
    | typedFunctionDecl                  			# TypedFnDeclStmt
    | block                         				# BlockStmt
	| uniformStmt                 					# UniformStmtWrapper
	| inStmt                 						# InStmtWrapper
	| outStmt                 						# OutStmtWrapper
    | assignment ';'                				# AssignStmt
    | spaceDecl ';'                 				# SpaceDeclStmt
    | RETURN simpleExpression ';'   				# ReturnStmt    
    | () simpleExpression ';'     				    # ExprStmt
    ;

inOut
	: IN
	| OUT
	;

importStmt
    : INCLUDE path=STRING ';'
    ;
	
stages
	: 'vertex'
	| 'fragment'
	| 'compute'
	;
	
assignment
    : DECLARE? LET variable=ID (':' type=ID typeSpace?)? EQUAL value=simpleExpression  # VarDeclAssign
    | DECLARE? LET variable=ID (':' type=ID typeSpace?)?    						   # VarDecl
    | varRef=ID EQUAL value=simpleExpression                                           # VarAssign
    ;
    
inStmt 
    : IN '{' (members+=inTypeDecl (',' members+=inTypeDecl)*)? '}' 		# InBlockDecl
    | IN inTypeDecl ';'                                           	 	# InVarDecl
	;

outStmt 
    : OUT '{' (members+=outTypeDecl (',' members+=outTypeDecl)*)? '}' 	# OutBlockDecl
    | OUT outTypeDecl ';'                                           	# OutVarDecl
	;

uniformStmt
    : UNIFORM '{' (members+=uniformTypeDecl (',' members+=uniformTypeDecl)*)? '}' 	# UniformBlockDecl
    | UNIFORM uniformTypeDecl ';'                                           		# UniformVarDecl
    ;

outTypeDecl
	: name=ID ':' type=ID typeSpace?
	;

inTypeDecl
	: name=ID ':' type=ID typeSpace?
	;

uniformTypeDecl
	: name=ID ':' type=ID typeSpace?
	;
	
block
    : OPEN_CURLY  statement* CLOSE_CURLY
    ;

structDecl
    : (DECLARE)? STRUCT name=ID OPEN_CURLY (members+=typeDecl (',' members+=typeDecl)*)? CLOSE_CURLY
    ;
	
functionDecl
    : (DECLARE)? FN functionIdentifier? OPEN_PAR params=paramList? CLOSE_PAR (':' returnType=ID)? (block | ';')
    ;

typedFunctionDecl
    : (DECLARE)? FN OPEN_PAR typeFn=ID CLOSE_PAR functionIdentifier? OPEN_PAR params=paramList? CLOSE_PAR (':' returnType=ID)? (block | ';')
    ;

constructorFunctionDecl
    : (DECLARE)? FN OPEN_PAR typeFn=ID CLOSE_PAR OPEN_PAR params=paramList? CLOSE_PAR (':' returnType=ID)? (block | ';')
	;
	
functionIdentifier
	: functionName=ID
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
	| left=simpleExpression op=(LT | GT | LTE | GTE) right=simpleExpression      	  # BooleanExpression
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
IF		: 'if';

IN		: 'in';
OUT		: 'out';

AT 		: '@';
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
LTE      : '<=';
GT      : '>';
GTE      : '>=';
DOT     : '.';
COMMA   : ',';

INT     : [0-9]+;
FLOAT   : [0-9]+ '.' [0-9]+;

ID      : [a-zA-Z_] [a-zA-Z0-9_]*;

WS      : [ \t\r\n]+ -> skip;

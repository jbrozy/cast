CC = clang++
FLAGS = -W -std=c++17
CXXFLAGS = -std=c++17

GRAMMAR_DIR = ./grammar/
GRAMMAR_FILES = ./grammar/CastBaseListener.cpp ./grammar/CastLexer.cpp ./grammar/CastListener.cpp ./grammar/CastParser.cpp

ANTLR_INC = /opt/homebrew/opt/antlr4-cpp-runtime/include/antlr4-runtime
# ANTLR_LIB = /opt/homebrew/opt/antlr4-cpp-runtime/lib/

main:
	$(CC) $(CXXFLAGS) src/main.cpp -o cast -I$(ANTLR_INC)

#/bin/bash

cd grammar
antlr -Dlanguage=Cpp -lib ../generated \
      -visitor \
      -o ../generated \
      CastParser.g4 CastLexer.g4

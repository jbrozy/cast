#!/bin/bash

if ! command -v antlr >/dev/null 2>&1
then
    echo "executable for antlr could not be found."
    exit 1
fi

antlr -o ../cast.core/antlr -Dlanguage=CSharp -visitor -package cast.core CastParser.g4 CastLexer.g4

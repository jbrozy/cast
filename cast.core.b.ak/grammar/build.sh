#!/bin/bash

if ! command -v antlr >/dev/null 2>&1
then
    echo "executable for antlr could not be found."
    exit 1
fi

antlr -Dlanguage=CSharp -visitor -package Cast.Compiler CastLexer.g4 CastParser.g4 CastPreParser.g4

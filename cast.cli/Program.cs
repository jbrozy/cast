using System.Text.RegularExpressions;
using Antlr4.Runtime;
using cast.cli.commands;
using cast.core;
using cast.core.logging;
using cast.core.models;
using cast.core.models.symbols;
using cast.core.registry;
using cast.core.visitor;
using Spectre.Console.Cli;

string input = """
               #version 330 core

               in vec3 a;
               out vec3 b;

               void main() {
                    a = 6;
               }

               """;
var app = new CommandApp();
Registry.Setup();
app.Configure(config =>
{
    config.SetApplicationName("cast");
    config.AddCommand<BuildCommand>("build")
        .WithDescription("Transpile Cast-Files to GLSL");
    config.AddCommand<ComposeCommand>("compose")
        .WithDescription("Compose a Cast Shader Project");
});

return app.Run(args);
using cast.cli.commands;
using cast.core.registry;
using Spectre.Console.Cli;

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
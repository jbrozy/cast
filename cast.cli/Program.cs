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
    config.AddCommand<WatchCommand>("watch")
        .WithDescription("Watch a directory and auto-transpile Cast files to GLSL on change");
});

return app.Run(args);
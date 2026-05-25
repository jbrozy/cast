using System.ComponentModel;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class WatchSettings : CommandSettings
{
    [CommandArgument(0, "<INPUT_DIR>")]
    [Description("Input directory to watch for Cast files.")]
    public string InputDirectory { get; set; } = string.Empty;

    [CommandArgument(1, "[OUTPUT_DIR]")]
    [Description("Directory where the GLSL output will be stored. Defaults to 'output' subfolder of INPUT_DIR.")]
    public string? OutputDirectory { get; set; }
}

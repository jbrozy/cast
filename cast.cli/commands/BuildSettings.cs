using System.ComponentModel;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class BuildSettings : CommandSettings
{
    [CommandArgument(0, "<INPUT_FILE>")]
    [Description("Input file to be transpiled from Cast to GLSL.")]
    public string InputFile { get; set; } = string.Empty;
    
    [CommandArgument(1, "<INPUT_FILE>")]
    [Description("Path where the GLSL-Output should be stored.")]
    public string? OutputFile { get; set; } = string.Empty;
}
using System.ComponentModel;
using Spectre.Console.Cli;

namespace cast.cli.commands;

public class ComposeSettings : CommandSettings
{
    [CommandArgument(0, "<INPUT_FILE>")]
    [Description("Composer for the Cast.")]
    public string ComposeFile { get; set; } = string.Empty;

    [CommandOption("-g|--graph")]
    [Description("Verify the Input-Output Graph")]
    [DefaultValue("false")]
    public bool VerifyGraph { get; set; } = false;
}
using System.Text;
using System.Text.RegularExpressions;

namespace Cast;

public class CastImport
{
    private readonly HashSet<string> _loadedFiles = new();
    private readonly string _stdLibPath;

    public CastImport(string stdLibPath)
    {
        _stdLibPath = Path.GetFullPath(stdLibPath);
    }

    public string Load(string entryFilePath)
    {
        _loadedFiles.Clear();
        var fullPath = entryFilePath;
        if (!File.Exists(fullPath))
            throw new FileNotFoundException(fullPath);
        return LoadRecursive(fullPath);
    }

    private string LoadRecursive(string currentFilePath)
    {
        if (_loadedFiles.Contains(currentFilePath)) return "";

        var src = File.ReadAllText(currentFilePath);
        var currentDir = Path.GetDirectoryName(currentFilePath);

        var sb = new StringBuilder();
        var importRegex = new Regex(
            @"^\s*include\s+""([^""]+)""\s*;",
            RegexOptions.Multiline
        );
        var lastIndex = 0;

        foreach (Match match in importRegex.Matches(src))
        {
            sb.Append(src.Substring(lastIndex, match.Index - lastIndex));
            var importString = match.Groups[1].Value;
            var resolvedPath = ResolveImportPath(currentDir, importString);
            if (resolvedPath == null) throw new FileNotFoundException(currentFilePath);

            var importedContent = LoadRecursive(resolvedPath);
            sb.Append(importedContent);
            lastIndex = match.Index + match.Length;
        }

        sb.Append(src.Substring(lastIndex));
        return sb.ToString();
    }

    private string ResolveImportPath(string? currentDir, string importString)
    {
        var localPath = Path.Combine(currentDir, importString + ".cst");
        if (Path.Exists(localPath)) return Path.GetFullPath(localPath);

        return null;
    }
}
using System.Text;

namespace Cast;

public class StdHelper
{
    private static string[] std = new[] { "types", "vectors", "spaces", "math", "opengl" };
    public static string getStd()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var file in std)
        {
            string path = "std/" + file + ".cst";
            string content = File.ReadAllText(path);
            builder.Append(content);
            builder.Append("\n");
        }
        builder.Append("\n");
        return builder.ToString();
    }
}
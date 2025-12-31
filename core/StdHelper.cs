using System.Reflection;
using System.Text;

namespace Cast;

public class StdHelper
{
    private static string[] std = new[] { "types", "vectors", "spaces", "math", "opengl", "texture" };
    public static string getStd()
    {
        string assemblyPath = System.AppContext.BaseDirectory;
            
        StringBuilder builder = new StringBuilder();
        foreach (var file in std)
        {
            string path = assemblyPath + "/std/" + file + ".cst";
            string content = File.ReadAllText(path);
            builder.Append(content);
            builder.Append("\n");
        }
        builder.Append("\n");
        return builder.ToString();
    }
}
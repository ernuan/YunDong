using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudsMove.Interops
{
    public static class Utils
    {
        public static string GetFileVersion(string file)
        {
            if (File.Exists(file))
                return FileVersionInfo.GetVersionInfo(file).FileVersion ?? "";

            return "";
        }

        public static string ToRegexString(this string value)
        {
            var sb = new StringBuilder();
            foreach (var t in value)
            {
                var escapeCharacters = new[] { '\\', '*', '+', '?', '|', '{', '}', '[', ']', '(', ')', '^', '$', '.' };
                if (escapeCharacters.Any(s => s == t))
                    sb.Append('\\');

                sb.Append(t);
            }

            return sb.ToString();
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPGMakerDecrypter.RGSSAD
{
    public static class ArchivedFileNameUtils
    {
        public static string GetFileName(string name)
        {
            return GetPathParts(name).Last();
        }

        public static string GetPlatformSpecificPath(string name)
        {
            var pathParts = GetPathParts(name);

            pathParts = CleanUnicodeCharacters(pathParts);
            pathParts = CleanInvalidPathCharacters(pathParts);

            return Path.Combine(pathParts);
        }

        private static string[] GetPathParts(string name)
        {
            // Paths in RGSSAD file names are always with Windows-style delimeters
            return name.Split('\\');
        }

        private static string[] CleanUnicodeCharacters(string[] pathParts)
        {
            var cleanedPathParts = new List<string>();
            var unicodeConstantRegex = new Regex(@"(?i)\\(u|U)([0-9]|[A-F])([0-9]|[A-F])([0-9]|[A-F])([0-9]|[A-F])");

            foreach (var pathPart in pathParts)
            {
                cleanedPathParts.Add(unicodeConstantRegex.Replace(pathPart, string.Empty));
            }

            return cleanedPathParts.ToArray();
        }

        private static string[] CleanInvalidPathCharacters(string[] pathParts)
        {
            var cleanedPathParts = new List<string>();

            foreach (var pathPart in pathParts)
            {
                var cleanedPathPart = pathPart;

                foreach(var invalidFileNameChar in Path.GetInvalidFileNameChars())
                {
                    cleanedPathPart = cleanedPathPart.Replace($"{invalidFileNameChar}", string.Empty);
                }

                cleanedPathParts.Add(cleanedPathPart);
            }

            return cleanedPathParts.ToArray();
        }
    }
}

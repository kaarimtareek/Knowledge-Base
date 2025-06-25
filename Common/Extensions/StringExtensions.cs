using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KTS.Common.Extensions
{
    public static class StringExtnesions
    {

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static bool IsPDF(this string value)
        {
            return value.ToLower() == ".pdf";
        }
        public static bool IsRangeAllowed(this string value)
        {
            return value.ToLower() == ".pdf"
                || value.ToLower() == ".mp4"
                || value.ToLower() == ".3gp"
                || value.ToLower() == ".webm"
                || value.ToLower() == ".ogg"
                || value.ToLower() == ".quicktime"
                || value.ToLower() == ".mp3"
                || value.ToLower() == ".wav";
        }
        [DebuggerStepThrough]
        public static List<string> SplitString(this string value)
        {
            return value.Split(',').ToList();
        }
        [DebuggerStepThrough]
        public static List<string> SplitStringBy_(this string value)
        {
            return value.Split('_').ToList();
        }
        [DebuggerStepThrough]
        public static string JoinString(this IList<object> input)
        {
            return input.Select(e => e.ToString()).StrJoin();
        }
        public static string StrJoin(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
        public static string StrJoin(this IEnumerable<string> source)
        {
            return string.Join(",", source);
        }
        public static string ToBase64Encode(this string source)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(source);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string ReplaceBackslash(this string source, string replaceStr)
        {
            return source.Replace("/", replaceStr);
        }
        public static DateTime ToDate(this string value)
        {
            if (value.IsEmpty())
                return default;
            DateTime.TryParse(value, out DateTime result);
            return result;
        }
        public static string SplitString(string value, char separator, int position)
        {
            return value.Split(separator)[position];
        }
        public static string ToShortString(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length));
        }

        public static string GetBetween(this string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
        public static string ExtractBetween(this string source,
                                         string beginDelim,
                                         string endDelim,
                                         bool caseSensitive = false,
                                         bool allowMissingEndDelimiter = false,
                                         bool returnDelimiters = false)
        {
            int at1, at2;

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (caseSensitive)
            {
                at1 = source.IndexOf(beginDelim);
                if (at1 == -1)
                    return string.Empty;

                if (!returnDelimiters)
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length);
                else
                    at2 = source.IndexOf(endDelim, at1);
            }
            else
            {
                at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (at1 == -1)
                    return string.Empty;

                if (!returnDelimiters)
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
                else
                    at2 = source.IndexOf(endDelim, at1, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && at2 == -1)
                return source.Substring(at1 + beginDelim.Length);

            if (at1 > -1 && at2 > 1)
            {
                if (!returnDelimiters)
                    return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);
                else
                    return source.Substring(at1, at2 - at1 + endDelim.Length);
            }

            return string.Empty;
        }
        public static string ToStringNotAvaliable(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "N/A";

            return value;
        }
        public static string ToCamelCase(this string value)
        {
            if (value == null)
                return null;
            if (value.Length > 1)
                return char.ToUpper(value[0]) + value.Substring(1);
            return value.ToUpper();
        }
        public static bool IsZipFile(this string value)
        {
            return Path.GetExtension(value).ToLower() == ".zip";
        }
        public static string ToFormatedProjectField(this string value)
        {
            switch (value)
            {
                case "LevelOfDetail":
                    return "Level Of Detail";
                case "ProjectOutputType":
                    return "Project Output Type";
                case "ProjectType":
                    return "Project  Type";
                default:
                    return value;
            }
        }

        [DebuggerStepThrough]
        public static string RemoveSpecialCharacters(this string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9]", "");
        }

        [DebuggerStepThrough]
        public static int LevenshteinDistance(this string source, string target)
        {
            // Remove special characters from both strings.
            source = RemoveSpecialCharacters(source);
            target = RemoveSpecialCharacters(target);

            int sourceLen = source.Length;
            int targetLen = target.Length;

            // Create a 2D array to store distances.
            int[,] distance = new int[sourceLen + 1, targetLen + 1];

            // Initialize distances for an empty substring case.
            for (int i = 0; i <= sourceLen; i++)
            {
                distance[i, 0] = i;
            }
            for (int j = 0; j <= targetLen; j++)
            {
                distance[0, j] = j;
            }

            // Compute distances using dynamic programming.
            for (int i = 1; i <= sourceLen; i++)
            {
                for (int j = 1; j <= targetLen; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(
                        Math.Min(
                            distance[i - 1, j] + 1,  // Deletion
                            distance[i, j - 1] + 1   // Insertion
                        ),
                        distance[i - 1, j - 1] + cost // Substitution
                    );
                }
            }

            // The last cell contains the Levenshtein distance.
            return distance[sourceLen, targetLen];
        }
        public static string PascalCaseToWords(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Replace capital letter followed by lowercase, if preceded by another capital or the start
            string result = Regex.Replace(input, @"(?<=[a-z])([A-Z])", " $1");
            result = Regex.Replace(result, @"(?<=[A-Z])([A-Z][a-z])", " $1");

            return result;
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;

namespace JFPGeneric
{
    public static class StringExtensions
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }

        public static string RemoveExtraSpaces(this string source)
        {
            return Regex.Replace(source, @"\s+", " ", RegexOptions.Multiline);
        }

        public static string RemoveAllSpaces(this string source)
        {
            return Regex.Replace(source, @"\s+", "", RegexOptions.Multiline);
        }

        public static string DefaultIfNullOrEmpty(this string x)
        {
            return string.IsNullOrEmpty(x) ? string.Empty : x;
        }

        public static string DefaultIfNullOrEmpty(this string x, string defaultValue)
        {
            return string.IsNullOrEmpty(x) ? defaultValue : x;
        }

        public static string NullTrim(this string source)
        {
            return (source ?? string.Empty).Trim();
        }

        public static bool IsNeitherNullNorEmpty(this string x)
        {
            return !x.IsNullOrEmpty();
        }

        public static string TrimOrNull(this string source)
        {
            return source == null 
                ? null 
                : String.IsNullOrWhiteSpace(source) 
                    ? null //if whitespace or empty return null
                    : source.Trim();
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
                return true;
            else
                return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}

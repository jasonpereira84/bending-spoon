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
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return (source == null || !source.Any());
        }

        public static bool IsNeitherNullNorEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source.IsNullOrEmpty<TSource>();
        }
    }
}

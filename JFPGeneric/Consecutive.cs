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

namespace JFPGeneric
{
    public partial class Functions
    {
        public static Boolean IsConsecutive(IEnumerable<int> list)
        {
            var min = list.Min();
            var max = list.Max();
            var all = Enumerable.Range(min, max - min + 1);
            return list.SequenceEqual(all);
        }
    }

    public static class EnumberableExtension
    {
        public static Boolean IsConsecutive(this IEnumerable<int> list)
        {
            return Functions.IsConsecutive(list);
        }
    }
}

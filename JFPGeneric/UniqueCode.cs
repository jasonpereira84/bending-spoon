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
        //16 chars
        public static String GetUniqueCode()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        //19 chars
        public static Int64 GetUniqueCode_Int64()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        //public static string GetUniqueCode()
        //{
        //    string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    string ticks = DateTime.UtcNow.Ticks.ToString();
        //    var code = "";
        //    for (var i = 0; i < characters.Length; i += 2)
        //    {
        //        if ((i + 2) <= ticks.Length)
        //        {
        //            var number = int.Parse(ticks.Substring(i, 2));
        //            if (number > characters.Length - 1)
        //            {
        //                var one = double.Parse(number.ToString().Substring(0, 1));
        //                var two = double.Parse(number.ToString().Substring(1, 1));
        //                code += characters[Convert.ToInt32(one)];
        //                code += characters[Convert.ToInt32(two)];
        //            }
        //            else
        //                code += characters[number];
        //        }
        //    }
        //    return code;
        //}
    }
}

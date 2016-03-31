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
        /*Note: Uppercase 'o' is not present*/
        public static Char[] AlphanumericChars = new char[] { 
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',   //0
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',   //1
                'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U',   //2
                'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e',   //3
                'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',   //4
                'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',   //5
                'z' };                                              //6

        public static String GetRandomCode(UInt16 length, Boolean includeLower = false)
        {
            var retVal = new StringBuilder(length, length);
            var rnd = new Random();
            while(retVal.Length < length)
                retVal.Append(AlphanumericChars[rnd.Next(includeLower ? 62 : 36)]);//since Random.Next returns 0 ~ (maxVal-1)
            return retVal.ToString();
        }

        public static String RandomCode6
        {
            get { return GetRandomCode(6); }
        }

        public static String RandomCode6L
        {
            get { return GetRandomCode(6, true); }
        }
    }
}

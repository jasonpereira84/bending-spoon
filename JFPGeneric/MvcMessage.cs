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

namespace JFPGeneric
{
    public class MvcMessage
    {
        public MvcMessage()
            : this(string.Empty, string.Empty)
        {
        }

        public MvcMessage(String major, String minor)
        {
            Major = major;
            Minor = minor;
        }

        public String Major { get; set; }

        public String Minor { get; set; }
    }
}

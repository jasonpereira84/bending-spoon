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
        public enum BootstrapTextEnum
        {
            [StringValue("text-muted")]
            Muted = 0,
            [StringValue("text-primary")]
            Primary = 1,
            [StringValue("text-success")]
            Success = 2,
            [StringValue("text-info")]
            Info = 3,
            [StringValue("text-warning")]
            Warning = 4,
            [StringValue("text-danger")]
            Danger = 5,
        }

        public MvcMessage()
            : this(string.Empty, string.Empty)
        {
        }

        public MvcMessage(String major, String minor, BootstrapTextEnum textType = BootstrapTextEnum.Muted)
        {
            Major = major;
            Minor = minor;
            TextType = textType;
        }

        public static MvcMessage Uninitialized
        {
            get { return new MvcMessage("FALSE", "Unitialized", BootstrapTextEnum.Muted); }
        }

        public static MvcMessage Success(String message)
        {
            return new MvcMessage("TRUE", message, BootstrapTextEnum.Success);
        }

        public static MvcMessage Muted(String message)
        {
            return new MvcMessage("FALSE", message, BootstrapTextEnum.Muted);
        }

        public static MvcMessage Info(String message)
        {
            return new MvcMessage("FALSE", message, BootstrapTextEnum.Info);
        }

        public static MvcMessage Warning(String message)
        {
            return new MvcMessage("FALSE", message, BootstrapTextEnum.DAnger);
        }

        public static MvcMessage Danger(String message)
        {
            return new MvcMessage("FALSE", message, BootstrapTextEnum.Danger);
        }

        public String Major { get; set; }

        public String Minor { get; set; }

        private BootstrapTextEnum TextType { get; set; }

        public String TextType4Display
        {
            get { return StringEnum.GetStringValue(TextType); }
        }
    }
}

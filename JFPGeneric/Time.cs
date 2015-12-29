using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JFPGeneric
{
    public partial class Functions
    {
        public static TimeSpan RemoveSeconds(TimeSpan timespan)
        {
            return new TimeSpan(timespan.Ticks - (timespan.Ticks % 600000000));
        }
    }

    public static class TimeSpan_
    {
        public static TimeSpan RemoveSeconds(this TimeSpan timespan)
        {
            return Functions.RemoveSeconds(timespan);
        }
    }
}

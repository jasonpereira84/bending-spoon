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
        public static String Get4Display_PHONE(String countryCode, String areaCode, String lineNumber, String extension)
        {
            string retVal = string.Empty;
            retVal += String.IsNullOrWhiteSpace(countryCode) ? string.Empty : countryCode;
            retVal += "("; retVal += String.IsNullOrWhiteSpace(areaCode) ? "?" : areaCode; retVal += ")";
            retVal += String.IsNullOrWhiteSpace(lineNumber) ? "???" : lineNumber;
            retVal += String.IsNullOrWhiteSpace(extension) ? string.Empty : " x" + extension;
            return retVal;
        }

        public static String Get4Display_ADDRESS(String line1, String line2, String line3, String line4, String city, String county, String state, String stateAbbreviation, String postalCode, String country, Boolean showCountry = false)
        {
            string retVal = string.Empty;
            try
            {
                List<string> lines = new List<string>();
                if (!String.IsNullOrWhiteSpace(line1)) { lines.Add(line1); }
                if (!String.IsNullOrWhiteSpace(line2)) { lines.Add(line2); }
                if (!String.IsNullOrWhiteSpace(line3)) { lines.Add(line3); }
                if (!String.IsNullOrWhiteSpace(line4)) { lines.Add(line4); }
                if (!String.IsNullOrWhiteSpace(city)) { lines.Add(city); }
                if (!String.IsNullOrWhiteSpace(county)) { lines.Add(county); }
                if (String.IsNullOrWhiteSpace(postalCode)) 
                { 
                    if (!String.IsNullOrWhiteSpace(state)) { lines.Add(state); }
                    else if (!String.IsNullOrWhiteSpace(stateAbbreviation)) { lines.Add(stateAbbreviation); }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(state) && String.IsNullOrWhiteSpace(stateAbbreviation))
                        lines.Add(postalCode);
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(stateAbbreviation)) { lines.Add(stateAbbreviation + " " + postalCode); }
                        else
                            if (!String.IsNullOrWhiteSpace(state)) { lines.Add(state + " " + postalCode); }
                    }
                }
                if (showCountry)
                    if (!String.IsNullOrWhiteSpace(country)) { lines.Add(country); }
                retVal = string.Join(", ", lines);
            }
            catch (Exception) { }
            return retVal;
        }
    }
}

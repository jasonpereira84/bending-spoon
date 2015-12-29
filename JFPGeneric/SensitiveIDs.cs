using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace JFPGeneric
{
    public partial class Functions
    {
        public static String Get4Display_GovenmentID(CountryEnum country, String govIDTypeCode, String govID)
        {
            string retVal = govIDTypeCode + "-";
            switch(country)
            {
                case CountryEnum.United_States:
                    govID = String.IsNullOrWhiteSpace(govID) ? "????" : govID;
                    retVal += govID.Length < 4 ? string.Empty : govID.GetLast(4);
                    break;
                case CountryEnum.Earth:
                default:
                    retVal += govID;
                    break;
            }
            return retVal;
        }

        public static String Get4Display_GovenmentID(Int32 countryID, String govIDTypeCode, String govID)
        {
            var country = CountryEnum.Earth;

            if (Enum.IsDefined(typeof(CountryEnum), countryID)) { country = (CountryEnum)countryID; }

            return Get4Display_GovenmentID(country, govIDTypeCode, govID);
        }

        public static String Get4Display_BankRoutingNumber(String routingNumber)
        {
            string retVal = "XXXX";
            if(String.IsNullOrEmpty(routingNumber)){routingNumber = "000000000";}
            var numchar = Convert.ToInt32((decimal)Math.Round(routingNumber.Length / 2.0m, 0)) + 1;
            retVal += routingNumber.Substring(numchar);
            return retVal;
        }

        public static String Get4Display_BankAccountNumber(String accountNumber)
        {
            string retVal = "XXXX";
            if (String.IsNullOrEmpty(accountNumber)) { accountNumber = "00000"; }
            var numchar = Convert.ToInt32((decimal)Math.Round(accountNumber.Length / 2.0m, 0)) + 1;
            retVal += accountNumber.Substring(numchar);
            return retVal;
        }

        public static String Get4Display_CardNumber(String cardNumber)
        {
            string retVal = "XXXX";
            if (String.IsNullOrEmpty(cardNumber)) { cardNumber = "0000"; }
            while (cardNumber.Length < 4) { cardNumber = "X" + cardNumber; }
            retVal += cardNumber.Substring(cardNumber.Length - 4);
            return retVal;
        }
    }
}

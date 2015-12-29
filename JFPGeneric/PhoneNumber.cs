using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace JFPGeneric
{
    public class PhoneNumber
    {
        private Char[] _countryCode;
        private Char[] _areaCode;
        private Char[] _lineNumber;
        private Char[] _extension;
        private String _raw;
        private Boolean _valid;
        private CountryEnum _country;
        private String _pattern;
        private String _formatMsg;

        private PhoneNumber()
        {
        }

        public PhoneNumber(String thePhoneNumber)
            : this(thePhoneNumber, CountryEnum.United_States)
        {
        }

        public PhoneNumber(String thePhoneNumber, CountryEnum theCountry)
        {
            _raw = thePhoneNumber;
            _country = theCountry;
            _countryCode = new Char[16];
            _areaCode = new Char[16];
            _lineNumber = new Char[16];
            _extension = new Char[16];
            _valid = false;
            //_pattern = @"^(\d{3})\D?\d{3}\D?\d{4}$";//default to USA
            _pattern = "(^\\()(?<=\\()(\\d{3})(?=\\))(\\))(\\d{3})(-)(\\d{4})";//default to USA
            switch (theCountry)
            {
                case CountryEnum.United_States:
                    _formatMsg = "(AAA)NNN-NNNN";
                    _pattern = "(^\\()(?<=\\()(\\d{3})(?=\\))(\\))(\\d{3})(-)(\\d{4})";
                    Match match = Regex.Match(thePhoneNumber, _pattern, RegexOptions.IgnoreCase);
                    _valid = !thePhoneNumber.StartsWith("(0") && match.Success;
                    if(_valid)
                    {
                        _countryCode = ("1").ToCharArray();//get from DB?
                        _areaCode = match.Groups[2].Value.ToCharArray();
                        _lineNumber = (match.Groups[4].Value + match.Groups[6].Value).ToCharArray();
                    }
                    break;
                default:
                    break;
            }
        }

        public String RAW
        {
            get { return _raw; }
        }

        public Boolean IsValid
        {
            get { return _valid; }
        }

        public CountryEnum Country
        {
            get { return _country; }
        }

        public String CountryCode
        {
            get { return _countryCode.ToString(); }
        }

        public String AreaCode
        {
            get { return _areaCode.ToString(); }
        }

        public String LineNumber
        {
            get { return _lineNumber.ToString(); }
        }

        public String Extension
        {
            get { return _extension.ToString(); }
        }

        public String FormatDescription
        {
            get { return _formatMsg; }
        }
    }

    public class PhoneNumberAttribute : ValidationAttribute
    {
        private string _otherProperty;

        public PhoneNumberAttribute(string theOtherProperty)
        {
            _otherProperty = theOtherProperty;
        }

        public string OtherProperty
        {
            get { return _otherProperty; }
            set { _otherProperty = value; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retVal = new ValidationResult("UnInitialized");
            try
            {
                if (value == null) { value = string.Empty; }

                var raw = value.ToString();
                var country = GetCountry(validationContext);

                string errMsg = (country == CountryEnum.Earth) ? "Country is invalid" : string.Empty;
                errMsg += String.IsNullOrWhiteSpace(raw) ? String.IsNullOrWhiteSpace(errMsg) ? "Phone number cannot be null" : ", Phone number cannot be null" : string.Empty;
                if (!String.IsNullOrWhiteSpace(errMsg)) { retVal = new ValidationResult(errMsg); }
                else
                {
                    var phoneNumber = new PhoneNumber(raw, country);
                    if (phoneNumber.IsValid) { retVal = ValidationResult.Success; }
                    else { retVal = new ValidationResult("Unable to parse to PhoneNumber, format must be: " + phoneNumber.FormatDescription); }
                }
            }
            catch (Exception x) { return new ValidationResult(x.Message); }
            return retVal;
        }

        protected CountryEnum GetCountry(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                try
                {
                    return (CountryEnum)secondValue;
                }
                catch (Exception) { return CountryEnum.Earth; }
            }
            return CountryEnum.Earth;
        }
    }

    public class PhoneNumberAREA
    {
        private Char[] _areaCode;
        private String _raw;
        private Boolean _valid;
        private CountryEnum _country;
        private String _formatMsg;

        private PhoneNumberAREA()
            : this(String.Empty)
        {
        }

        public PhoneNumberAREA(String theAreaCode)
            : this(theAreaCode, CountryEnum.United_States)
        {
        }

        public PhoneNumberAREA(String theAreaCode, CountryEnum theCountry)
        {
            _raw = theAreaCode;
            _country = theCountry;
            _areaCode = new Char[16];
            _valid = false;
            switch (theCountry)
            {
                case CountryEnum.United_States:
                    _formatMsg = "AAA";
                    theAreaCode = Regex.Replace(theAreaCode, "[^0-9]", "");
                    _valid = !theAreaCode.StartsWith("0") && (theAreaCode.Length == 3);
                    if (_valid) { _areaCode = theAreaCode.ToCharArray(); }
                    break;
                default:
                    break;
            }
        }

        public String RAW
        {
            get { return _raw; }
        }

        public Boolean IsValid
        {
            get { return _valid; }
        }

        public CountryEnum Country
        {
            get { return _country; }
        }

        public String AreaCode
        {
            get { return _areaCode.ToString(); }
        }

        public String FormatDescription
        {
            get { return _formatMsg; }
        }
    }

    public class PhoneNumberAREAAttribute : ValidationAttribute
    {
        private string _otherProperty;

        public PhoneNumberAREAAttribute(string theOtherProperty)
        {
            _otherProperty = theOtherProperty;
        }

        public string OtherProperty
        {
            get { return _otherProperty; }
            set { _otherProperty = value; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retVal = new ValidationResult("UnInitialized");
            try
            {
                if (value == null) { value = string.Empty; }

                var raw = value.ToString();
                var country = GetCountry(validationContext);

                string errMsg = (country == CountryEnum.Earth) ? "Country is invalid" : string.Empty;
                errMsg += String.IsNullOrWhiteSpace(raw) ? String.IsNullOrWhiteSpace(errMsg) ? "AreaCode cannot be null" : ", AreaCode cannot be null" : string.Empty;
                if (!String.IsNullOrWhiteSpace(errMsg)) { retVal = new ValidationResult(errMsg); }
                else
                {
                    var areaCode = new PhoneNumberAREA(raw, country);
                    if (areaCode.IsValid) { retVal = ValidationResult.Success; }
                    else { retVal = new ValidationResult("Unable to parse to AreaCode, format must be: " + areaCode.FormatDescription); }
                }
            }
            catch (Exception x) { return new ValidationResult(x.Message); }
            return retVal;
        }

        protected CountryEnum GetCountry(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                try
                {
                    return (CountryEnum)secondValue;
                }
                catch (Exception) { return CountryEnum.Earth; }
            }
            return CountryEnum.Earth;
        }
    }

    public class PhoneNumberLINE
    {
        private Char[] _lineNumber;
        private String _raw;
        private Boolean _valid;
        private CountryEnum _country;
        private String _pattern;
        private String _formatMsg;

        private PhoneNumberLINE()
            : this(String.Empty)
        {
        }

        public PhoneNumberLINE(String theLineNumber)
            : this(theLineNumber, CountryEnum.United_States)
        {
        }

        public PhoneNumberLINE(String theLineNumber, CountryEnum theCountry)
        {
            _raw = theLineNumber;
            _country = theCountry;
            _lineNumber = new Char[16];
            _valid = false;
            _pattern = "(\\d{3})(-)(\\d{4})";//default to USA
            switch (theCountry)
            {
                case CountryEnum.United_States:
                    _formatMsg = "NNN-NNNN";
                    _pattern = "(\\d{3})(-)(\\d{4})";
                    Match match = Regex.Match(theLineNumber, _pattern, RegexOptions.IgnoreCase);
                    _valid = theLineNumber.Length == 8 && match.Success;
                    if (_valid) { _lineNumber = match.Value.ToCharArray(); }
                    break;
                default:
                    break;
            }
        }

        public String RAW
        {
            get { return _raw; }
        }

        public Boolean IsValid
        {
            get { return _valid; }
        }

        public CountryEnum Country
        {
            get { return _country; }
        }

        public String LineNumber
        {
            get { return _lineNumber.ToString(); }
        }

        public String FormatDescription
        {
            get { return _formatMsg; }
        }
    }

    public class PhoneNumberLINEAttribute : ValidationAttribute
    {
        private string _otherProperty;

        public PhoneNumberLINEAttribute(string theOtherProperty)
        {
            _otherProperty = theOtherProperty;
        }

        public string OtherProperty
        {
            get { return _otherProperty; }
            set { _otherProperty = value; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult retVal = new ValidationResult("UnInitialized");
            try
            {
                if (value == null) { value = string.Empty; }

                var raw = value.ToString();
                var country = GetCountry(validationContext);

                string errMsg = (country == CountryEnum.Earth) ? "Country is invalid" : string.Empty;
                errMsg += String.IsNullOrWhiteSpace(raw) ? String.IsNullOrWhiteSpace(errMsg) ? "LineNumber cannot be null" : ", LineNumber cannot be null" : string.Empty;
                if (!String.IsNullOrWhiteSpace(errMsg)) { retVal = new ValidationResult(errMsg); }
                else
                {
                    var lineNumber = new PhoneNumberLINE(raw, country);
                    if (lineNumber.IsValid) { retVal = ValidationResult.Success; }
                    else { retVal = new ValidationResult("Unable to parse to LineNumber, format must be: " + lineNumber.FormatDescription); }
                }
            }
            catch (Exception x) { return new ValidationResult(x.Message); }
            return retVal;
        }

        protected CountryEnum GetCountry(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
                try
                {
                    return (CountryEnum)secondValue;
                }
                catch (Exception) { return CountryEnum.Earth; }
            }
            return CountryEnum.Earth;
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace JFPGeneric
{
    public partial class Functions
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static Dictionary<int, string> ToDictionary(Enum @enum)
        {
            var type = @enum.GetType();
            return Enum.GetValues(type).Cast<int>().ToDictionary(e => e, e => Enum.GetName(type, e));
        }
    }

    public static class EnumExtensions
    {
        public static Dictionary<string, TEnum> ToDictionary<TEnum>()
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Type must be an enumeration");
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().
                    ToDictionary(e => Enum.GetName(typeof(TEnum), e));
        }
    }

    /// <summary>
    /// Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
    /// </summary>
    public class StringEnum
    {
        #region Instance implementation

        private Type _enumType;
        private static Hashtable _stringValues = new Hashtable();

        /// <summary>
        /// Creates a new <see cref="StringEnum"/> instance.
        /// </summary>
        /// <param name="enumType">Enum type.</param>
        public StringEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", enumType.ToString()));

            _enumType = enumType;
        }

        /// <summary>
        /// Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valueName">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public string GetStringValue(string valueName)
        {
            Enum enumType;
            string stringValue = null;
            try
            {
                enumType = (Enum)Enum.Parse(_enumType, valueName);
                stringValue = GetStringValue(enumType);
            }
            catch (Exception) { }//Swallow!

            return stringValue;
        }

        /// <summary>
        /// Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public Array GetStringValues()
        {
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    values.Add(attrs[0].Value);

            }

            return values.ToArray();
        }

        /// <summary>
        /// Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public IList GetListValues()
        {
            Type underlyingType = Enum.GetUnderlyingType(_enumType);
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(_enumType, fi.Name), underlyingType), attrs[0].Value));

            }

            return values;

        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue)
        {
            return Parse(_enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public bool IsStringDefined(string stringValue, bool ignoreCase)
        {
            return Parse(_enumType, stringValue, ignoreCase) != null;
        }

        /// <summary>
        /// Gets the underlying enum type for this instance.
        /// </summary>
        /// <value></value>
        public Type EnumType
        {
            get { return _enumType; }
        }

        #endregion

        #region Static implementation

        /// <summary>
        /// Gets a string value for a particular enum value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            if (_stringValues.ContainsKey(value))
                output = (_stringValues[value] as StringValueAttribute).Value;
            else
            {
                //Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    _stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }

            }
            return output;

        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue)
        {
            return Parse(type, stringValue, false);
        }

        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="stringValue">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type type, string stringValue, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!type.IsEnum)
                throw new ArgumentException(String.Format("Supplied type must be an Enum.  Type was {0}", type.ToString()));

            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in type.GetFields())
            {
                //Check for our custom attribute
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                    enumStringValue = attrs[0].Value;

                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                {
                    output = Enum.Parse(type, fi.Name);
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue)
        {
            return Parse(enumType, stringValue) != null;
        }

        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="stringValue">String value.</param>
        /// <param name="enumType">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
        {
            return Parse(enumType, stringValue, ignoreCase) != null;
        }

        #endregion
    }

    public class StringValueAttribute : Attribute
    {
        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value { get { return _value; } }
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(c.countryCommonName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_'),'Â','_'),'–',''),',','') +  ' = ' + CAST(c.countryID AS VARCHAR) +  ',' from [dbo].[plCountry] c
    public enum CountryEnum
    {
        Earth = 0,
        Afghanistan = 1,
        Albania = 2,
        Algeria = 3,
        Andorra = 4,
        Angola = 5,
        Antigua_and_Barbuda = 6,
        Argentina = 7,
        Armenia = 8,
        Australia = 9,
        Austria = 10,
        Azerbaijan = 11,
        Bahamas, _The = 12,
        Bahrain = 13,
        Bangladesh = 14,
        Barbados = 15,
        Belarus = 16,
        Belgium = 17,
        Belize = 18,
        Benin = 19,
        Bhutan = 20,
        Bolivia = 21,
        Bosnia_and_Herzegovina = 22,
        Botswana = 23,
        Brazil = 24,
        Brunei = 25,
        Bulgaria = 26,
        Burkina_Faso = 27,
        Burundi = 28,
        Cambodia = 29,
        Cameroon = 30,
        Canada = 31,
        Cape_Verde = 32,
        Central_African_Republic = 33,
        Chad = 34,
        Chile = 35,
        China = 36,
        Colombia = 37,
        Comoros = 38,
        Congo_A_Kinshasa_ = 39,
        Congo_A_Brazzaville_ = 40,
        Costa_Rica = 41,
        Cote_d_Ivoire__Ivory_Coast_ = 42,
        Croatia = 43,
        Cuba = 44,
        Cyprus = 45,
        Czech_Republic = 46,
        Denmark = 47,
        Djibouti = 48,
        Dominica = 49,
        Dominican_Republic = 50,
        Ecuador = 51,
        Egypt = 52,
        El_Salvador = 53,
        Equatorial_Guinea = 54,
        Eritrea = 55,
        Estonia = 56,
        Ethiopia = 57,
        Fiji = 58,
        Finland = 59,
        France = 60,
        Gabon = 61,
        Gambia = 62,
        Georgia = 63,
        Germany = 64,
        Ghana = 65,
        Greece = 66,
        Grenada = 67,
        Guatemala = 68,
        Guinea = 69,
        GuineaBissau = 70,
        Guyana = 71,
        Haiti = 72,
        Honduras = 73,
        Hungary = 74,
        Iceland = 75,
        India = 76,
        Indonesia = 77,
        Iran = 78,
        Iraq = 79,
        Ireland = 80,
        Israel = 81,
        Italy = 82,
        Jamaica = 83,
        Japan = 84,
        Jordan = 85,
        Kazakhstan = 86,
        Kenya = 87,
        Kiribati = 88,
        North_Korea = 89,
        South_Korea = 90,
        Kuwait = 91,
        Kyrgyzstan = 92,
        Laos = 93,
        Latvia = 94,
        Lebanon = 95,
        Lesotho = 96,
        Liberia = 97,
        Libya = 98,
        Liechtenstein = 99,
        Lithuania = 100,
        Luxembourg = 101,
        Macedonia = 102,
        Madagascar = 103,
        Malawi = 104,
        Malaysia = 105,
        Maldives = 106,
        Mali = 107,
        Malta = 108,
        Marshall_Islands = 109,
        Mauritania = 110,
        Mauritius = 111,
        Mexico = 112,
        Micronesia = 113,
        Moldova = 114,
        Monaco = 115,
        Mongolia = 116,
        Montenegro = 117,
        Morocco = 118,
        Mozambique = 119,
        Myanmar__Burma_ = 120,
        Namibia = 121,
        Nauru = 122,
        Nepal = 123,
        Netherlands = 124,
        New_Zealand = 125,
        Nicaragua = 126,
        Niger = 127,
        Nigeria = 128,
        Norway = 129,
        Oman = 130,
        Pakistan = 131,
        Palau = 132,
        Panama = 133,
        Papua_New_Guinea = 134,
        Paraguay = 135,
        Peru = 136,
        Philippines = 137,
        Poland = 138,
        Portugal = 139,
        Qatar = 140,
        Romania = 141,
        Russia = 142,
        Rwanda = 143,
        Saint_Kitts_and_Nevis = 144,
        Saint_Lucia = 145,
        Saint_Vincent_and_the_Grenadines = 146,
        Samoa = 147,
        San_Marino = 148,
        Sao_Tome_and_Principe = 149,
        Saudi_Arabia = 150,
        Senegal = 151,
        Serbia = 152,
        Seychelles = 153,
        Sierra_Leone = 154,
        Singapore = 155,
        Slovakia = 156,
        Slovenia = 157,
        Solomon_Islands = 158,
        Somalia = 159,
        South_Africa = 160,
        Spain = 161,
        Sri_Lanka = 162,
        Sudan = 163,
        Suriname = 164,
        Swaziland = 165,
        Sweden = 166,
        Switzerland = 167,
        Syria = 168,
        Tajikistan = 169,
        Tanzania = 170,
        Thailand = 171,
        TimorLeste__East_Timor_ = 172,
        Togo = 173,
        Tonga = 174,
        Trinidad_and_Tobago = 175,
        Tunisia = 176,
        Turkey = 177,
        Turkmenistan = 178,
        Tuvalu = 179,
        Uganda = 180,
        Ukraine = 181,
        United_Arab_Emirates = 182,
        United_Kingdom = 183,
        United_States = 184,
        Uruguay = 185,
        Uzbekistan = 186,
        Vanuatu = 187,
        Vatican_City = 188,
        Venezuela = 189,
        Vietnam = 190,
        Yemen = 191,
        Zambia = 192,
        Zimbabwe = 193,
        Abkhazia = 194,
        Taiwan = 195,
        NagornoKarabakh = 196,
        Northern_Cyprus = 197,
        Pridnestrovie__Transnistria_ = 198,
        Somaliland = 199,
        South_Ossetia = 200,
        Ashmore_and_Cartier_Islands = 201,
        Christmas_Island = 202,
        Cocos__Keeling__Islands = 203,
        Coral_Sea_Islands = 204,
        Heard_Island_and_McDonald_Islands = 205,
        Norfolk_Island = 206,
        New_Caledonia = 207,
        French_Polynesia = 208,
        Mayotte = 209,
        Saint_Barthelemy = 210,
        Saint_Martin = 211,
        Saint_Pierre_and_Miquelon = 212,
        Wallis_and_Futuna = 213,
        French_Southern_and_Antarctic_Lands = 214,
        Clipperton_Island = 215,
        Bouvet_Island = 216,
        Cook_Islands = 217,
        Niue = 218,
        Tokelau = 219,
        Guernsey = 220,
        Isle_of_Man = 221,
        Jersey = 222,
        Anguilla = 223,
        Bermuda = 224,
        British_Indian_Ocean_Territory = 225,
        British_Sovereign_Base_Areas = 226,
        British_Virgin_Islands = 227,
        Cayman_Islands = 228,
        Falkland_Islands__Islas_Malvinas_ = 229,
        Gibraltar = 230,
        Montserrat = 231,
        Pitcairn_Islands = 232,
        Saint_Helena = 233,
        South_Georgia___South_Sandwich_Islands = 234,
        Turks_and_Caicos_Islands = 235,
        Northern_Mariana_Islands = 236,
        Puerto_Rico = 237,
        American_Samoa = 238,
        Baker_Island = 239,
        Guam = 240,
        Howland_Island = 241,
        Jarvis_Island = 242,
        Johnston_Atoll = 243,
        Kingman_Reef = 244,
        Midway_Islands = 245,
        Navassa_Island = 246,
        Palmyra_Atoll = 247,
        US_Virgin_Islands = 248,
        Wake_Island = 249,
        Hong_Kong = 250,
        Macau = 251,
        Faroe_Islands = 252,
        Greenland = 253,
        French_Guiana = 254,
        Guadeloupe = 255,
        Martinique = 256,
        Reunion = 257,
        Aland = 258,
        Aruba = 259,
        Netherlands_Antilles = 260,
        Svalbard = 261,
        Ascension = 262,
        Tristan_da_Cunha = 263,
        Australian_Antarctic_Territory = 264,
        Ross_Dependency = 265,
        Peter_I_Island = 266,
        Queen_Maud_Land = 267,
        British_Antarctic_Territory = 268,
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(p.priorityName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(p.priorityID AS VARCHAR) +  ',' from [dbo].[plPriority] p ORDER BY p.priorityID
    public enum PriorityEnum
    {
        None = 0,

        Primary = 1,
        Secondary = 2,
        Tertiary = 3,
        Quaternary = 4,
        Quinary = 5,
        Senary = 6,
        Septenary = 7,
        Octonary = 8,
        Nonary = 9,
        Denary = 10,
        Monodenary = 11,
        Duodenary = 12,

        #region
        Thirteen = 13,
        Fourteen = 14,
        Fifteen = 15,
        Sixteen = 16,
        Seventeen = 17,
        Eighteen = 18,
        Nineteen = 19,
        Twenty = 20,
        TwentyOne = 21,
        TwentyTwo = 22,
        TwentyThree = 23,
        TwentyFour = 24,
        TwentyFive = 25,
        TwentySix = 26,
        TwentySeven = 27,
        TwentyEight = 28,
        TwentyNine = 29,
        Thirty = 30,
        ThirtyOne = 31,
        ThirtyTwo = 32,
        ThirtyThree = 33,
        ThirtyFour = 34,
        ThirtyFive = 35,
        ThirtySix = 36,
        ThirtySeven = 37,
        ThirtyEight = 38,
        ThirtyNine = 39,
        Forty = 40,
        FortyOne = 41,
        FortyTwo = 42,
        FortyThree = 43,
        FortyFour = 44,
        FortyFive = 45,
        FortySix = 46,
        FortySeven = 47,
        FortyEight = 48,
        FortyNine = 49,
        Fifty = 50,
        FiftyOne = 51,
        FiftyTwo = 52,
        FiftyThree = 53,
        FiftyFour = 54,
        FiftyFive = 55,
        FiftySix = 56,
        FiftySeven = 57,
        FiftyEight = 58,
        FiftyNine = 59,
        Sixty = 60,
        SixtyOne = 61,
        SixtyTwo = 62,
        SixtyThree = 63,
        SixtyFour = 64,
        SixtyFive = 65,
        SixtySix = 66,
        SixtySeven = 67,
        SixtyEight = 68,
        SixtyNine = 69,
        Seventy = 70,
        SeventyOne = 71,
        SeventyTwo = 72,
        SeventyThree = 73,
        SeventyFour = 74,
        SeventyFive = 75,
        SeventySix = 76,
        SeventySeven = 77,
        SeventyEight = 78,
        SeventyNine = 79,
        Eighty = 80,
        EightyOne = 81,
        EightyTwo = 82,
        EightyThree = 83,
        EightyFour = 84,
        EightyFive = 85,
        EightySix = 86,
        EightySeven = 87,
        EightyEight = 88,
        EightyNine = 89,
        Ninety = 90,
        NinetyOne = 91,
        NinetyTwo = 92,
        NinetyThree = 93,
        NinetyFour = 94,
        NinetyFive = 95,
        NinetySix = 96,
        NinetySeven = 97,
        NinetyEight = 98,
        NinetyNine = 99,
        #endregion
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ct.contactInformationTypeName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(ct.contactInformationTypeID AS VARCHAR) +  ',' from [dbo].[plContactInformationType] ct ORDER BY ct.contactInformationTypeID
    public enum ContactInfoTypeEnum
    {
        Home_Phone = 0,
        Work_Phone = 1,
        Mobile_Phone = 2,
        Other_Phone = 3,
        Personal_Email = 4,
        Work_Email = 5,
        Other_Email = 6,
        Home_Address = 7,
        Billing_Address = 8,
        Work_Address = 9,
        Other_Address = 10,
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(cc.contactInformationCategoryName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(cc.contactInformationCategoryID AS VARCHAR) +  ',' from [dbo].[plContactInformationCategory] cc ORDER BY cc.contactInformationCategoryID
    public enum ContactInfoCategoryEnum
    {
        None = -1,
        Phone = 0,
        EMail = 1,
        Street_Address = 2,
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(g.genderName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(g.genderID AS VARCHAR) +  ',' from [dbo].[plGender] g
    public enum GenderEnum
    {
        Unknown = 0,
        Male = 1,
        Female = 2,
        Not_Applicable = 9,
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(r.relationshipTypeName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(r.relationshipTypeID AS VARCHAR) +  ',' from [dbo].[plRelationshipType] r ORDER BY r.relationshipTypeID
    public enum FamilyRoleEnum
    {
        None = 0,
        Other = 1,
        Approved_Pickup = 2,
        Legal_Guardian = 3,
        Ward = 4,
        Fiance = 5,
        Partner = 6,
        Spouse = 7,
        Sibling = 8,
        Parent = 9,
        Child = 10,
        Grandparent = 11,
        Grandchild = 12,
        InLaw__Parent_ = 13,
        InLaw__Child_ = 14,
    }

    //SELECT pt.pertypeName +  ' = ' + CAST(pt.pertypeID AS VARCHAR) +  ',' from [dbo].[plPeriodicityType] pt
    public enum PeriodicityTypeEnum
    {
        Day = 0,
        Month = 2,
        Week = 1,
        Year = 3,
    }

    //SELECT pt.pertypeName +  'ly = ' + CAST(pt.pertypeID AS VARCHAR) +  ',' from [dbo].[plPeriodicityType] pt
    public enum PeriodicityCategoryEnum
    {
        Daily = 0,
        Monthly = 2,
        Weekly = 1,
        Yearly = 3,
    }

    //SELECT REPLACE(p.periodDescription, ' ', '_') +  ' = ' + CAST(p.periodID AS VARCHAR) +  ',' from [dbo].[plPeriodicity] p
    public enum PeriodicityLevelEnum
    {
        Days_of_Year = 0,
        Days_of_Month = 1,
        Days_of_Week = 2,
        Weeks_of_Year = 3,
        Weeks_of_Month = 4,
        Months_of_Year = 5,
    }

    //SELECT '_' + REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(o.ordinalName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_') +  ' = ' + CAST(o.ordinalID AS VARCHAR) +  ',' from [dbo].[plOrdinalNumber] o ORDER BY o.ordinalID;
    public enum OrdinalNumberEnum
    {
        _1st = 1,
        _2nd = 2,
        _3rd = 3,
        _4th = 4,
        _5th = 5,
        _6th = 6,
        _7th = 7,
        _8th = 8,
        _9th = 9,
        _10th = 10,
        _11th = 11,
        _12th = 12,
        _13th = 13,
        _14th = 14,
        _15th = 15,
        _16th = 16,
        _17th = 17,
        _18th = 18,
        _19th = 19,
        _20th = 20,
        _21st = 21,
        _22nd = 22,
        _23rd = 23,
        _24th = 24,
        _25th = 25,
        _26th = 26,
        _27th = 27,
        _28th = 28,
        _29th = 29,
        _30th = 30,
        _31st = 31,
        _32nd = 32,
        _33rd = 33,
        _34th = 34,
        _35th = 35,
        _36th = 36,
        _37th = 37,
        _38th = 38,
        _39th = 39,
        _40th = 40,
        _41st = 41,
        _42nd = 42,
        _43rd = 43,
        _44th = 44,
        _45th = 45,
        _46th = 46,
        _47th = 47,
        _48th = 48,
        _49th = 49,
        _50th = 50,
        _51st = 51,
        _52nd = 52,
        _53rd = 53,
        _54th = 54,
        _55th = 55,
        _56th = 56,
        _57th = 57,
        _58th = 58,
        _59th = 59,
        _60th = 60,
        _61st = 61,
        _62nd = 62,
        _63rd = 63,
        _64th = 64,
        _65th = 65,
        _66th = 66,
        _67th = 67,
        _68th = 68,
        _69th = 69,
        _70th = 70,
        _71st = 71,
        _72nd = 72,
        _73rd = 73,
        _74th = 74,
        _75th = 75,
        _76th = 76,
        _77th = 77,
        _78th = 78,
        _79th = 79,
        _80th = 80,
        _81st = 81,
        _82nd = 82,
        _83rd = 83,
        _84th = 84,
        _85th = 85,
        _86th = 86,
        _87th = 87,
        _88th = 88,
        _89th = 89,
        _90th = 90,
        _91st = 91,
        _92nd = 92,
        _93rd = 93,
        _94th = 94,
        _95th = 95,
        _96th = 96,
        _97th = 97,
        _98th = 98,
        _99th = 99,
        _100th = 100,
    }

    //SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ci.cultureDisplayName, ' ', '_'), '-', ''),'[',''),']',''),'.',''),'+',''),'°',''),'''','_'),'\','_'),'/','_'),'&','_'),'’',''),'(','_'),')','_'),'Â','_'),'–',''),',','') +  ' = ' + CAST(ci.cultureCode AS VARCHAR) +  ',' from [dbo].[plCultureInfo] ci
    public enum CultureInfoEnum
    {
        Afrikaans__South_Africa = 1078,
        Albanian__Albania = 1052,
        Arabic__Algeria = 5121,
        Arabic__Bahrain = 15361,
        Arabic__Egypt = 3073,
        Arabic__Iraq = 2049,
        Arabic__Jordan = 11265,
        Arabic__Kuwait = 13313,
        Arabic__Lebanon = 12289,
        Arabic__Libya = 4097,
        Arabic__Morocco = 6145,
        Arabic__Oman = 8193,
        Arabic__Qatar = 16385,
        Arabic__Saudi_Arabia = 1025,
        Arabic__Syria = 10241,
        Arabic__Tunisia = 7169,
        Arabic__United_Arab_Emirates = 14337,
        Arabic__Yemen = 9217,
        Armenian__Armenia = 1067,
        Azeri__Cyrillic___Azerbaijan = 2092,
        Azeri__Latin___Azerbaijan = 1068,
        Basque__Basque = 1069,
        Belarusian__Belarus = 1059,
        Bulgarian__Bulgaria = 1026,
        Catalan__Catalan = 1027,
        Chinese__China = 2052,
        Chinese__Hong_Kong_SAR = 3076,
        Chinese__Macau_SAR = 5124,
        Chinese__Singapore = 4100,
        Chinese__Taiwan = 1028,
        Chinese__Simplified_ = 4,
        Chinese__Traditional_ = 31748,
        Croatian__Croatia = 1050,
        Czech__Czech_Republic = 1029,
        Danish__Denmark = 1030,
        Dhivehi__Maldives = 1125,
        Dutch__Belgium = 2067,
        Dutch__The_Netherlands = 1043,
        English__Australia = 3081,
        English__Belize = 10249,
        English__Canada = 4105,
        English__Caribbean = 9225,
        English__Ireland = 6153,
        English__Jamaica = 8201,
        English__New_Zealand = 5129,
        English__Philippines = 13321,
        English__South_Africa = 7177,
        English__Trinidad_and_Tobago = 11273,
        English__United_Kingdom = 2057,
        English__United_States = 1033,
        English__Zimbabwe = 12297,
        Estonian__Estonia = 1061,
        Faroese__Faroe_Islands = 1080,
        Farsi__Iran = 1065,
        Finnish__Finland = 1035,
        French__Belgium = 2060,
        French__Canada = 3084,
        French__France = 1036,
        French__Luxembourg = 5132,
        French__Monaco = 6156,
        French__Switzerland = 4108,
        Galician__Galician = 1110,
        Georgian__Georgia = 1079,
        German__Austria = 3079,
        German__Germany = 1031,
        German__Liechtenstein = 5127,
        German__Luxembourg = 4103,
        German__Switzerland = 2055,
        Greek__Greece = 1032,
        Gujarati__India = 1095,
        Hebrew__Israel = 1037,
        Hindi__India = 1081,
        Hungarian__Hungary = 1038,
        Icelandic__Iceland = 1039,
        Indonesian__Indonesia = 1057,
        Italian__Italy = 1040,
        Italian__Switzerland = 2064,
        Japanese__Japan = 1041,
        Kannada__India = 1099,
        Kazakh__Kazakhstan = 1087,
        Konkani__India = 1111,
        Korean__Korea = 1042,
        Kyrgyz__Kazakhstan = 1088,
        Latvian__Latvia = 1062,
        Lithuanian__Lithuania = 1063,
        Macedonian__FYROM_ = 1071,
        Malay__Brunei = 2110,
        Malay__Malaysia = 1086,
        Marathi__India = 1102,
        Mongolian__Mongolia = 1104,
        Norwegian__Bokmål___Norway = 1044,
        Norwegian__Nynorsk___Norway = 2068,
        Polish__Poland = 1045,
        Portuguese__Brazil = 1046,
        Portuguese__Portugal = 2070,
        Punjabi__India = 1094,
        Romanian__Romania = 1048,
        Russian__Russia = 1049,
        Sanskrit__India = 1103,
        Serbian__Cyrillic___Serbia = 3098,
        Serbian__Latin___Serbia = 2074,
        Slovak__Slovakia = 1051,
        Slovenian__Slovenia = 1060,
        Spanish__Argentina = 11274,
        Spanish__Bolivia = 16394,
        Spanish__Chile = 13322,
        Spanish__Colombia = 9226,
        Spanish__Costa_Rica = 5130,
        Spanish__Dominican_Republic = 7178,
        Spanish__Ecuador = 12298,
        Spanish__El_Salvador = 17418,
        Spanish__Guatemala = 4106,
        Spanish__Honduras = 18442,
        Spanish__Mexico = 2058,
        Spanish__Nicaragua = 19466,
        Spanish__Panama = 6154,
        Spanish__Paraguay = 15370,
        Spanish__Peru = 10250,
        Spanish__Puerto_Rico = 20490,
        Spanish__Spain = 3082,
        Spanish__Uruguay = 14346,
        Spanish__Venezuela = 8202,
        Swahili__Kenya = 1089,
        Swedish__Finland = 2077,
        Swedish__Sweden = 1053,
        Syriac__Syria = 1114,
        Tamil__India = 1097,
        Tatar__Russia = 1092,
        Telugu__India = 1098,
        Thai__Thailand = 1054,
        Turkish__Turkey = 1055,
        Ukrainian__Ukraine = 1058,
        Urdu__Pakistan = 1056,
        Uzbek__Cyrillic___Uzbekistan = 2115,
        Uzbek__Latin___Uzbekistan = 1091,
        Vietnamese__Vietnam = 1066,
    }

    //SELECT g.govidTypeCode +  ' = ' + CAST(g.govidTypeID AS VARCHAR) +  ',' from [dbo].[plGovernmentIDType] g
    public enum GovernmentIDTypeEnum
    {
        XX = 0,
        SSN = 1,
        NINO = 2,
    }
}

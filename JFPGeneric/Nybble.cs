using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
    // The Nybble class extends the Value class and 
    // provides object representation of the byte primitive type.
    // 
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable, System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
    public struct Nybble : IComparable, IFormattable, IConvertible
#if GENERICS_WORK 
        , IComparable<byte>, IEquatable<byte>
#endif
    {
        private Nybble m_value;

        // The maximum value that a Byte may represent: 15. 
        public const Nybble MaxValue = (Nybble)0xF;

        // The minimum value that a Byte may represent: 0. 
        public const Nybble MinValue = 0;


        // Compares this object to another object, returning an integer that 
        // indicates the relationship.
        // Returns a value less than zero if this  object 
        // null is considered to be less than any instance. 
        // If object is not of type byte, this method throws an ArgumentException.
        // 
        public int CompareTo(Object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is Byte))
            {
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeByte"));
            }

            return m_value - (((Byte)value).m_value);
        }

        public int CompareTo(Nybble value)
        {
            return m_value - value;
        }

        // Determines whether two Nybble objects are equal. 
        public override bool Equals(Object obj)
        {
            if (!(obj is Nybble))
            {
                return false;
            }
            return m_value == ((Nybble)obj).m_value;
        }

        public bool Equals(Nybble obj)
        {
            return m_value == obj;
        }

        // Gets a hash code for this instance.
        public override int GetHashCode()
        {
            return m_value;
        }

        public static byte Parse(String s)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static byte Parse(String s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static byte Parse(String s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }


        // Parses an unsigned byte from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's 
        // NumberFormatInfo is assumed.
        public static byte Parse(String s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Parse(s, style, NumberFormatInfo.GetInstance(provider));
        }

        private static byte Parse(String s, NumberStyles style, NumberFormatInfo info)
        {
            int i = 0;
            try
            {
                i = Number.ParseInt32(s, style, info);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(Environment.GetResourceString("Overflow_Byte"), e);
            }

            if (i < MinValue || i > MaxValue) throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
            return (byte)i;
        }

        public static bool TryParse(String s, out Byte result)
        {
            return TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(String s, NumberStyles style, IFormatProvider provider, out Byte result)
        {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        private static bool TryParse(String s, NumberStyles style, NumberFormatInfo info, out Byte result)
        {
            result = 0;
            int i;
            if (!Number.TryParseInt32(s, style, info, out i))
            {
                return false;
            }
            if (i < MinValue || i > MaxValue)
            {
                return false;
            }
            result = (byte)i;
            return true;
        }

        public override String ToString()
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(String format)
        {
            return Number.FormatInt32(m_value, format, NumberFormatInfo.CurrentInfo);
        }

        public String ToString(IFormatProvider provider)
        {
            return Number.FormatInt32(m_value, null, NumberFormatInfo.GetInstance(provider));
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return Number.FormatInt32(m_value, format, NumberFormatInfo.GetInstance(provider));
        }

        //
        // IValue implementation 
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.Byte;
        }


        /// <internalonly>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(m_value);
        }

        /// <internalonly> 
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(m_value);
        }

        /// <internalonly> 
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(m_value);
        }

        /// <internalonly>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return m_value;
        }

        /// <internalonly>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(m_value);
        }

        /// <internalonly> 
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(m_value);
        }

        /// <internalonly>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(m_value);
        }

        /// <internalonly> 
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(m_value);
        }

        /// <internalonly>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value);
        }

        /// <internalonly>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }

        /// <internalonly> 
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(m_value);
        }

        /// <internalonly> 
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(m_value);
        }

        /// <internalonly>
        Decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(m_value);
        }

        /// <internalonly>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(String.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), "Nybble", "DateTime"));
        }

        /// <internalonly> 
        Object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType((IConvertible)this, type, provider);
        }
    }
}
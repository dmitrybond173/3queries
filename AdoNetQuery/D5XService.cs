/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * Compatibility layer for CSV support (copy-paste from other project).
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Shopfloor.Borland.Delphi5
{
    public sealed class XService
    {
        #region XService.pas

		public struct SYSTEMTIME 
		{
			public ushort wYear;
			public ushort wMonth; 
			public ushort wDayOfWeek; 
			public ushort wDay; 
			public ushort wHour; 
			public ushort wMinute; 
			public ushort wSecond; 
			public ushort wMilliseconds; 
		}

        /*
        [DllImport("coredll.dll")]
        public extern static void GetLocalTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("coredll.dll")]
        public extern static uint SetLocalTime(ref SYSTEMTIME lpSystemTime);

        public static void SetSysTime(DateTime dt)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            //GetLocalTime(ref st);
			
            st.wYear = (ushort)dt.Year; 
            st.wMonth = (ushort)dt.Month; 
            st.wDay = (ushort)dt.Day; 

            st.wHour = (ushort)dt.Hour; 
            st.wMinute = (ushort)dt.Minute; 
            st.wSecond = (ushort)dt.Second; 
            st.wMilliseconds = (ushort)dt.Millisecond;

            SetLocalTime(ref st);
        }
        */

        public enum ArrType
        {
            Bytes = 0,
            Chars = 1,
            ShortInts = 2,
            SmallInts = 3,
            Words = 4,
            Integers = 5,
            LongInts = 6,
            LongWords = 7,
            Pointers = 8,
            Reals = 9,
            Doubles = 10
        }

        public struct SegArr 
        {
            public const ulong SegSize = 0x10000;            
            public Array Arr;

            public SegArr(int arrType)
            {
                switch ( arrType )
                {
                    case 0: Arr = new byte[SegSize]; break;
                    case 1: Arr = new char[SegSize]; break;
                    case 2: Arr = new sbyte[SegSize]; break;
                    case 3: Arr = new short[SegSize / 2]; break;
                    case 4: Arr = new ushort[SegSize / 2]; break;
                    case 5: Arr = new int[SegSize / 4]; break; //4294967294]; //(2*int.MaxValue)];
                    case 6: Arr = new int[SegSize / 4]; break;
                    case 7: Arr = new uint[SegSize / 4]; break;
                    case 8: Arr = new object[SegSize]; break;
                    case 9: Arr = new double[SegSize / 8]; break;
                    case 10: Arr = new double[SegSize / 8]; break;
                    default: Arr = null; break;
                }
            }
        }

        public static string AsDump(object AData)
        {
            string frm = (AData is string ? "\"" : "");
            return String.Format("{0}:{1}{2}{1}", GetTypeName(AData), frm, (AData != null ? AData.ToString() : "(null)"));
        }

        public static string GetTypeName(object AData)
        {
            string tn = (AData != null ? AData.GetType().ToString() : "(null)");
            if (tn.StartsWith("System.")) tn = tn.Remove(0, 7);
            return tn;
        }

		public static string _IDENTIFIER_1ST_CHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
		public static string _IDENTIFIER = _IDENTIFIER_1ST_CHAR + "0123456789";

		public static bool IsValidIdentifier(string id)
		{
			if (id == null || id.Length == 0) 
				return false;
			if (_IDENTIFIER_1ST_CHAR.IndexOf(id[0]) < 0) 
				return false;
			for (int i=1; i<id.Length; i++)
				if (_IDENTIFIER.IndexOf(id[i]) < 0) 
					return false;
			return true;
		}

        public static bool IsDigitChar(char ch, int nBase)
        {
            int n = -1;
            if (ch >= '0' && ch <= '9') n = (int)ch - (int)'0';
            if (ch >= 'A' && ch <= 'Z') n = (int)ch - (int)'A' + 10;
            if (ch >= 'a' && ch <= 'z') n = (int)ch - (int)'a' + 10;
            return (n >= 0 && n < nBase);
        }

        // Character escapes and their replacements
        static char[,] escChars = new char[10, 2] 
		{
			{'a', '\x07'}, {'b', '\x08'}, {'f', '\x0C'},
			{'t', '\x09'}, {'n', '\x0A'}, {'r', '\x0D'},
			{'v', '\x0B'}, {'\\', '\\'},  {'\'', '\''},
			{'\"', '\"'}
		};
 
        /// <summary>
        /// Check if passed string contains valid character escapes
        /// </summary>
        /// <param name="srcStr">String to be checked</param>
        /// <param name="realChar">Character by which character escapes should be replaced</param>
        /// <param name="len">Length of character escapes</param>
        /// <param name="escSeq">The string w/o character escapes</param>
        /// <returns>True if character escapes are valid</returns>
        public static bool IsValidEscSeq(string srcStr, out char realChar, out int len, out string escSeq)
        {
			if (srcStr == null || srcStr.Length == 0)
			{
				realChar = '\x00';
				len = 0;
				escSeq = "";
				return false;
			}

            bool result = true;
            string s = string.Empty;
            int j = 0;
            int n = 0;

            escSeq = string.Empty;      
            len = 2;
            realChar = srcStr[0]; // ( (srcStr != null && srcStr.Length > 0) ? srcStr[0] : '\x00' ); <-- now validated before this code reached
            if (srcStr.Length < 2)
                return true;

            for (int i = 0; i < escChars.GetLength(0); i++)
            {
                if (srcStr[1] == escChars[i, 0])
                {
                    realChar = escChars[i, 1];
                    escSeq = srcStr.Substring(0, len);
                    return result;
                }
            }

            switch (srcStr[1])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                {
                    j = 0;
                    len = srcStr.Length - 1;
                    while ((j < 3) && (j < len) && ("01234567".IndexOf(srcStr[j + 1]) >= 0)) 
                    {
                        s = s + srcStr[j + 1];
                        j++;
                    }
                    len = s.Length + 1;
                    realChar = (char)Convert.ToInt32(s, 8);
                    break;
                }
                case 'x':
                {
                    j = 0;
                    len = srcStr.Length - 1;
                    while ((j < 2) && (j < len) && (Uri.IsHexDigit(srcStr[j + 2]))) 
                    {
                        s = s + srcStr[j + 2];
                        j++;
                    }
                    len = s.Length + 2;
                    if (len == 1)
                        result = false;
                    else
                        realChar = (char)Convert.ToInt32(s, 16);
                    break;
                }
                case 'd':
                {
                    j = 0;
                    len = srcStr.Length - 1;            
                    while ((j < 3) && (j < len) && char.IsDigit(srcStr[j + 2])) 
                    {
                        s = s + srcStr[j + 2];
                        j++;
                    }
                    len = s.Length + 2;
                    if (len == 1)
                        result = false;
                    else
                    {
                        n = Convert.ToInt32(s);
                        realChar = (char)n;
                    }
                    break;
                }
                default:
                {
                    result = false;
                    escSeq = srcStr.Substring(0, len);
                    break;
                }
            }
            if (result)
                escSeq = srcStr.Substring(0, len);
        
            return result;
        }

        /// <summary>
        /// Compile (replace character escapes) from string
        /// </summary>
        /// <param name="s">String to be compiled</param>
        /// <param name="disableToCompile">String of characters by which replacement should not be performed</param>
        /// <param name="ignoreError">Set to true to raise error for not valid character escapes</param>
        /// <returns>Compiled string</returns>
        public static string CompileStr(string s, string disableToCompile, bool ignoreError)
        {
            string result = string.Empty;
            string res = string.Empty;
            string errS = string.Empty;
            int len = s.Length;
            int i = 0;
            char realChar;
            while (i < len) 
            {
                int seqLen = 1;
                if (s[i] == '\\')
                {
                    if (!IsValidEscSeq(s.Substring(i, s.Length-i), out realChar, out seqLen, out errS))
                    {
                        if (ignoreError)
                        {
                            realChar = s[i];
                            seqLen = 1;
                        }
                        else
                        {
                            i += seqLen; 
                            if (i > len) i = len;
                            throw new ApplicationException(String.Format("invalid escape sequence \"{0}\" in string \"{1}[...]\"",
                                errS, s.Substring(0, i)));
                        }
                    }
					if (disableToCompile.IndexOf(realChar) < 0)
					{
						res += realChar;
					}
					else
					{
						seqLen = 1;
						res += s[i];
					}
                }
                else
                {
                    res += s[i];
                }
                i += seqLen;
            }
            result = res;
            return result;
        }

        public static string GetToStr(string S, string SubStr)
        {
            int i = S.IndexOf(SubStr);
            if (i >= 0)
                return S.Substring(0, i);
            return S;
        }

        public static string ReversStr(string S)
        {
            string result = "";
            for (int i=0; i<S.Length; i++) 
            {
                result = result.Insert(0, "" + S[i]);
            }
            return result;
        }

        public static string GetAfterStr(string S, string SubStr)
        {
            int i = S.IndexOf(SubStr);
            if (i >= 0)
            {
                int L = SubStr.Length;
                return S.Substring(i + L, S.Length - i - L);
            }
            return S;
        }

        public static bool GetAsBool(string s, out bool Value)
        {
            Value = false;
            s = s.Trim().ToLower();
            if (s == "yes" || s == "true" || s == "1")
            {
                Value = true;
                return true;
            }
            else if (s == "no" || s == "false" || s == "0")
            {
                Value = false;
                return true;
            }
            return false;
        }

        private static char[] _DIGITS_ARR = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

        public static bool GetAsInt(string s, out int Value)
        {
            Value = -1;
			if (s.Length == 0) return false;
			int sign = 1;
            s = s.Trim().ToLower();
            if (s.StartsWith("-")) { sign = -1; s = s.Remove(0, 1); }
			else if (s.StartsWith("+")) { sign = +1; s = s.Remove(0, 1); }
            // if only digits in string ...
            if (s.Trim(_DIGITS_ARR) == string.Empty)
            {
                Value = Convert.ToInt32(s) * sign;
                return true;
            }
            return false;
        }
        
        public static bool GetAsDouble(string s, out double Value)
        {
            Value = -1;
			if (s.Length == 0) return false;
			int sign = 1;
            s = s.Trim().ToLower();
            if (s.StartsWith("-")) { sign = -1; s = s.Remove(0, 1); }
            else if (s.StartsWith("+")) { sign = +1; s = s.Remove(0, 1); }			
            // if only digits in string ...
			try
            {
				string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				if (s.IndexOf(sep) < 0 && sep != "." && s.IndexOf(".") >= 0)
					s = s.Replace(".", sep);
				else if (s.IndexOf(sep) < 0 && sep != "," && s.IndexOf(",") >= 0)
					s = s.Replace(",", sep);
				Value = Convert.ToDouble(s) * sign;
                return true;
            }
			catch (Exception) { }
            return false;
        }

        /// <summary>
        /// Parse string of format "key1=value1; key2=value2; ... keyN=valueN;"
        /// To split key and value instead of '=' you could also use ':'.
        /// </summary>
        /// <param name="parameters">String to parse</param>
        /// <param name="forceLowerCaseKeys">If true all key names will be lowercased</param>
        /// <returns>Hastable object that contains parsed parameters.</returns>
        private static char[] nv_delims = new char[] { '=', ':' };
        public static Hashtable ParseParametersStr(string parameters, bool forceLowercaseKeys)
        {
            Hashtable result = new Hashtable();
            string[] items = parameters.Split(';');
            foreach (string item in items)
            {
                // skip empty items
                if (item.Trim() == string.Empty) continue;

                // search name-value delimiter
                int p = item.IndexOfAny(nv_delims);
                string key = "";
                string val = "";
                if (p >= 0) // if found ...
                {
                    key = item.Substring(0, p).Trim();
                    val = item.Remove(0, p + 1);
                }
                else
                    key = item;

                // put new item to collection
                if (forceLowercaseKeys) key = key.ToLower();
                result[key] = val;
            }
            return result;
        }

        public static RTL.TStringList GetListFromStr(string parameters, bool isCaseSensitive)
        {
            RTL.TStringList result = new RTL.TStringList();
            result.IsCaseSensitive = isCaseSensitive;
            string[] items = parameters.Split(';');
            foreach (string item in items)
            {
                // skip empty items
                if (item.Trim() == string.Empty) continue;

                // search name-value delimiter
                int p = item.IndexOfAny(nv_delims);
                string key = "";
                string val = "";
                if (p >= 0) // if found ...
                {
                    key = item.Substring(0, p).Trim();
                    val = item.Remove(0, p + 1);
                }
                else
                    key = item;

                // put new item to collection
                //if (forceLowercaseKeys) key = key.ToLower();
                result.Add(string.Format("{0}={1}", key, val));
            }
            return result;
        }
        #endregion // XService.pas
    }
}

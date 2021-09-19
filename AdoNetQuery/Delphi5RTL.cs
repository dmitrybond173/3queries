/*
  Some Delphi 5 RTL facilities to make code porting process easier.

  Author: Dmitry Bond.
  Date: October, 2006
*/

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using Shopfloor.Parser.Services;


namespace Shopfloor.Borland.Delphi5
{
    /// <summary>
    /// CompatibilityError
    /// </summary>
    public class RTLError : Exception
    {
        public RTLError(string message)
            : base(message)
        {
        }
    }


    /// <summary>
    /// CompatibilityError
    /// </summary>
    public class CompatibilityError : RTLError
    {
        public CompatibilityError(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// ConvertError
    /// </summary>
    public class ConvertError : RTLError
    {
        public ConvertError(string message)
            : base(message)
        {
        }
    }

    public enum EFileAttr
    {
        faReadOnly = 0x00000001,
        faHidden = 0x00000002,
        faSysFile = 0x00000004,
        faVolumeID = 0x00000008,
        faDirectory = 0x00000010,
        faArchive = 0x00000020,
        faAnyFile = 0x0000003F,
    }

    public enum EVariantType
    {
        varEmpty = 0x0000,
        varNull = 0x0001,
        varSmallint = 0x0002,
        varInteger = 0x0003,
        varSingle = 0x0004,
        varDouble = 0x0005,
        varCurrency = 0x0006,
        varDate = 0x0007,
        varOleStr = 0x0008,
        varDispatch = 0x0009,
        varError = 0x000A,
        varBoolean = 0x000B,
        varVariant = 0x000C,
        varUnknown = 0x000D,
        varByte = 0x0011,

        varStrArg = 0x0048,

        varString = 0x0100,
        varAny = 0x0101,
        varTypeMask = 0x0FFF,
        varArray = 0x2000,

        varByRef = 0x4000,
    }

    /// <summary>
    /// RTL
    /// </summary>
    public sealed class RTL
    {
        public static string _COMMA = ",";
        public static string _LF = "\n";
        public static string _CR = "\r";
        public static string _CRLF = _CR + _LF;
        //public static string _SPACES = " \t\r\n";
        public static string _EMPTY =
            "\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x20";

        // just all chars in range \x00..\x20 and '\"' and ','
        public static char[] _EMPTY_CHARS = new char[] 
					{
						'\x00','\x01','\x02','\x03', '\x04','\x05','\x06','\x07',
						'\x08','\x09','\x0A','\x0B', '\x0C','\x0D','\x0E','\x0F',
						'\x10','\x11','\x12','\x13', '\x14','\x15','\x16','\x17',
						'\x18','\x19','\x1A','\x1B', '\x1C','\x1D','\x1E','\x1F',
						'\x20','\"',','
					};

        internal class StringComparer : IComparer
        {

            private Boolean isCaseSensitive = false;

            public StringComparer(Boolean aIsCaseSensitive)
            {
                isCaseSensitive = aIsCaseSensitive;
            }

            #region IComparer inteface
            int IComparer.Compare(object x, object y)
            {
                return do_Compare((string)x, (string)y);
            }
            #endregion IComparer inteface

            #region Inteface
            public Boolean IsCaseSensitive
            {
                get { return isCaseSensitive; }
                set { isCaseSensitive = value; }
            }

            public int Compare(String x, String y)
            {
                return do_Compare(x, y);
            }
            #endregion Inteface

            #region Implementation
            private int do_Compare(String x, String y)
            {
                if (isCaseSensitive)
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(x, y);

                return CultureInfo.CurrentCulture.CompareInfo.Compare(x, y, CompareOptions.IgnoreCase);
            }
            #endregion Implementation
        }

        public class TStringList : IList
        {
            #region ICollection interface
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            int ICollection.Count { get { return this.items.Count; } }

            bool ICollection.IsSynchronized { get { return this.items.IsSynchronized; } }

            object ICollection.SyncRoot { get { return this.items.SyncRoot; } }

            void ICollection.CopyTo(Array array, int index)
            {
                this.items.CopyTo(array, index);
            }
            #endregion // ICollection

            #region IList interface
            bool IList.IsFixedSize
            {
                get { return this.items.IsFixedSize; }
            }

            bool IList.IsReadOnly
            {
                get { return this.items.IsReadOnly; }
            }

            object IList.this[int index]
            {
                get { return this.items[index]; }
                set { this.items[index] = value.ToString(); }
            }

            int IList.Add(object value)
            {
                return do_Add(value.ToString(), null);
            }

            void IList.Clear()
            {
                do_Clear();
            }

            bool IList.Contains(object value)
            {
                return this.items.Contains(value);
            }

            int IList.IndexOf(object value)
            {
                return this.items.IndexOf(value);
            }

            void IList.Insert(int index, object value)
            {
                do_Insert(index, value.ToString(), null);
            }

            void IList.Remove(object value)
            {
                do_Remove(value.ToString());
            }

            void IList.RemoveAt(int index)
            {
                do_RemoveAt(index);
            }
            #endregion // IList

            #region Interface
            public int Count { get { return this.items.Count; } }

            public int Add(string pValue)
            {
                return do_Add(pValue, null);
            }

            public void Append(string pValue)
            {
                do_Add(pValue, null);
            }

            public void AddStrings(TStringList pList)
            {
                for (int i = 0; i < pList.Count; i++)
                {
                    if (pList.IsStoreObjects)
                        AddObject(pList[i], pList.getObjectAt(i));
                    else
                        Add(pList[i]);
                }
            }

            public int AddObject(string pValue, object pObj)
            {
                return do_Add(pValue, pObj);
            }

            public void Insert(int pIndex, string pValue)
            {
                do_Insert(pIndex, pValue, null);
            }

            public void InsertObject(int pIndex, string pValue, object pObj)
            {
                do_Insert(pIndex, pValue, pObj);
            }

            public void Delete(int pIndex)
            {
                do_RemoveAt(pIndex);
            }

            public void Clear()
            {
                do_Clear();
            }

            public void Move(int pCurIndex, int pNewIndex)
            {
                if (pCurIndex != pNewIndex)
                {
                    string S = this[pCurIndex];
                    object O = (this.IsStoreObjects ? this.objects[pCurIndex] : null);
                    Delete(pCurIndex);
                    InsertObject(pNewIndex, S, O);
                }
            }

            public void Exchange(int pOldIndex, int pNewIndex)
            {
                if (pOldIndex != pNewIndex)
                {
                    object O = this.items[pOldIndex];
                    object N = this.items[pNewIndex];
                    this.items[pNewIndex] = O;
                    this.items[pOldIndex] = N;

                    if (this.IsStoreObjects)
                    {
                        O = this.objects[pOldIndex];
                        N = this.objects[pNewIndex];
                        this.objects[pNewIndex] = O;
                        this.objects[pOldIndex] = N;
                    }
                }
            }

            public void Reverse()
            {
                int n = (int)(this.Count / 2);
                int i = 0;
                while (i < n)
                {
                    this.Exchange(i, this.Count - i - 1);
                    i++;
                }
            }

            public void LoadFromFile(string pFilename)
            {
                this.Clear();
                using (StreamReader sr = File.OpenText(pFilename))
                {
                    this.Text = sr.ReadToEnd();
                }
            }

            public void LoadFromStream(Stream pStrm)
            {
                this.Clear();
                using (StreamReader sr = new StreamReader(pStrm))
                {
                    this.Text = sr.ReadToEnd();
                }
            }

            public void SaveToFile(string pFilename)
            {
                using (StreamWriter sw = File.CreateText(pFilename))
                {
                    sw.Write(this.Text);
                }
            }

            public void SaveToStream(Stream pStrm)
            {
                using (StreamWriter sw = new StreamWriter(pStrm))
                {
                    sw.Write(this.Text);
                }
            }

            public void Sort()
            {
                string[] sorted = new string[this.Count];
                for (int i = 0; i < sorted.Length; i++)
                    sorted[i] = this[i];

                Array.Sort(sorted, 0, this.Count, comparer);

                this.Clear();
                for (int i = 0; i < sorted.Length; i++)
                    this.Add(sorted[i]);
            }

            public string this[int index]
            {
                get { return (string)this.items[index]; }
                set { this.items[index] = value; }
            }

            public bool IsCaseSensitive
            {
                get { return this.isCaseSensitive; }
                set
                {
                    comparer.IsCaseSensitive = value;
                    this.isCaseSensitive = value;
                }
            }

            public bool IsStoreObjects
            {
                get { return (this.objects != null); }
                set
                {
                    if (value)
                    {
                        if (this.objects == null)
                        {
                            this.objects = new ArrayList();
                            for (int i = 0; i < this.Count; i++)
                                this.objects.Add(null);
                        }
                    }
                    else
                    {
                        if (this.objects != null)
                        {
                            this.objects.Clear();
                            this.objects = null;
                        }
                    }
                }
            }

            public ArrayList Strings
            {
                get { return this.items; }
            }

            public ArrayList Objects
            {
                get { return this.objects; }
            }

            public string getNameAt(int index)
            {
                return XService.GetToStr(this[index], "=");
            }

            public void setNameAt(int index, string pName)
            {
                string s = this[index];
                int n = s.IndexOf("=");
                if (n >= 0)
                    this[index] = pName + "=" + getValueAt(index);
                else
                    this[index] = pName + "=" + s;
            }

            public string getValueAt(int index)
            {
                return XService.GetAfterStr(this[index], "=");
            }

            public void setValueAt(int index, string pValue)
            {
                string s = this[index];
                int n = s.IndexOf("=");
                if (n >= 0)
                    this[index] = getNameAt(index) + "=" + pValue;
                else
                    this[index] = s + "=" + pValue;
            }

            public object getObjectAt(int index)
            {
                if (this.IsStoreObjects)
                    return this.objects[index];
                else
                    return null;
            }

            public void setObjectAt(int index, object pObj)
            {
                if (!this.IsStoreObjects) this.IsStoreObjects = true;
                this.objects[index] = pObj;
            }

            public int IndexOf(string S)
            {
                bool isMatch;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.isCaseSensitive)
                        isMatch = AnsiCompareStr(this[i], S) == 0;
                    else
                        isMatch = SameText(this[i], S);
                    if (isMatch)
                        return i;
                }
                return -1;
            }

            public int IndexOfName(string Name)
            {
                bool isMatch;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.isCaseSensitive)
                        isMatch = AnsiCompareStr(getNameAt(i), Name) == 0;
                    else
                        isMatch = SameText(getNameAt(i), Name);
                    if (isMatch)
                        return i;
                }
                return -1;
            }

            public int IndexOfObject(object Obj)
            {
                if (!this.IsStoreObjects) return -1;

                for (int i = 0; i < this.Count; i++)
                {
                    if (Obj == this.objects[i])
                        return i;
                }
                return -1;
            }

            public string getValue(string Name)
            {
                bool isMatch;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.isCaseSensitive)
                        isMatch = AnsiCompareStr(getNameAt(i), Name) == 0;
                    else
                        isMatch = SameText(getNameAt(i), Name);
                    if (isMatch)
                        return getValueAt(i);
                }
                return string.Empty;
            }

            public string CommaText
            {
                get { return getCommaText(); }
                set { setCommaText(value); }
            }

            public string Text
            {
                get { return getText(); }
                set { setText(value); }
            }
            #endregion // Interface

            #region Implementation
            private string getCommaText()
            {
                if (this.items.Count == 1 && ((string)this.items[0]) == string.Empty)
                    return "\"\"";

                StringBuilder sb = new StringBuilder(this.items.Count * 20);
                foreach (string s in this.items)
                {
                    if (s.IndexOfAny(_EMPTY_CHARS) >= 0)
                        sb.Append(AnsiQuotedStr(s, '\"'));
                    else
                        sb.Append(s);
                    sb.Append(_COMMA);
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            private void setCommaText(string AText)
            {
                do_Clear();
                AdvStringReader sr = new AdvStringReader(AText);
                // skip leading spaces
                sr.SkipWhile(_EMPTY);
                // start parsing 
                if (!sr.EOF)
                {
                    int ch;
                    string s;
                    do
                    {
                        ch = sr.Lookup();
                        if (sr.EOF) continue;
                        if (ch == '\"')
                        {
                            s = AnsiExtractQuotedStr(sr, '\"');
                        }
                        else
                        {
                            sr.SetMarker();
                            do { ch = sr.Read(); }
                            while (!sr.EOF && ((char)ch > '\x20' && (char)ch != ','));
                            s = sr.GetMarked();
                            if (s.EndsWith(_COMMA)) s = s.Remove(s.Length - 1, 1);
                            s = s.Trim(_EMPTY_CHARS);
                        }
                        do_Add(s, null);
                        sr.SkipWhile(_EMPTY);
                        if (!sr.EOF && sr.Lookup() == ',')
                            sr.SkipWhile(_EMPTY);
                    }
                    while (!sr.EOF);
                }
            }

            private string getText()
            {
                if (this.items.Count < 1)
                    return _CRLF;

                StringBuilder sb = new StringBuilder(this.items.Count * 20);
                foreach (string s in this.items)
                {
                    sb.Append(s);
                    sb.Append(_CRLF);
                }
                return sb.ToString();
            }

            private void setText(string AText)
            {
                do_Clear();

                string originalText = AText;
                AText = AText.Replace(_CRLF, _LF);
                if (AText.Length > 0 && !AText.EndsWith(_LF)) AText += _LF;
                if (AText.EndsWith(_LF)) AText = AText.Remove(AText.Length - 1, 1);
                if (string.IsNullOrEmpty(AText))
                {
                    if (!string.IsNullOrEmpty(originalText))
                        do_Add("", null);
                    return;
                }

                string[] list = AText.Split(_LF[0]);
                for (int i = 0; i < list.Length; i++)
                {
                    //if (i == list.Length-1) continue;
                    do_Add(list[i], null);
                }
            }

            private int do_Add(string S, object Obj)
            {
                if (this.IsStoreObjects) this.objects.Add(Obj);
                return this.items.Add(S);
            }

            private void do_Remove(string S)
            {
                int index = this.IndexOf(S);
                if (index >= 0)
                {
                    if (this.IsStoreObjects) this.objects.RemoveAt(index);
                    this.items.RemoveAt(index);
                }
            }

            private void do_RemoveAt(int pIndex)
            {
                if (pIndex >= 0)
                {
                    if (this.IsStoreObjects) this.objects.RemoveAt(pIndex);
                    this.items.RemoveAt(pIndex);
                }
            }

            private void do_Insert(int pIndex, string S, object Obj)
            {
                if (this.IsStoreObjects) this.objects.Insert(pIndex, Obj);
                this.items.Insert(pIndex, S);
            }

            private void do_Clear()
            {
                if (this.IsStoreObjects) this.objects.Clear();
                this.items.Clear();
            }

            private ArrayList items = new ArrayList();
            private ArrayList objects = new ArrayList();

            private bool isCaseSensitive = false;
            private StringComparer comparer = new StringComparer(false);

            #endregion Implementation
        }
        // TStringList

        #region Standard RTL

        private static Random rnd = new Random();

        public static double Rnd()
        {
            return rnd.NextDouble();
        }

        public static string AnsiExtractQuotedStr(AdvStringReader sr, char Quote)
        {
            // if text is empty or not started with quote - return empty string
            if (sr.EOF || (char)sr.Lookup() != Quote)
                return "";

            char ch;
            bool escaped = false;
            string text = "";
            sr.Read(); // skip starting quote
            while (!sr.EOF)
            {
                ch = (char)sr.Read();
                if (ch == Quote)
                {
                    escaped = !escaped;
                    if (!escaped)
                        text += ch;
                }
                else // if not quote
                {
                    if (escaped)
                        break;
                    text += ch;
                }
            }

            return text;
        }

        public static string AnsiExtractQuotedStr(string S, char Quote)
        {
            // if text is empty or not started with quote - return empty string
            if (S == null || S == string.Empty)
                return "";
            if (S[0] != Quote)
                return "";

            int i = 1; // skip starting quote
            bool escaped = false;
            string text = "";
            while (i < S.Length)
            {
                if (S[i] == Quote)
                {
                    escaped = !escaped;
                    if (escaped)
                    {
                        text += S[i];
                    }
                }
                else
                {
                    if (escaped)
                        break;
                }
                i++;
            }

            return text;
        }

        public static string AnsiQuotedStr(string S, char Quote)
        {
            int add_count = 0;
            int p = S.IndexOf(Quote);
            while (p >= 0)
            {
                add_count++;
                p = S.IndexOf(Quote, p + 1);
            }
            if (add_count == 0)
                return Quote + S + Quote;

            string text = S;
            p = text.IndexOf(Quote);
            while (p >= 0)
            {
                text = text.Insert(p, "" + Quote);
                p = text.IndexOf(Quote, p + 2);
            }
            return Quote + text + Quote;
        }

        public static string AdjustLineBreaks(string S, string NewNL)
        {
            S = S.Replace("\r\n", "\n");
            S = S.Replace("\r", "\n");
            return S = S.Replace("\n", NewNL);
        }

        public static int AnsiCompareStr(string S1, string S2)
        {
            return CultureInfo.CurrentCulture.CompareInfo.Compare(S1, S2);
        }

        public static int AnsiCompareText(string S1, string S2)
        {
            return CultureInfo.CurrentCulture.CompareInfo.Compare(S1, S2, CompareOptions.IgnoreCase);
        }

        public static bool SameText(string S1, string S2)
        {
            return (AnsiCompareText(S1, S2) == 0);
        }

        // Date encoding and decoding 
        public struct SystemTime
        {
            public int Year;
            public int Month;
            public int DayOfWeek;
            public int Day;
            public int Hour;
            public int Minute;
            public int Second;
            public int Milliseconds;
        }

        public static DateTime EncodeDate(ushort Year, ushort Month, ushort Day)
        {
            return new DateTime(Year, Month, Day);
        }

        public static DateTime EncodeDate(int Year, int Month, int Day, int Hour, int Min, int Sec, int MSec)
        {
            return new DateTime(Year, Month, Day, Hour, Min, Sec, MSec);
        }

        public static TimeSpan EncodeTime(ushort Hour, ushort Min, ushort Sec, ushort MSec)
        {
            return new TimeSpan(0, Hour, Min, Sec, MSec);
        }

        public static void DecodeDate(DateTime date, ref int Year, ref int Month, ref int Day)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        public static void DecodeTime(DateTime date, ref int Hour, ref int Min, ref int Sec, ref int MSec)
        {
            Hour = date.Hour;
            Min = date.Minute;
            Sec = date.Second;
            MSec = date.Millisecond;
        }

        public static void DateTimeToSystemTime(DateTime dateTime, ref SystemTime sysTime)
        {
            sysTime.Year = dateTime.Year;
            sysTime.Month = dateTime.Month;
            sysTime.Day = dateTime.Day;
            sysTime.Hour = dateTime.Hour;
            sysTime.Minute = dateTime.Minute;
            sysTime.Second = dateTime.Second;
            sysTime.Milliseconds = dateTime.Millisecond;
            sysTime.DayOfWeek = (int)dateTime.DayOfWeek;
        }

        public static DateTime SystemTimeToDateTime(SystemTime sysTime)
        {
            return EncodeDate(sysTime.Year, sysTime.Month, sysTime.Day, sysTime.Hour, sysTime.Minute, sysTime.Second, sysTime.Milliseconds);
        }

        public static int WeekOfYear(DateTime dt, bool _1stDayIsMonday)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Calendar calendar = culture.Calendar;
            DayOfWeek dayOfWeek;
            if (_1stDayIsMonday)
                dayOfWeek = DayOfWeek.Monday;
            else
                dayOfWeek = DayOfWeek.Sunday;

            return calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, dayOfWeek) - 1;
        }

        private static int ParseRepeatPattern(string format, int pos, char patternChar)
        {
            int num1 = format.Length;
            int num2 = pos + 1;
            while ((num2 < num1) && (format[num2] == patternChar))
            {
                num2++;
            }
            return (num2 - pos);
        }

        private static bool SetAMPM(DateTime dateTime, ref string format)
        {
            string[] sAMPMs = new string[6];
            int[] AMPMs = new int[6];
            sAMPMs[0] = "AM/PM";
            sAMPMs[1] = "AMPM";
            sAMPMs[2] = "am/pm";
            sAMPMs[3] = "ampm";
            sAMPMs[4] = "A/P";
            sAMPMs[5] = "a/p";

            for (int i = 0; i < sAMPMs.Length; i++)
                AMPMs[i] = format.IndexOf(sAMPMs[i]);

            bool use12hourClock_2U = ((AMPMs[0] >= 0) || (AMPMs[1] >= 0));
            bool use12hourClock_2L = ((AMPMs[2] >= 0) || (AMPMs[3] >= 0));
            bool use12hourClock_1U = (AMPMs[4] >= 0);
            bool use12hourClock_1L = (AMPMs[5] >= 0);

            bool use12hourClock = (use12hourClock_2U || use12hourClock_2L ||
                use12hourClock_1U || use12hourClock_1L);

            if (!use12hourClock)
                return false;

            bool isAfternoon = dateTime.Hour >= 12;
            for (int i = 0; i < sAMPMs.Length; i++)
            {
                if (AMPMs[i] < 0) continue;
                switch (sAMPMs[i])
                {
                    case "AM/PM":
                    case "AMPM":
                        if (isAfternoon)
                            format = format.Replace(sAMPMs[i], "\"PM\"");
                        else
                            format = format.Replace(sAMPMs[i], "\"AM\"");
                        break;

                    case "am/pm":
                        if (isAfternoon)
                            format = format.Replace(sAMPMs[i], "\"pm\"");
                        else
                            format = format.Replace(sAMPMs[i], "\"am\"");
                        break;

                    case "ampm":
                        CultureInfo currCunture = CultureInfo.CurrentCulture;
                        if (isAfternoon)
                            format = format.Replace(sAMPMs[i], String.Format("\"{0}\"", currCunture.DateTimeFormat.PMDesignator));
                        else
                            format = format.Replace(sAMPMs[i], String.Format("\"{0}\"", currCunture.DateTimeFormat.AMDesignator));
                        break;

                    case "A/P":
                        if (isAfternoon)
                            format = format.Replace(sAMPMs[i], "\"P\"");
                        else
                            format = format.Replace(sAMPMs[i], "\"A\"");
                        break;

                    case "a/p":
                        if (isAfternoon)
                            format = format.Replace(sAMPMs[i], "\"p\"");
                        else
                            format = format.Replace(sAMPMs[i], "\"a\"");
                        break;
                }
            }
            return true;
        }

        internal static int ParseQuoteString(string format, int pos)
        {
            int num1 = format.Length;
            int num2 = pos;
            char ch1 = format[pos++];
            bool flag1 = false;
            while (pos < num1)
            {
                char ch2 = format[pos++];
                if (ch2 == ch1)
                {
                    flag1 = true;
                    break;
                }
                if (ch2 == '\\')
                {
                    if (pos < num1)
                        continue;
                    throw new FormatException("Format string is invalid");
                }
            }
            if (!flag1)
            {
                throw new FormatException(string.Format("Format contains bad quote: {0}", ch1));
            }
            return (pos - num2);
        }

        private static string ConvertFormat(string format, DateTime dateTime)
        {
            int num2;
            StringBuilder result = new StringBuilder();
            bool use12hourClock = SetAMPM(dateTime, ref format);
            for (int num1 = 0; num1 < format.Length; num1 += num2)
            {

                char ch1 = format[num1];
                switch (ch1)
                {
                    case '\'':
                    case '"':
                        {
                            num2 = ParseQuoteString(format, num1);
                            result = result.Append(format, num1, num2);
                            break;
                        }
                    case 'c':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        // The time is not displayed if the fractional part of the DateTime value is zero.
                        if (dateTime.TimeOfDay == TimeSpan.Zero)
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.ToShortDateString());
                        else
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.ToString("G"));
                        break;

                    case 'd':
                        {
                            num2 = ParseRepeatPattern(format, num1, ch1);
                            switch (num2)
                            {
                                case 1:
                                    if (format.Length == 1)
                                        // if the 'd' format specifier is used alone, without other custom format strings, 
                                        // it is interpreted as the standard short date pattern format specifier
                                        result = result.Append("%d");
                                    else
                                        result = result.Append(ch1);
                                    break;
                                case 5:
                                    // Displays the date using the format given by the ShortDateFormat global variable.
                                    result = result.AppendFormat(null, "\"{0}\"", dateTime.ToShortDateString());
                                    break;
                                case 6:
                                    // Displays the date using the format given by the LongDateFormat global variable.
                                    result = result.AppendFormat(null, "\"{0}\"", dateTime.ToLongDateString());
                                    break;
                                default:
                                    if (num2 > 6)
                                        result = result.AppendFormat(null, "\"{0}\"", dateTime.ToLongDateString());
                                    else
                                        result = result.Append(format, num1, num2);
                                    break;
                            }
                            break;
                        }

                    case 'm':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (format.Length == 1)
                            result = result.Append("%M");
                        else
                            result = result.Append("".PadRight(num2, 'M'));
                        break;

                    case 'y':
                        {
                            num2 = ParseRepeatPattern(format, num1, ch1);
                            switch (num2)
                            {
                                case 1:
                                    // if the 'y' format specifier is used alone, without other custom format strings,
                                    // it is interpreted as the standard short date pattern format specifier
                                    if (num2 == 1)
                                        result = result.Append(ch1, 2);
                                    else
                                        result = result.Append(ch1);
                                    break;

                                case 2:
                                case 4:
                                    result = result.Append(format, num1, num2);
                                    break;
                            }
                            break;
                        }

                    case 'h':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (format.Length == 1)
                        {
                            if (!use12hourClock)
                                result = result.Append("%H");
                            else
                                result = result.Append("%h");
                        }
                        else
                        {
                            if (!use12hourClock)
                                result = result.Append("".PadRight(num2, 'H'));
                            else
                                result = result.Append(format, num1, num2);
                        }
                        break;

                    case 'n':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (format.Length == 1)
                            // if the 'm' format specifier is used alone, without other custom format strings,
                            // it is interpreted as the standard month day pattern format specifier
                            result = result.Append("%m");
                        else
                            result = result.Append("".PadRight(num2, 'm'));
                        break;

                    case 's':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (format.Length == 1)
                            // if the 's' format specifier is used alone, without other custom format strings,
                            // it is interpreted as the standard sortable date/time pattern format specifier
                            result = result.Append("%s");
                        else
                            result = result.Append(format, num1, num2);
                        break;

                    case 'z':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (num2 == 1)
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.Millisecond.ToString());
                        else
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.Millisecond.ToString().PadLeft(3, '0'));
                        break;

                    case 't':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (num2 == 1)
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.ToShortTimeString());
                        else
                            result = result.AppendFormat(null, "\"{0}\"", dateTime.ToLongTimeString());
                        break;

                    case 'g':
                    case 'G':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        if (format.Length == 1)
                            result = result.Append("%g");
                        else
                            result = result.Append(format, num1, num2);
                        break;

                    case 'f':
                        num2 = ParseRepeatPattern(format, num1, ch1);
                        result = result.AppendFormat(null, "\"{0}\"", format.Substring(num1, num2));
                        break;
                    default:
                        result = result.Append(ch1);
                        num2 = 1;
                        break;
                }
            }
            return result.ToString();
        }


        public static string FormatDateTime(string format, DateTime date)
        {
            if (format == null)
                return date.ToString(format);

            string delphiFormat = ConvertFormat(format, date);

            return date.ToString(delphiFormat);
        }

        #endregion // Standard RTL

        #region Variants
        public static string EMPTY = "(EMPTY)";

        public static int VarType(object AValue)
        {
            if ((AValue is string) && RTL.EMPTY == ((string)AValue))
                return (int)EVariantType.varEmpty;
            else if (AValue == null)
                return (int)EVariantType.varNull;
            else if (AValue is string)
                return (int)EVariantType.varString;
            else if (AValue is bool)
                return (int)EVariantType.varBoolean;
            else if (AValue is byte)
                return (int)EVariantType.varByte;
            else if (AValue is SByte || AValue is Int16 || AValue is UInt16)
                return (int)EVariantType.varSmallint;
            else if (AValue is Int32 || AValue is UInt32)
                return (int)EVariantType.varInteger;
            else if (AValue is float)
                return (int)EVariantType.varSingle;
            else if (AValue is double)
                return (int)EVariantType.varDouble;
            else if (AValue is long)
                return (int)EVariantType.varCurrency;
            else if (AValue is DateTime)
                return (int)EVariantType.varDate;
            else
                throw new RTLError(String.Format(
                    "unable to convert type of value ({0})", XService.AsDump(AValue)));
        }

        public static string VarTypeName(int AVarType)
        {
            string result = null;
            int nType = AVarType & (int)EVariantType.varTypeMask;
            switch ((EVariantType)nType)
            {
                case EVariantType.varEmpty: result = "Empty"; break; //0000;
                case EVariantType.varNull: result = "Null"; break; //0001;
                case EVariantType.varSmallint: result = "Smallint"; break; //0002;
                case EVariantType.varInteger: result = "Integer"; break; //0003;
                case EVariantType.varSingle: result = "Single"; break; //0004;
                case EVariantType.varDouble: result = "Double"; break; //0005;
                case EVariantType.varCurrency: result = "Currency"; break; //0006;
                case EVariantType.varDate: result = "Date"; break; //0007;
                case EVariantType.varOleStr: result = "OleStr"; break; //0008;
                case EVariantType.varDispatch: result = "Dispatch"; break; //0009;
                case EVariantType.varError: result = "Error"; break; //000A;
                case EVariantType.varBoolean: result = "Boolean"; break; //000B;
                case EVariantType.varVariant: result = "Variant"; break; //000C;
                case EVariantType.varUnknown: result = "Unknown"; break; //000D;
                case EVariantType.varByte: result = "Byte"; break; //0011;
                case EVariantType.varStrArg: result = "StrArg"; break; //0048;
                case EVariantType.varString: result = "String"; break; //0100;
                case EVariantType.varAny: result = "Any"; break; //0101;
                default: result = "(unknown)"; break;
            }
            return result;
        }

        public static object VarAsType(object AValue, EVariantType AVarType)
        {
            object result = null;
            switch (AVarType)
            {
                case EVariantType.varEmpty: result = null; break; //0000;
                case EVariantType.varNull: result = null; break; //0001;
                case EVariantType.varSmallint: result = Convert.ToInt16(AValue, null); break; //0002;
                case EVariantType.varInteger: result = Convert.ToInt32(AValue); break; //0003;
                case EVariantType.varSingle: result = Convert.ToSingle(AValue); break; //0004;
                case EVariantType.varDouble: result = Convert.ToDouble(AValue); break; //0005;
                case EVariantType.varCurrency: result = Convert.ToDecimal(AValue); break; //0006;
                case EVariantType.varDate: result = Convert.ToDateTime(AValue); break; //0007;
                case EVariantType.varOleStr: result = AValue.ToString(); break; //0008;
                case EVariantType.varDispatch: result = (object)AValue; break; //0009;
                case EVariantType.varError: result = (Exception)AValue; break; //000A;
                case EVariantType.varBoolean: result = Convert.ToBoolean(AValue); break; //000B;
                case EVariantType.varVariant: result = (object)AValue; break; //000C;
                case EVariantType.varUnknown: result = (object)AValue; break; //000D;
                case EVariantType.varByte: result = Convert.ToByte(AValue, null); break; //0011;
                case EVariantType.varStrArg: result = AValue.ToString(); break; //0048;
                case EVariantType.varString: result = AValue.ToString(); break; //0100;
                case EVariantType.varAny: result = (object)AValue; break; //0101;
                default: result = "(unknown)"; break;
            }
            return result;
        }

        public static bool IsNumericType(object AValue)
        {
            return (AValue is Byte)
                || (AValue is Int16) || (AValue is UInt16)
                || (AValue is Int32) || (AValue is UInt32)
                || (AValue is Int64) || (AValue is UInt64)
                || (AValue is float)
                || (AValue is Double);
        }

        public static bool VarIsEmpty(object V)
        {
            int varType = VarType(V);
            object varAsType = VarAsType(V, EVariantType.varDispatch);
            return (varType == (int)EVariantType.varEmpty) ||
                ((varType == (int)EVariantType.varDispatch) || (varType == (int)EVariantType.varUnknown)) &&
                (varAsType == null);
        }

        public static string VarAsDump(object V)
        {
            int t = VarType(V);
            string result = VarTypeName(t);
            try
            {
                V = VarAsType(V, EVariantType.varString);
                switch (t)
                {
                    case (int)EVariantType.varOleStr:
                    case (int)EVariantType.varString:
                    case (int)EVariantType.varStrArg: V = "\"" + V + "\"";
                        break;
                }
                result = V + ", type is " + result;
            }
            catch (Exception e)
            {
                result = result + String.Format("AccessError({0}): {1}", e.GetType().ToString(), e.Message);
            }
            return result;
        }
        #endregion // Variants
    }
    // Delphi5RTL
}

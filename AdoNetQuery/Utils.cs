/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * Application utils.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using XService.Utils;

namespace AdoNetQuery
{
    class ListViewItemComparer : IComparer
    {
        public ListView List { get; set; }
        public int ColumnNo { get; set; }
        public bool RevertSorting { get; set; }
        
        public ListViewItemComparer()
        {
            this.List = null;
            this.ColumnNo = 0;
            this.RevertSorting = false;
        }

        public ListViewItemComparer(ListView pList, int pColumn)
        {
            this.List = pList;
            this.ColumnNo = pColumn;
            this.RevertSorting = false;
        }

        public int Compare(object x, object y)
        {
            ListViewItem liX = (ListViewItem)x;
            ListViewItem liY = (ListViewItem)y;
            int result = String.Compare(liX.SubItems[this.ColumnNo].Text, liY.SubItems[this.ColumnNo].Text);
            if (this.RevertSorting)
                result *= -1;
            return result;
        }
    }

	public class AppUtils
	{
		public static Type StrToType(string pTypeName, Type pDefaultType)
		{
			Type t = pDefaultType;
			string tn = pTypeName.ToLower();
			if (tn.StartsWith("system.")) tn = tn.Remove(0, "system.".Length);
			switch (tn)
			{
				case "bool": t = typeof(bool); break;
				case "boolean": t = typeof(bool); break;
				case "byte": t = typeof(byte); break;
				case "short": t = typeof(short); break;
				case "ushort": t = typeof(ushort); break;
				case "int16": t = typeof(short); break;
				case "uint16": t = typeof(ushort); break;
				case "int": t = typeof(int); break;
				case "uint": t = typeof(uint); break;
				case "int32": t = typeof(int); break;
				case "uint32": t = typeof(uint); break;
				case "long": t = typeof(long); break;
				case "ulong": t = typeof(ulong); break;
				case "int64": t = typeof(long); break;
				case "uint64": t = typeof(ulong); break;
				case "float": t = typeof(float); break;
				case "double": t = typeof(double); break;
				case "string": t = typeof(string); break;
			}
			return t;
		}

		public static string FindPrmLine(string pSourcePrmText, string pPrmKeyPrefixToSearch)
		{ 
			pSourcePrmText = pSourcePrmText.Replace("\r\n", "\n").Replace('\r', '\n');
			string[] lines = pSourcePrmText.Split('\n');
			foreach (string line in lines)
			{
				string s = line.Trim(StrUtils.CH_SPACES);
				if (string.IsNullOrEmpty(s)) continue;
				if (s.ToLower().StartsWith(pPrmKeyPrefixToSearch))
					return s;
			}
			return null;
		}
	}
}

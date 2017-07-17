using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Widgets
{
    public class GameTrayHelper
    {
        public static string ConvertUintToString(uint text)
        {
            char[] chars = System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(text));
            StringBuilder sb = new StringBuilder();
            foreach (char c in chars)
            {
                if (c != '\0')
                    sb.Append(c);
            }
            string str = sb.ToString();
            return str;
        }
    }
}

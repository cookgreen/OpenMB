using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Core
{
    public class GameVersion
    {
        private static int main = 0;
        private static int sub = 0;
        private static int modify = 0;

        public static string Current
        {
            get
            {
                return string.Format("{0}.{1}.{2}", main, sub, modify);
            }
        }
    }
}

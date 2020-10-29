using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Utilities.UCSEditor
{
    public class UCSLine
    {
        public string ID { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return ID + "\t" + Text;
        }
    }
}

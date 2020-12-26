using KBFEditor.FileFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBFEditor.Loader
{
    public class KBFLoader
    {
        public KBFLoader()
        {

        }

        public KBF Read(Stream stream)
        {
            KBF kbf = new KBF();
            kbf.Read(stream);
            return kbf;
        }


        public void Write(KBF kbf, Stream stream)
        {
            kbf.Write(stream);
        }
    }
}

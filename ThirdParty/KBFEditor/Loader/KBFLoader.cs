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
            KBF kbf = new KBF((stream as FileStream).Name);
            kbf.Read(stream);
            return kbf;
        }


        public void Write(KBF kbf)
        {
            FileStream stream = new FileStream(kbf.FullFileName, FileMode.Open, FileAccess.Write);
            kbf.Write(stream);
            stream.Close();
        }
    }
}

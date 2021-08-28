using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KBFEditor.FileFormat
{
    public class KBFInvalidFileFormatException : Exception
    {
        public KBFInvalidFileFormatException() : base() { }

        public KBFInvalidFileFormatException(string message) : base(message) { }

        protected KBFInvalidFileFormatException(SerializationInfo info,
                                                StreamingContext context) : base(info, context)
        { }
    }
}

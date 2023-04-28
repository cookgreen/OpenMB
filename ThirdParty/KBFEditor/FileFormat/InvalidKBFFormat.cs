using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KBFEditor.FileFormat
{
    public class InvalidKBFFormat : Exception
    {
        public InvalidKBFFormat() : base() { }

        public InvalidKBFFormat(string message) : base(message) { }

        protected InvalidKBFFormat(SerializationInfo info,
                                                StreamingContext context) : base(info, context)
        { }
    }
}

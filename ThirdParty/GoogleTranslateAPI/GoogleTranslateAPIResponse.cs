using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.google.translate.api
{
    public class GoogleTranslateAPIResponse
    {
        public List<GoogleTranslateAPIResponseSentence> sentences { get; set; }
        public string src { get; set; }
        public int id_confidence { get; set; }
    }

    public class GoogleTranslateAPIResponseSentence
    {
        public string trans { get; set; }
        public string orig { get; set; }
        public int backend { get; set; } 
    }


}

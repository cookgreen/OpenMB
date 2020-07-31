using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Modio
{
	public class ImageFile
	{
		public string filename { get; set; }
		public string original { get; set; }
		public string thumb_50x50 { get; set; }
		public string thumb_100x100 { get; set; }
	}
}

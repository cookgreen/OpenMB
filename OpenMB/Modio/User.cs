using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Modio
{
	public class User
	{
		public int id { get; set; }
		public string name_id { get; set; }
		public string user_name { get; set; }
		public int date_online { get; set; }
		public ImageFile avatar { get; set; }
		public string timezone { get; set; }
		public string language { get; set; }
		public string profile_url { get; set; }
	}
}

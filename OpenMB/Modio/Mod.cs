using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Modio
{
	public class Mod
	{
		public string id { get; set; }
		public string game_id { get; set; }
		public string status { get; set; }
		public string visible { get; set; }
		public User submitted_by { get; set; }
		public int date_added { get; set; }
		public int date_updated { get; set; }
		public int date_live { get; set; }
		public int maturity_option { get; set; }
		public ImageFile logo { get; set; }
		public string homepage_url { get; set; }
		public string name { get; set; }
		public string name_id { get; set; }
		public string summary { get; set; }
		public string description { get; set; }
		public string description_plaintext { get; set; }
		public string metadata_blob { get; set; }
		public string profile_url { get; set; }
		public Media media { get; set; }
		public ModFile modfile { get; set; }
		public List<string> metadata_kvp { get; set; }
		public List<string> tags { get; set; }
		public Stats modstats { get; set; }
	}
}

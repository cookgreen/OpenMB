using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modio
{
	public class ModioStats
	{
		public int mod_id { get; set; }
		public int popularity_rank_position { get; set; }
		public int popularity_rank_total_mods { get; set; }
		public string downloads_total { get; set; }
		public string subscribers_total { get; set; }
		public string ratings_total { get; set; }
		public string ratings_positive { get; set; }
		public string ratings_negative { get; set; }
		public string ratings_percentage_positive { get; set; }
		public string ratings_weighted_aggregate { get; set; }
		public string ratings_display_text { get; set; }
		public int date_expires { get; set; }
	}
}

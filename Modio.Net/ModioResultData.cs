using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.mod
{
	public class ModioResultData
	{
		public object data { get; set; }
		public int result_count { get; set; }
		public int result_offset { get; set; }
		public int result_limit { get; set; }
		public int result_total { get; set; }
	}
}

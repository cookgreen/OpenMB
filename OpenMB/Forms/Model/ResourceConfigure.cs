using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenMB.Forms.Model
{
	public class ResourceConfigure
	{
		private BindingList<string> fileSystemResources;
		private BindingList<string> zipResources;

		public BindingList<string> FileSystemResources
		{
			get
			{
				return fileSystemResources;
			}

			set
			{
				fileSystemResources = value;
			}
		}
		public BindingList<string> ZipResources
		{
			get
			{
				return zipResources;
			}

			set
			{
				zipResources = value;
			}
		}

		public string ResourceRootDir { get; set; }

		public ResourceConfigure()
		{
			fileSystemResources = new BindingList<string>();
			zipResources = new BindingList<string>();
		}
	}
}

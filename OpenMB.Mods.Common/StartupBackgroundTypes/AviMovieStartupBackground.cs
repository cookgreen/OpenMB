using Mogre;
using OpenMB.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.StartupBackgroundTypes
{
	public class AviMovieStartupBackground : IModStartupBackgroundType
	{
		public string Name { get { return "AviMovie"; } }

		public void StartBackground(string value, params object[] param)
		{
            SceneManager scm = param[0] as SceneManager;
            VideoTextureManager.Instance.CreateVideoTexture(scm, 1024, 1024, value);
		}
	}
}

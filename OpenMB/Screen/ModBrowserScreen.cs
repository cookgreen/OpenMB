using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Screen
{
	public class ModBrowserScreen : Screen
	{
		public override event Action<string, string> OnScreenEventChanged;
		public override event Action OnScreenExit;
		public override string Name
		{
			get { return "ModBrowser"; }
		}
		public ModBrowserScreen()
		{
		}

		public override void Run()
		{
			base.Run();
		}
	}
}

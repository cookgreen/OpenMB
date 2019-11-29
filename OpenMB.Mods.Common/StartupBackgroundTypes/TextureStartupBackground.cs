using Mogre;
using OpenMB.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.StartupBackgroundTypes
{
	public class TextureStartupBackground : IModStartupBackgroundType
	{
		public string Name { get { return "2DTexture"; } }

		public void StartBackground(string value, params object[] param)
		{
			var sceneManager = param[0] as SceneManager;
			MaterialManager.Singleton.Remove("Background");

			MaterialPtr material = MaterialManager.Singleton.Create("Background", "General");
			material.GetTechnique(0).GetPass(0).CreateTextureUnitState(value);
			material.GetTechnique(0).GetPass(0).DepthCheckEnabled = false;
			material.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
			material.GetTechnique(0).GetPass(0).LightingEnabled = false;

			Rectangle2D rect = new Rectangle2D(true);
			rect.SetCorners(-1.0f, 1.0f, 1.0f, -1.0f);
			rect.SetMaterial("Background");

			rect.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_BACKGROUND;

			AxisAlignedBox aab = new AxisAlignedBox();
			aab.SetInfinite();
			rect.BoundingBox = aab;

			SceneNode node = sceneManager.RootSceneNode.CreateChildSceneNode(Guid.NewGuid().ToString());
			node.AttachObject(rect);
		}
	}
}

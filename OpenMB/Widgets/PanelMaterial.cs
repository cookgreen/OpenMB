using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{
	public class PanelMaterial : PanelTemplate
	{
		private MaterialPtr materialPtr;
		public PanelMaterial(string name, string texture, float width = 0, float height = 0, float left = 0, float top = 0) : base(name, "MeshPanel", width, height, left, top)
		{
			string matName = texture.Substring(0, texture.Length - texture.IndexOf('.'));
			materialPtr = MaterialManager.Singleton.Create(matName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			materialPtr.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
			materialPtr.GetTechnique(0).GetPass(0).CreateTextureUnitState().SetTextureName(texture);

			mElement.MaterialName = matName;
		}
		public PanelMaterial(string name, MaterialPtr material, float width = 0, float height = 0, float left = 0, float top = 0) : base(name, "MeshPanel", width, height, left, top)
		{
			materialPtr = material;

			mElement.MaterialName = material.Name;
		}

		public override void Dispose()
		{
			MaterialManager.Singleton.Remove(materialPtr.Name);
			materialPtr.Dispose();
		}
	}
}

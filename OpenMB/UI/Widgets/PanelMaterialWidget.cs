using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	public class PanelMaterialWidget : PanelTemplateWidget
	{
		private MaterialPtr materialPtr;
		public PanelMaterialWidget(string name, string texture, float width = 0, float height = 0, float left = 0, float top = 0) : base(name, "MeshPanel", width, height, left, top)
		{
			string matName = null;
			if (!string.IsNullOrEmpty(texture))
			{
				matName = texture.Substring(0, texture.Length - texture.IndexOf('.'));
			}
			else
			{
				matName = "NoMaterial";
			}
			materialPtr = MaterialManager.Singleton.Create(matName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			materialPtr.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
			materialPtr.GetTechnique(0).GetPass(0).CreateTextureUnitState().SetTextureName(texture);

			element.MaterialName = matName;
		}
		public PanelMaterialWidget(string name, MaterialPtr material, float width = 0, float height = 0, float left = 0, float top = 0) : base(name, "MeshPanel", width, height, left, top)
		{
			materialPtr = material;

			element.MaterialName = material.Name;
		}

		public override void Dispose()
		{
			MaterialManager.Singleton.Remove(materialPtr.Name);
			materialPtr.Dispose();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMB.FileFormats;
using SharpGL;
using SharpGL.SceneGraph.Assets;

namespace OpenMB.Render
{
	public class MBBrfMeshRenderable : IOpenGLRenderable
	{
		private MBBrfMesh brfMesh;
		private Texture texture;

		public MBBrfMeshRenderable(MBBrfMesh brfMesh)
		{
			this.brfMesh = brfMesh;
			texture = new Texture();
		}

		public void Render(OpenGL gl)
		{
			//Brf mesh render logic
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AviFile;

namespace OpenMB.Video
{
	public class VideoTexture : IDisposable
	{
		public HardwarePixelBufferSharedPtr PixelBuffer;
		public VideoStream Stream;
		public int FrameNum;
		private ManualObject screen;
		private TexturePtr videoTex;
		private MaterialPtr videoMat;

		public VideoTexture(
			SceneManager scm, 
			string internalName, 
			float width, float height, 
			string aviFileName, 
			SceneNode parentNode)
		{
			AviManager aviMgr = new AviManager(aviFileName, true);
			Stream = aviMgr.GetVideoStream();

			videoTex = TextureManager.Singleton.CreateManual(
                internalName,
				ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
				TextureType.TEX_TYPE_2D, Convert.ToUInt32(Stream.Width), Convert.ToUInt32(Stream.Height), 0, PixelFormat.PF_R8G8B8A8, (int)TextureUsage.TU_DYNAMIC_WRITE_ONLY_DISCARDABLE);
			videoMat = MaterialManager.Singleton.Create(
                internalName+"Mat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			videoMat.GetTechnique(0).GetPass(0).LightingEnabled = false;
			videoMat.GetTechnique(0).GetPass(0).CreateTextureUnitState("Video");

			PixelBuffer = videoTex.GetBuffer();

			screen = scm.CreateManualObject("Screen");
			screen.Dynamic = true;
			screen.Begin("VideoMat", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

			screen.Position(0, 0, 0);
			screen.TextureCoord(0, 0);

			screen.Position(width, 0, 0);
			screen.TextureCoord(1, 0);

			screen.Position(width, height, 0);
			screen.TextureCoord(1, 1);

			screen.Triangle(0, 1, 2);

			screen.Position(0, 0, 0);
			screen.TextureCoord(0, 0);

			screen.Position(width, height, 0);
			screen.TextureCoord(1, 1);

			screen.Position(0, height, 0);
			screen.TextureCoord(0, 1);

			screen.Triangle(3, 4, 5);

			screen.End();

			SceneNode node = parentNode.CreateChildSceneNode();
			node.Position = new Vector3(0, 0, 0);
			node.AttachObject(screen);

			Stream.GetFrameOpen();
			FrameNum = 0;
		}

		public void Dispose()
		{
			Stream.GetFrameClose();
			PixelBuffer.Dispose();
			screen.Dispose();
			videoMat.Dispose();
			videoTex.Dispose();

			videoTex = null;
			videoMat = null;
			screen = null;
			PixelBuffer = null;
			FrameNum = 0;
		}
	}
}

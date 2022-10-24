using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.IO;

namespace OpenMB.Video
{
	public class VideoTextureManager
	{
		private List<VideoTexture> videotexes;
		private static VideoTextureManager instance;
		public static VideoTextureManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new VideoTextureManager();
				}
				return instance;
			}
		}
		public VideoTextureManager()
		{
			videotexes = new List<VideoTexture>();
		}
		public void CreateVideoTexture(SceneManager scm, float width, float height, string aviFileName, SceneNode parentNode)
		{
			var videotex = new VideoTexture(
				scm, "Video-" + Guid.NewGuid().ToString(), 
				width, height, aviFileName,
                parentNode);
			videotexes.Add(videotex);
		}

		public void DestroyVideoTexture(VideoTexture vt)
		{
			vt.Dispose();
			videotexes.Remove(vt);
		}

		public void Update(float timeSinceLastFrame)
		{
			foreach (var videotex in videotexes)
			{
				if (videotex.FrameNum >= videotex.Stream.CountFrames)
				{
					videotex.FrameNum = 0;
				}
				System.Drawing.Bitmap bitmap = videotex.Stream.GetBitmap(videotex.FrameNum);
				MemoryStream ms = new MemoryStream();
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;
				try
				{
					Image image = new Image();
					image.Load(Utilities.Helper.StreamToDataPtr(ms));
					image.FlipAroundX();
					videotex.PixelBuffer.BlitFromMemory(image.GetPixelBox());
					image.Dispose();
					ms.Close();
					videotex.FrameNum++;
				}
				catch (Exception ex)
				{
					EngineManager.Instance.log.LogMessage("[Engine Warning]: Image Data Exception. Detals:" + ex.ToString());
				}
				finally
				{
					videotex.FrameNum++;
				}
			}
		}
	}
}

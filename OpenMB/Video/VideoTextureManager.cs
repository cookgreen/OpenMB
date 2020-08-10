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
		private VideoTexture videotex;
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
			videotex = null;
		}
		public void CreateVideoTexture(SceneManager scm, float width, float height, string aviFileName)
		{
			if (videotex != null)
			{
				videotex.Dispose();
			}
			videotex = new VideoTexture(scm, width, height, aviFileName);
		}

		public void DestroyVideoTexture()
		{
			videotex.Dispose();
			videotex = null;
		}

		public void Update(float timeSinceLastFrame)
		{
			if (videotex == null)
			{
				return;
			}
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
				GameManager.Instance.log.LogMessage("[Engine Warning]: Image Data Exception. Detals:" + ex.ToString());
			}
			finally
			{
				videotex.FrameNum++;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Video
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
        public void CreateVideoTexture(SceneManager scm,float width, float height,string aviFileName)
        {
            //if (videotex == null)
            //{
                videotex = new VideoTexture(scm, width, height, aviFileName);
            //}
        }

        public void DestroyVideoTexture()
        {
            videotex.Dispose();
            videotex = null;
        }

        public void Update(float timeSinceLastFrame)
        {
            if (videotex.FrameNum >= videotex.Stream.CountFrames)
            {
                videotex.FrameNum = 0;
            }
            System.Drawing.Bitmap bitmap = videotex.Stream.GetBitmap(videotex.FrameNum);
            bitmap.Save("./Media/materials/textures/frame.png");
            try
            {
                Image image = new Image();
                image.Load("frame.png", "General");
                image.FlipAroundX();
                videotex.PixelBuffer.BlitFromMemory(image.GetPixelBox());
                image.Dispose();

                videotex.FrameNum++;
            }
            catch (Exception ex)
            {
                GameManager.Instance.mLog.LogMessage("[Engine Warning]: Image Data Exception. Detals:" + ex.ToString());
            }
            finally
            {
                videotex.FrameNum++;
            }
        }
    }
}

using Mogre;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Render
{
	public static class OpenGLRenderManager
	{
		private static OpenGL gl;
		private static SceneManager sceneMgr;
		private static LinkedList<IOpenGLRenderable> glRenderableObjects;
		public static bool AfterQueue
		{
			get;
			set;
		}
		public static RenderQueueGroupID TargetQueue
		{
			get;
			set;
		}
		public static void Initization(SceneManager sceneManager)
		{
			sceneMgr = sceneManager;
			sceneMgr.RenderQueueStarted += SceneManager_RenderQueueStarted;
			sceneMgr.RenderQueueEnded += SceneManager_RenderQueueEnded;
			TargetQueue = RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
			AfterQueue = false;
			glRenderableObjects = new LinkedList<IOpenGLRenderable>();
			gl = new OpenGL();
		}

		private static void SceneManager_RenderQueueEnded(byte queueGroupId, string invocation, out bool repeatThisInvocation)
		{
			repeatThisInvocation = false; // shut up compiler
			if (AfterQueue && queueGroupId == (byte)TargetQueue)
			{
				Render();
			}
		}

		private static void SceneManager_RenderQueueStarted(byte queueGroupId, string invocation, out bool skipThisInvocation)
		{
			skipThisInvocation = false; // shut up compiler
			if (!AfterQueue && queueGroupId == (byte)TargetQueue)
			{
				Render();
			}
		}

		private static void Render()
		{
			foreach (var glRenderableObject in glRenderableObjects)
			{
				glRenderableObject.Render(gl);
			}
		}

		public static void EnqueueObjects(IOpenGLRenderable glRenderableObject)
		{
			if (glRenderableObjects.Contains(glRenderableObject))
			{
				return;
			}
			glRenderableObjects.AddLast(glRenderableObject);
		}

		public static void Shutdown()
		{
			sceneMgr.RenderQueueStarted -= SceneManager_RenderQueueStarted;
			sceneMgr.RenderQueueEnded -= SceneManager_RenderQueueEnded;
		}
	}
}

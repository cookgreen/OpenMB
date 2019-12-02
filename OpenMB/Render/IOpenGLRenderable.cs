using SharpGL;

namespace OpenMB.Render
{
	public interface IOpenGLRenderable
	{
		void Render(OpenGL gl);
	}
}
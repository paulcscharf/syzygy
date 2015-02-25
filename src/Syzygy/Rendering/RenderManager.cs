using Bearded.Utilities;
using OpenTK.Graphics.OpenGL;

namespace Syzygy.Rendering
{
    sealed class RenderManager : Singleton<RenderManager>
    {
        private readonly ShaderManager shaders;
        private readonly SurfaceManager surfaces;

        public RenderManager()
        {
            this.shaders = new ShaderManager();
            this.surfaces = new SurfaceManager();
        }

        public void PrepareFrame()
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(GameState game)
        {
            game.Draw();

            this.surfaces.Primitives.Render();
        }

        public void FinaliseFrame()
        {
            
        }
    }
}

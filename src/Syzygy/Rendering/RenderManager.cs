using Bearded.Utilities;
using OpenTK.Graphics.OpenGL;
using Syzygy.Game;

namespace Syzygy.Rendering
{
    sealed class RenderManager : Singleton<RenderManager>
    {
        private readonly ShaderManager shaders;
        private readonly SurfaceManager surfaces;
        private readonly GeometryManager geometries;

        public RenderManager()
        {
            this.shaders = new ShaderManager();
            this.surfaces = new SurfaceManager();
            this.geometries = new GeometryManager();
        }


        public void Resize(int width, int height)
        {
            this.surfaces.Resize(width, height);
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

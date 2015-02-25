using amulware.Graphics;
using Bearded.Utilities;

namespace Syzygy.Rendering
{
    sealed class ShaderManager : Singleton<ShaderManager>
    {
        public ISurfaceShader Primitives { get; private set; }

        public ShaderManager()
        {
            this.Primitives = ShaderProgram.FromFiles("data/shaders/primitives.vs", "data/shaders/primitives.fs");
        }
    }
}

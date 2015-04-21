using amulware.Graphics;
using amulware.Graphics.ShaderManagement;
using Bearded.Utilities;

namespace Syzygy.Rendering
{
    sealed class ShaderManager : Singleton<ShaderManager>
    {
        public ISurfaceShader Primitives { get; private set; }
        public ISurfaceShader UVColor { get; private set; }

        public ShaderManager()
        {
            var loader = ShaderFileLoader.CreateDefault("data/shaders");
            var man = new amulware.Graphics.ShaderManagement.ShaderManager();

            man.Add(loader.Load(""));

            this.Primitives = man.MakeShaderProgram("primitives");

            this.UVColor = man.MakeShaderProgram("uvcolor");

        }
    }
}

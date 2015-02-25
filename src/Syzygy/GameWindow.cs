using System;
using amulware.Graphics;
using Bearded.Utilities.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Syzygy.Rendering;

namespace Syzygy
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private GameState gameState;
        private RenderManager renderer;

        public GameWindow()
            :base(1290, 720, GraphicsMode.Default, "Syzygy",
            GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
            this.gameState = new GameState();
        }

        protected override void OnLoad(EventArgs e)
        {
            InputManager.Initialize(this.Mouse);

            this.renderer = new RenderManager();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);

            this.renderer.Resize(this.Width, this.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            this.gameState.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            this.renderer.PrepareFrame();

            this.renderer.Render(this.gameState);

            this.renderer.FinaliseFrame();

            this.SwapBuffers();
        }
    }
}

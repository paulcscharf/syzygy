using System;
using amulware.Graphics;
using Bearded.Utilities.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Syzygy
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private GameState gameState;

        public GameWindow()
            :base(1290, 720, GraphicsMode.Default, "Syzygy",
            GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
            this.gameState = new GameState();
        }

        protected override void OnLoad(EventArgs e)
        {
            InputManager.Initialize(this.Mouse);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            this.gameState.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            this.gameState.Draw();

            this.SwapBuffers();
        }
    }
}

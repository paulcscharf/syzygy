using System;
using amulware.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Syzygy
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        public GameWindow()
            :base(1290, 720, GraphicsMode.Default, "Syzygy",
            GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
            
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {

        }

        protected override void OnRender(UpdateEventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);


            this.SwapBuffers();
        }
    }
}

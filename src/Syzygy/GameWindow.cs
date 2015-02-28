using System;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Bearded.Utilities.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Syzygy.Game;
using Syzygy.GameManagement;
using Syzygy.Rendering;

namespace Syzygy
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private RenderManager renderer;

        private ManualActionQueue glQueue = new ManualActionQueue();

        private bool resized;
        private GameManager game;

        public GameWindow()
            :base(1290, 720, GraphicsMode.Default, "Syzygy",
            GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            InputManager.Initialize(this.Mouse);

            this.renderer = new RenderManager();

            this.game = new GameManager(this);
        }

        protected override void OnResize(EventArgs e)
        {
            this.glQueue.RunAndForget(
                () => this.resized = true
                );
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            this.game.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            this.glQueue.ExecuteFor(TimeSpan.FromMilliseconds(5));
            if (this.resized)
            {
                GL.Viewport(0, 0, this.Width, this.Height);
                this.renderer.Resize(this.Width, this.Height);
                this.resized = false;
            }

            this.renderer.PrepareFrame();


            this.game.Render(this.renderer);


            this.renderer.FinaliseFrame();

            this.SwapBuffers();
        }
    }
}

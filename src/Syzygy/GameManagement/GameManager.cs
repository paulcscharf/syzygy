using System;
using amulware.Graphics;
using Syzygy.Rendering;

namespace Syzygy.GameManagement
{
    sealed class GameManager
    {
        private readonly GameWindow gameWindow;
        private IGameHandler gameHandler;

        public GameManager(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
            this.setGameHandler(new ConnectionGameHandler(gameWindow));
        }

        private void setGameHandler(IGameHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (this.gameHandler != null)
                this.gameHandler.Stopped -= this.setGameHandler;

            this.gameHandler = handler;
            this.gameHandler.Stopped += this.setGameHandler;
        }

        public void Update(UpdateEventArgs e)
        {
            this.gameHandler.Update(e);
        }

        public void Render(RenderManager renderMan)
        {
            
        }
    }
}

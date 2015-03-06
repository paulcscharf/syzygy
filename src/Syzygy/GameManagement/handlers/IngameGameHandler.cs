using amulware.Graphics;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering;

namespace Syzygy.GameManagement
{
    sealed class IngameGameHandler : GenericGameHandler<NetPeer>, IGameDrawer
    {
        private readonly GameState game;
        private readonly PlayerList players;

        public IngameGameHandler(NetPeer peer, GameState game, PlayerList players)
            : base(peer)
        {
            this.game = game;
            this.players = players;
        }

        public override void Update(UpdateEventArgs e)
        {
            base.Update(e);

            this.game.Update(e);
        }

        public void Render(RenderManager renderMan)
        {
            renderMan.Render(this.game);
        }
    }
}

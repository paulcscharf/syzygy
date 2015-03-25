using amulware.Graphics;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering;

namespace Syzygy.GameManagement
{
    abstract class GenericIngameGameHandler<TPeer> : GenericGameHandler<TPeer>, IGameDrawer
        where TPeer : NetPeer
    {
        protected readonly GameState game;
        protected readonly PlayerLookup players;

        public GenericIngameGameHandler(TPeer peer, GameState game, PlayerLookup players)
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

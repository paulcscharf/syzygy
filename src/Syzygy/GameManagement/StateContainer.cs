using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering.Game;

namespace Syzygy.GameManagement
{
    abstract class StateContainer<TPeer> : IStateContainer<TPeer>
        where TPeer : NetPeer
    {
		private readonly TPeer peer;
		private readonly GameState game;
		private readonly PlayerLookup players;
		private readonly IGameView view;

        public TPeer Peer { get { return this.peer; } }
        public GameState Game { get { return this.game; } }
        public PlayerLookup Players { get { return this.players; } }
        public IGameView View { get { return this.view; } }

        protected StateContainer(TPeer peer, GameState game, PlayerLookup players, IGameView view)
        {
            this.view = view;
            this.players = players;
            this.game = game;
            this.peer = peer;
        }

        public interface IBuilder
        {
            GameState Game { set; }
            IGameView View { set; }
        }
    }
}

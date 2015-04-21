using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering.Game;

namespace Syzygy.GameManagement.Server
{
    sealed class StateContainer : StateContainer<NetServer>
    {
        private readonly PlayerConnectionLookup connections;

        public PlayerConnectionLookup Connections { get { return this.connections; } }

        private StateContainer(NetServer peer,
            GameState game, PlayerLookup players, IGameView view,
            PlayerConnectionLookup connections)
            : base(peer, game, players, view)
        {
            this.connections = connections;
        }

        public class Builder : IBuilder
        {
            public NetServer Server { get; set; }
            public GameState Game { get; set; }
            public PlayerLookup Players { get; set; }
            public IGameView View { get; set; }
            public PlayerConnectionLookup Connections { get; set; }

            public StateContainer Build()
            {
                return new StateContainer(this.Server, this.Game, this.Players, this.View, this.Connections);
            }

        }
    }
}

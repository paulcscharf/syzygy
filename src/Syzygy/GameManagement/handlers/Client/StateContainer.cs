using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Rendering.Game;

namespace Syzygy.GameManagement.Client
{
    sealed class StateContainer : StateContainer<NetClient>
    {
        private StateContainer(NetClient peer, GameState game, PlayerLookup players, IGameView view)
            : base(peer, game, players, view)
        {
        }

        public class Builder : IBuilder
        {
            public NetClient Client { get; set; }
            public GameState Game { get; set; }
            public PlayerLookup Players { get; set; }
            public IGameView View { get; set; }

            public StateContainer Build()
            {
                return new StateContainer(this.Client, this.Game, this.Players, this.View);
            }
        }
    }
}

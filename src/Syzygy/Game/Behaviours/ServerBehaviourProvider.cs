using Lidgren.Network;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerBehaviourProvider : IGameBehaviourProvider
    {
        private readonly NetServer server;
        private readonly PlayerConnectionLookup connections;

        public ServerBehaviourProvider(NetServer server, PlayerConnectionLookup connections)
        {
            this.server = server;
            this.connections = connections;
        }

        public ICollisionHandler GetCollisionHandler(GameState game)
        {
            return new ServerCollisionHandler(game);
        }

        public IRequestHandler GetRequestHandler()
        {
            return new ServerRequestHandler(this.server, this.connections);
        }
    }
}

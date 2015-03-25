using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerBehaviourProvider : IGameBehaviourProvider
    {
        private readonly NetServer server;
        private readonly PlayerConnectionLookup connections;
        private readonly ServerCommandSender commandSender;

        public ServerBehaviourProvider(NetServer server, PlayerConnectionLookup connections)
        {
            this.server = server;
            this.connections = connections;
            this.commandSender = new ServerCommandSender(server, connections);
        }

        public ICollisionHandler GetCollisionHandler(GameState game)
        {
            return new ServerCollisionHandler(game, this.commandSender);
        }

        public IRequestHandler GetRequestHandler()
        {
            return new ServerRequestHandler(this.commandSender);
        }

        public IContinuousSynchronizer GetContinuousSynchronizer(GameState game)
        {
            return new ServerContinuousSynchronizer(game, this.server, this.connections);
        }
    }
}

using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerBehaviourProvider : IGameBehaviourProvider
    {
        private readonly ServerCommandSender commandSender;

        public ServerBehaviourProvider(NetServer server, PlayerConnectionLookup connections)
        {
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
    }
}

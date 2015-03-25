using Lidgren.Network;
using Syzygy.Game.SyncedCommands;
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
            return new ServerRequestHandler(this.server, this.connections);
        }
    }

    class ServerCommandSender
    {
        private readonly NetServer server;
        private readonly PlayerConnectionLookup connections;

        public ServerCommandSender(NetServer server, PlayerConnectionLookup connections)
        {
            this.server = server;
            this.connections = connections;
        }

        public void ExecuteAndSend(ICommand command)
        {
            command.Execute();

            var message = this.server.CreateMessage();
            message.Write((byte)IngameMessageType.Command);
            command.WriteToBuffer(message);

            this.server.SendMessage(message, this.connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}

using Lidgren.Network;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
{
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

            if (this.connections.Count == 0)
                return;

            var message = this.server.CreateMessage();
            message.Write((byte)IngameMessageType.Command);
            command.WriteToBuffer(message);

            this.server.SendMessage(message, this.connections, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}

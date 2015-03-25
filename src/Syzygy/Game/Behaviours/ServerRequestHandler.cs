using System;
using Lidgren.Network;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
{
    sealed class ServerRequestHandler : IRequestHandler
    {
        private readonly NetServer server;
        private readonly PlayerConnectionLookup connections;

        public ServerRequestHandler(NetServer server, PlayerConnectionLookup connections)
        {
            this.server = server;
            this.connections = connections;
        }

        public void TryDo(IRequest request)
        {
            if (!request.CheckPreconditions())
            {
                Console.WriteLine("Invalid request received and disregarded.");
                return;
            }

            var command = request.MakeCommand();

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

using Lidgren.Network;
using Syzygy.GameManagement;

namespace Syzygy.Game.SyncedCommands
{
    sealed class ClientRequestHandler : IRequestHandler
    {
        private readonly NetClient client;

        public ClientRequestHandler(NetClient client)
        {
            this.client = client;
        }

        public void TryDo(IRequest request)
        {
            var message = client.CreateMessage();
            message.Write((byte)IngameMessageType.Request);
            request.WriteToBuffer(message);

            this.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}

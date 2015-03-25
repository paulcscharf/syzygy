using Lidgren.Network;
using Syzygy.Game.SyncedCommands;
using Syzygy.GameManagement;

namespace Syzygy.Game.Behaviours
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
            var message = this.client.CreateMessage();
            message.Write((byte)IngameMessageType.Request);
            request.WriteToBuffer(message);

            this.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}

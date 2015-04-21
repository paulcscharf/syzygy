using System;
using Lidgren.Network;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.GameManagement.Server
{
    sealed class IngameGameHandler : GenericIngameGameHandler<NetServer>
    {
        private readonly RequestReader requestReader;

        public IngameGameHandler(StateContainer state)
            : base(state)
        {
            this.requestReader = new RequestReader(state.Game, state.Connections);
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (IngameMessageType)message.ReadByte();

            switch (type)
            {
                case IngameMessageType.Request:
                    this.handleRequest(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void handleRequest(NetIncomingMessage message)
        {
            var request = this.requestReader.FromBuffer(message);

            this.state.Game.RequestHandler.TryDo(request);
        }
    }
}

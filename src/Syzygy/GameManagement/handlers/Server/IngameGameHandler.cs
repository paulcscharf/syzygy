using System;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.GameManagement.Server
{
    sealed class IngameGameHandler : GenericIngameGameHandler<NetServer>
    {
        private readonly RequestReader requestReader;

        public IngameGameHandler(NetServer peer, GameState game, PlayerLookup players, PlayerConnectionLookup connections)
            : base(peer, game, players)
        {
            this.requestReader = new RequestReader(this.game, connections);
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

            this.game.RequestHandler.TryDo(request);
        }
    }
}

using System;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.GameManagement.Client
{
    sealed class IngameGameHandler : GenericIngameGameHandler<NetClient>
    {
        private readonly CommandReader commandReader;

        public IngameGameHandler(NetClient peer, GameState game, PlayerLookup players)
            : base(peer, game, players)
        {
            this.commandReader = new CommandReader(game);
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (IngameMessageType)message.ReadByte();

            switch (type)
            {
                case IngameMessageType.Command:
                    this.handleCommand(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void handleCommand(NetIncomingMessage message)
        {
            var command = this.commandReader.FromBuffer(message);

            command.Execute();
        }
    }
}

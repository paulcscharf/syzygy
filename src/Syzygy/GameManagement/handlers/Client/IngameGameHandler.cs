using System;
using Lidgren.Network;
using Syzygy.Game.SyncedCommands;

namespace Syzygy.GameManagement.Client
{
    sealed class IngameGameHandler : GenericIngameGameHandler<NetClient>
    {
        private readonly CommandReader commandReader;

        public IngameGameHandler(StateContainer state)
            : base(state)
        {
            this.commandReader = new CommandReader(state.Game);
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

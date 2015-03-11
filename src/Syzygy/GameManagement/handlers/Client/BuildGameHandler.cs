using Lidgren.Network;
using Syzygy.Game;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement.Client
{
    sealed class BuildGameHandler : GenericBuildGameHandler<NetClient>
    {
        private readonly PlayerLookup players;

        public BuildGameHandler(NetClient peer, PlayerLookup players, Id<Player> ownID)
            : base(peer, players, ownID)
        {
            this.players = players;
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (GenerationMessageType)message.ReadByte();
            if (type == GenerationMessageType.FinishGenerating)
            {
                var game = this.finish();

                var readyMessage = this.peer.CreateMessage();
                readyMessage.Write((byte)GameGenerationMessageType.PlayerReady);
                this.peer.SendMessage(readyMessage, NetDeliveryMethod.ReliableOrdered);

                this.stop(new ReadyGameHandler(this.peer, game, this.players));
            }
            else
            {
                var instruction = GenerationInstruction.FromBuffer(type, message);

                this.executeInstruction(instruction);
            }
        }
    }
}

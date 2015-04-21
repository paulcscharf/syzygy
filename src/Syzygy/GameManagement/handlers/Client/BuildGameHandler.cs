using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Behaviours;
using Syzygy.GameGeneration;
using Syzygy.Rendering.Game;

namespace Syzygy.GameManagement.Client
{
    sealed class BuildGameHandler : GenericBuildGameHandler<NetClient>
    {
        private readonly StateContainer.Builder stateBuilder;

        public BuildGameHandler(NetClient peer, PlayerLookup players, Id<Player> ownID)
            : base(peer, players, ownID, new ClientBehaviourProvider(peer))
        {
            this.stateBuilder = new StateContainer.Builder
            {
                Client = peer,
                Players = players
            };
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (GenerationMessageType)message.ReadByte();
            if (type == GenerationMessageType.FinishGenerating)
            {
                this.stateBuilder.Game = this.finish();
                this.stateBuilder.View = new PlayerGameView(this.stateBuilder.Game);

                var readyMessage = this.peer.CreateMessage();
                readyMessage.Write((byte)GameGenerationMessageType.PlayerReady);
                this.peer.SendMessage(readyMessage, NetDeliveryMethod.ReliableOrdered);

                this.stop(new ReadyGameHandler(this.stateBuilder.Build()));
            }
            else
            {
                var instruction = GenerationInstruction.FromBuffer(type, message);

                this.executeInstruction(instruction);
            }
        }
    }
}

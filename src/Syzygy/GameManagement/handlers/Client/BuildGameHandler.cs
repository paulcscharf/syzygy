using Lidgren.Network;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement.Client
{
    sealed class BuildGameHandler : GenericBuildGameHandler<NetClient>
    {
        public BuildGameHandler(NetClient peer)
            : base(peer)
        {
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (GenerationMessageType)message.ReadByte();
            if (type == GenerationMessageType.FinishGenerating)
            {
                var game = this.finish();
                // todo: send ready message and go into waiting loop
                this.stopMessageHandling();
            }
            else
            {
                var instruction = GenerationInstruction.FromBuffer(type, message);

                this.executeInstruction(instruction);
            }
        }
    }
}

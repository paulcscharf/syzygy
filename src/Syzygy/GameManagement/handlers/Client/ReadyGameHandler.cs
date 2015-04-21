using Bearded.Utilities;
using Lidgren.Network;

namespace Syzygy.GameManagement.Client
{
    sealed class ReadyGameHandler : GenericGameHandler<NetClient>
    {
        private readonly StateContainer state;

        public ReadyGameHandler(StateContainer state)
            : base(state.Peer)
        {
            this.state = state;
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {
            var type = (GameGenerationMessageType)message.ReadByte();
            switch (type)
            {
                case GameGenerationMessageType.StartGame:
                    this.startGame();
                    break;
                default:
                    Log.Warning("Invalid game generation message with type: " + type);
                    break;
            }
        }

        private void startGame()
        {
            this.stop(new IngameGameHandler(this.state));
        }
    }
}

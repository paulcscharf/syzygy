using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameManagement.Client
{
    sealed class ReadyGameHandler : GenericGameHandler<NetClient>
    {
        private readonly GameState game;
        private readonly PlayerLookup players;

        public ReadyGameHandler(NetClient client, GameState game, PlayerLookup players)
            : base(client)
        {
            this.game = game;
            this.players = players;
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
            this.stop(new IngameGameHandler(this.peer, this.game, this.players));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Linq;
using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameManagement.Server
{
    sealed class ReadyGameHandler : GenericGameHandler<NetServer>
    {
        private readonly GameState game;
        private readonly PlayerList players;
        private readonly HashSet<Player> readyPlayers = new HashSet<Player>(); 

        public ReadyGameHandler(NetServer server, GameState game, PlayerList players)
            : base(server)
        {
            this.game = game;
            this.players = players;
            this.readyPlayers.Add(players.First(p => p.Connection == null));
        }

        public override void Update(UpdateEventArgs e)
        {
            base.Update(e);

            this.tryStartGame();
        }

        protected override void onDataMessage(NetIncomingMessage message)
        {

            var type = (GameGenerationMessageType)message.ReadByte();
            switch (type)
            {
                case GameGenerationMessageType.PlayerReady:
                {
                    this.readyPlayers.Add(this.players[message.SenderConnection]);
                    break;
                }
                default:
                {
                    Log.Warning("Invalid game generation message with type: " + type);
                    break;
                }
            }
        }

        private void tryStartGame()
        {
            if (this.readyPlayers.Count == this.players.Count)
                this.startGame();
        }

        private void startGame()
        {
            var playerConnections = this.players.Select(p => p.Connection).NotNull().ToList();
            if (playerConnections.Count > 0)
            {
                var startMessage = this.peer.CreateMessage();
                startMessage.Write((byte)GameGenerationMessageType.StartGame);
                this.peer.SendMessage(startMessage, playerConnections,
                    NetDeliveryMethod.ReliableOrdered, 0);
            }
            this.stop(new IngameGameHandler(this.peer, this.game, this.players));
        }
    }
}

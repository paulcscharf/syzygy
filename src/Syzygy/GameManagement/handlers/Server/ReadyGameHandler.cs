using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities;
using Lidgren.Network;
using Syzygy.Game;

namespace Syzygy.GameManagement.Server
{
    sealed class ReadyGameHandler : GenericGameHandler<NetServer>
    {
        private readonly GameState game;
        private readonly PlayerLookup players;
        private readonly PlayerConnectionLookup connections;
        private readonly HashSet<Id<Player>> readyPlayers = new HashSet<Id<Player>>(); 
        
        public ReadyGameHandler(NetServer server, GameState game, PlayerLookup players, PlayerConnectionLookup connections)
            : base(server)
        {
            this.game = game;
            this.players = players;
            this.connections = connections;
            this.readyPlayers.Add(players.First().ID);
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
                    this.readyPlayers.Add(this.connections[message.SenderConnection]);
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
            if (this.connections.Count > 0)
            {
                var startMessage = this.peer.CreateMessage();
                startMessage.Write((byte)GameGenerationMessageType.StartGame);
                this.peer.SendMessage(startMessage, this.connections,
                    NetDeliveryMethod.ReliableOrdered, 0);
            }
            this.stop(new IngameGameHandler(this.peer, this.game, this.players, this.connections));
        }
    }
}

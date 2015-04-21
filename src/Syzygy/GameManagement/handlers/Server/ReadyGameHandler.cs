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
        private readonly StateContainer state;
        private readonly HashSet<Id<Player>> readyPlayers = new HashSet<Id<Player>>(); 
        
        public ReadyGameHandler(StateContainer state)
            : base(state.Peer)
        {
            this.state = state;
            this.readyPlayers.Add(state.Players.First().ID);
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
                    this.readyPlayers.Add(this.state.Connections[message.SenderConnection]);
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
            if (this.readyPlayers.Count == this.state.Players.Count)
                this.startGame();
        }

        private void startGame()
        {
            if (this.state.Connections.Count > 0)
            {
                var startMessage = this.peer.CreateMessage();
                startMessage.Write((byte)GameGenerationMessageType.StartGame);
                this.peer.SendMessage(startMessage, this.state.Connections,
                    NetDeliveryMethod.ReliableOrdered, 0);
            }
            this.stop(new IngameGameHandler(this.state));
        }
    }
}

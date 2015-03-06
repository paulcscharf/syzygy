using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.Linq;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement.Server
{
    sealed class BuildGameHandler : GenericBuildGameHandler<NetServer>
    {
        private readonly PlayerList players;
        private GameState game;

        public BuildGameHandler(NetServer server, PlayerList players)
            : base(server, players)
        {
            this.players = players;
            var generator = new SimpleGenerator();

            var instructions = generator.Generate(players.Select(p => p.ID).ToList());

            var playerConnections = players.Select(p => p.Connection).NotNull().ToList();

            // instructions may want to be spaced out in time in the future,
            // to prevent packet loss, and thus longer building time
            foreach (var instruction in instructions)
            {
                // build own game
                this.executeInstruction(instruction);

                if (playerConnections.Count > 0)
                {
                    // instruct clients how to build game
                    var message = server.CreateMessage();
                    instruction.WriteMessage(message);
                    server.SendMessage(message, playerConnections, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }

            // send finish message and finish
            this.game = this.finish();

            if (playerConnections.Count > 0)
            {
                var finishMessage = server.CreateMessage();
                finishMessage.Write((byte)GenerationMessageType.FinishGenerating);
                server.SendMessage(finishMessage, playerConnections, NetDeliveryMethod.ReliableOrdered, 0);
            }
        }

        public override void Update(UpdateEventArgs e)
        {
            this.stop(new ReadyGameHandler(this.peer, game, players));
        }
    }
}

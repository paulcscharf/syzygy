using System.Collections.Generic;
using System.Linq;
using amulware.Graphics;
using Bearded.Utilities.Linq;
using Lidgren.Network;
using Syzygy.GameGeneration;

namespace Syzygy.GameManagement.Server
{
    sealed class BuildGameHandler : GenericBuildGameHandler<NetServer>
    {
        public BuildGameHandler(NetServer server, IList<Player> players)
            : base(server)
        {
            var generator = new SimpleGenerator();

            var instructions = generator.Generate(players.Select(p => p.ID).ToList());

            var playerConnections = players.Select(p => p.Connection).NotNull().ToList();

            // instructions may want to be spaced out in time in the future, to prevent packet loss
            foreach (var instruction in instructions)
            {
                // build own game
                this.executeInstruction(instruction);

                // instruct clients how to build game
                var message = server.CreateMessage();
                instruction.WriteMessage(message);
                server.SendMessage(message, playerConnections, NetDeliveryMethod.ReliableOrdered, 0);
            }

            this.finish();
        }

        public override void Update(UpdateEventArgs e)
        {
            this.stop(new ReadyGameHandler(this.peer));
        }
    }
}

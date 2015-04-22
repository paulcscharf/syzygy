using System.Runtime.InteropServices;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Astronomy;
using Syzygy.Game.Economy;

namespace Syzygy.GameGeneration
{
    sealed class AssignPlayerToBodyInstruction
        : GenerationInstruction<AssignPlayerToBodyInstruction.Parameters>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Parameters
        {
            public readonly Id<Player> Player; 
            public readonly Id<IBody> Body;

            public Parameters(Id<Player> player, Id<IBody> body)
            {
                this.Player = player;
                this.Body = body;
            }
        }

        public AssignPlayerToBodyInstruction(NetBuffer buffer)
            : base(GenerationMessageType.AssignPlayerToBody, buffer) { }

        public AssignPlayerToBodyInstruction(Parameters parameters)
            : base(GenerationMessageType.AssignPlayerToBody, parameters) { }

        public AssignPlayerToBodyInstruction(Id<Player> player, Id<IBody> body)
            : this(new Parameters(player, body))
        {
        }

        public override void Execute(GameState game)
        {
            new Economy(game, this.parameters.Player, this.parameters.Body);
        }
    }
}

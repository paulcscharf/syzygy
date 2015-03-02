using System.Runtime.InteropServices;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Astronomy;
using Syzygy.GameManagement;

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

        public override void Execute(GameState game)
        {
            throw new System.NotImplementedException();
        }
    }
}

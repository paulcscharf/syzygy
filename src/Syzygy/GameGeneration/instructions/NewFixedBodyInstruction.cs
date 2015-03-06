using System.Runtime.InteropServices;
using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Astronomy;

namespace Syzygy.GameGeneration
{
    class NewFixedBodyInstruction
        : GenerationInstruction<NewFixedBodyInstruction.Parameters>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Parameters
        {
            public readonly Id<IBody> Id; 
            public readonly Position2 Position;
            public readonly Radius Radius;
            public readonly float Mass;

            public Parameters(Id<IBody> id, Position2 position, Radius radius, float mass)
            {
                this.Id = id;
                this.Position = position;
                this.Radius = radius;
                this.Mass = mass;
            }
        }

        public NewFixedBodyInstruction(NetBuffer buffer)
            : base(GenerationMessageType.NewFixedBody, buffer) { }

        public NewFixedBodyInstruction(Parameters parameters)
            : base(GenerationMessageType.NewFixedBody, parameters) { }

        public NewFixedBodyInstruction(Id<IBody> id, Position2 position, Radius radius, float mass)
            : this(new Parameters(id, position, radius, mass)) { }

        public override void Execute(GameState game)
        {
            new FixedBody(game,
                this.parameters.Id,
                this.parameters.Position,
                this.parameters.Radius,
                this.parameters.Mass,
                Color.Yellow
                );
        }
    }
}

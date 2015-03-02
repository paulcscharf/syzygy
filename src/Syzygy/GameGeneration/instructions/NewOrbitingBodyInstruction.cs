using System.Runtime.InteropServices;
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Lidgren.Network;
using Syzygy.Game;
using Syzygy.Game.Astronomy;

namespace Syzygy.GameGeneration
{
    class NewOrbitingBodyInstruction
        : GenerationInstruction<NewOrbitingBodyInstruction.Parameters>
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Parameters
        {
            public readonly Id<IBody> Id;
            public readonly Id<IBody> ParentId; 
            public readonly Unit OrbitRadius;
            public readonly Direction2 OrbitDirection;
            public readonly Unit Radius;
            public readonly float Mass;

            public Parameters(Id<IBody> id, Id<IBody> parentId, Unit orbitRadius, Direction2 orbitDirection, Unit radius, float mass)
            {
                this.Id = id;
                this.ParentId = parentId;
                this.OrbitRadius = orbitRadius;
                this.OrbitDirection = orbitDirection;
                this.Radius = radius;
                this.Mass = mass;
            }
        }

        public NewOrbitingBodyInstruction(NetBuffer buffer)
            : base(GenerationMessageType.NewOrbitingBody, buffer) { }

        public NewOrbitingBodyInstruction(Parameters parameters)
            : base(GenerationMessageType.NewOrbitingBody, parameters) { }

        public NewOrbitingBodyInstruction(Id<IBody> id, Id<IBody> parentId,
            Unit orbitRadius, Direction2 orbitDirection, Unit radius, float mass)
            : this(new Parameters(id, parentId, orbitRadius, orbitDirection, radius, mass)) { }

        public override void Execute(GameState game)
        {
            new OrbitingBody(game,
                this.parameters.Id,
                this.parameters.ParentId,
                this.parameters.OrbitRadius,
                this.parameters.OrbitDirection,
                this.parameters.Radius,
                this.parameters.Mass,
                Color.Gray
                );
        }
    }
}

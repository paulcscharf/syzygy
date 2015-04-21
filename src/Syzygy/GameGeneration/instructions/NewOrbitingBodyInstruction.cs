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
            public readonly Radius OrbitRadius;
            public readonly Direction2 OrbitDirection;
            public readonly Radius Radius;
            public readonly float Mass;
            public readonly float Health;

            public Parameters(Id<IBody> id, Id<IBody> parentId, Radius orbitRadius, Direction2 orbitDirection,
                Radius radius, float mass, float health)
            {
                this.Id = id;
                this.ParentId = parentId;
                this.OrbitRadius = orbitRadius;
                this.OrbitDirection = orbitDirection;
                this.Radius = radius;
                this.Mass = mass;
                this.Health = health;
            }
        }

        public NewOrbitingBodyInstruction(NetBuffer buffer)
            : base(GenerationMessageType.NewOrbitingBody, buffer) { }

        public NewOrbitingBodyInstruction(Parameters parameters)
            : base(GenerationMessageType.NewOrbitingBody, parameters) { }

        public NewOrbitingBodyInstruction(Id<IBody> id, Id<IBody> parentId,
            Radius orbitRadius, Direction2 orbitDirection, Radius radius, float mass, float health)
            : this(new Parameters(id, parentId, orbitRadius, orbitDirection, radius, mass, health)) { }

        public override void Execute(GameState game)
        {
            new OrbitingBody(game,
                this.parameters.Id,
                this.parameters.ParentId,
                this.parameters.OrbitRadius,
                this.parameters.OrbitDirection,
                this.parameters.Radius,
                this.parameters.Mass,
                Color.Gray,
                this.parameters.Health
                );
        }
    }
}

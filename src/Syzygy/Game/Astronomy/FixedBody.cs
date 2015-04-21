using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Syzygy.Rendering;

namespace Syzygy.Game.Astronomy
{
    sealed class FixedBody : GameObject<IBody>, IBody
    {
        private readonly Position2 center;
        private readonly Radius radius;
        private readonly float mass;
        private readonly Color color;

        public Circle Shape { get { return new Circle(this.center, this.radius); } }
        public float Mass { get { return this.mass; } }
        public Velocity2 Velocity { get { return new Velocity2(); } }

        public FixedBody(GameState game, Id<IBody> id, Position2 center, Radius radius, float mass, Color color)
            : base(game, id)
        {
            this.center = center;
            this.radius = radius;
            this.mass = mass;
            this.color = color;
        }

        public override void Update(TimeSpan t)
        {
            
        }

        public override void Draw(GeometryManager geos)
        {
            geos.Primitives.Color = this.color;
            geos.Primitives.DrawCircle(this.center.Vector, this.radius.NumericValue);
        }
    }
}

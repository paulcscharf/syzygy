using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using Syzygy.Rendering;

namespace Syzygy.Astronomy
{
    sealed class FixedBody : GameObject, IBody
    {
        private readonly Position2 center;
        private readonly Unit radius;
        private readonly float mass;
        private readonly Color color;

        public Circle Shape { get { return new Circle(this.center, this.radius); } }
        public float Mass { get { return this.mass; } }

        public FixedBody(GameState game, Position2 center, Unit radius, float mass, Color color)
            : base(game)
        {
            this.center = center;
            this.radius = radius;
            this.mass = mass;
            this.color = color;

            game.Bodies.Add(this);
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

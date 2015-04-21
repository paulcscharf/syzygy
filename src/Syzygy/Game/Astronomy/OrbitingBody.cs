using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game.Astronomy
{
    sealed class OrbitingBody : GameObject<IBody>, IBody
    {
        private readonly IBody parent;
        private readonly Radius orbitRadius;
        private Direction2 orbitDirection;
        private readonly Radius radius;
        private readonly float mass;
        private readonly Color color;

        private readonly Angle angularVelocity;

        private Position2 center;

        public OrbitingBody(GameState game, Id<IBody> id, IBody parent,
            Radius orbitRadius, Direction2 orbitDirection, Radius radius, float mass, Color color)
            : base(game, id)
        {
            this.parent = parent;
            this.orbitRadius = orbitRadius;
            this.orbitDirection = orbitDirection;
            this.radius = radius;
            this.mass = mass;
            this.color = color;

            this.center = this.calculatePosition();

            this.angularVelocity =
                ((Constants.G * parent.Mass / orbitRadius.NumericValue).Sqrted() / orbitRadius.NumericValue).Radians();
        }

        public OrbitingBody(GameState game, Id<IBody> id, Id<IBody> parentId,
            Radius orbitRadius, Direction2 orbitDirection, Radius radius, float mass, Color color)
            : this(game, id, game.Bodies[parentId], orbitRadius, orbitDirection, radius, mass, color)
        {
        }

        private Position2 calculatePosition()
        {
            return new Position2(this.parent.Shape.Center.Vector + this.orbitDirection.Vector * this.orbitRadius.NumericValue);
        }

        public Circle Shape { get { return new Circle(this.center, this.radius); } }
        public float Mass { get { return this.mass; } }

        public Velocity2 Velocity
        {
            get
            {
                return new Velocity2(this.orbitDirection.Vector.PerpendicularLeft
                    * (this.angularVelocity.Radians * this.orbitRadius.NumericValue));
            }
        }

        public override void Update(TimeSpan t)
        {
            var step = this.angularVelocity * (float)t.NumericValue;
            this.orbitDirection += step;
            this.center = this.calculatePosition();
        }

        public override void Draw(GeometryManager geos)
        {
            geos.Primitives.Color = this.color;
            geos.Primitives.DrawCircle(this.center.Vector, this.radius.NumericValue);
        }

    }
}

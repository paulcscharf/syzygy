using System;
using amulware.Graphics;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game.Astronomy
{
    sealed class OrbitingBody : GameObject, IBody
    {
        private readonly IBody parent;
        private readonly Unit orbitRadius;
        private Direction2 orbitDirection;
        private readonly Unit radius;
        private readonly float mass;
        private readonly Color color;

        private readonly Angle angularVelocity;

        private Position2 center;

        public OrbitingBody(GameState game, IBody parent, Unit orbitRadius, Direction2 orbitDirection,
            Unit radius, float mass, Color color)
            : base(game)
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

            game.Bodies.Add(this);
        }

        private Position2 calculatePosition()
        {
            return new Position2(this.parent.Shape.Center.Vector + this.orbitDirection.Vector * this.orbitRadius.NumericValue);
        }

        public Circle Shape { get { return new Circle(this.center, this.radius); } }
        public float Mass { get { return this.mass; } }

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

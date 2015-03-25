using amulware.Graphics;
using Bearded.Utilities.SpaceTime;
using OpenTK;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game.Astronomy
{
    class FreeObject : GameObject<FreeObject>
    {
        private Position2 position;
        private Velocity2 velocity;

        public Position2 Position { get { return this.position; } }
        public Velocity2 Velocity { get { return this.velocity; } }

        public FreeObject(GameState game, Id<FreeObject> id, Position2 position, Velocity2 velocity)
            : base(game, id)
        {
            this.position = position;
            this.velocity = velocity;

            game.ContinuousSynchronizer.Sync(this);
        }

        public override void Update(TimeSpan t)
        {
            this.game.CollisionHandler.HandleCollision(this);

            if (this.Deleted)
                return;

            var acceleration = Vector2.Zero;

            foreach (var body in this.game.Bodies)
            {
                var shape = body.Shape;

                var difference = shape.Center - this.position;

                var distanceSquared = difference.LengthSquared;

                var a = Constants.G * body.Mass / distanceSquared.NumericValue;

                var dirNormal = difference.Direction.Vector;

                acceleration += dirNormal * a;
            }

            this.velocity += new Velocity2(acceleration * (float)t.NumericValue);

            this.position += this.velocity * t;
        }

        public void HitBody(IBody body)
        {
            this.hitBody(body);
        }

        protected virtual void hitBody(IBody body)
        {
            this.delete();
        }

        public override void Draw(GeometryManager geos)
        {
            geos.Primitives.Color = Color.White;
            geos.Primitives.DrawRectangle(this.position.Vector, new Vector2(0.05f, 0.05f));
        }

        public void UpdatePositionAndVelocity(Position2 position, Velocity2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }
    }
}

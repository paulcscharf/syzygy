using System;
using System.Collections;
using amulware.Graphics;
using Bearded.Utilities;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;
using OpenTK;
using Syzygy.Astronomy;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy
{
    sealed class GameState
    {
        private Instant time = Instant.Zero;

        // more info on this collection at http://genericgamedev.com/general/on-collections
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();

        private readonly DeletableObjectList<IBody> bodies = new DeletableObjectList<IBody>();
        public DeletableObjectList<IBody> Bodies { get { return this.bodies; } }

        public GameState()
        {
            var sun = new FixedBody(this, new Position2(0f.Units(), 0f.Units()), 1f.Units(), 1, Color.Yellow);

            var p0 = new OrbitingBody(this, sun, 5f.Units(), Direction2.Zero, 0.5f.Units(), 0.25f, Color.Blue);
            var p1 = new OrbitingBody(this, sun, 9f.Units(), Direction2.Zero, 0.5f.Units(), 0.25f, Color.Green);

            var m0 = new OrbitingBody(this, p0, 1f.Units(), Direction2.Zero, 0.1f.Units(), 0.01f, Color.Gray);

            new FreeObject(this, new Position2(5f.Units(), 5f.Units()), new Velocity2(0f.Units(), 0f.Units()));

            for (int i = 0; i < 1000; i++)
            {
                new FreeObject(this,
                    new Position2(StaticRandom.Float(-10, 10).Units(), StaticRandom.Float(-10, 10).Units()),
                    new Velocity2(Direction2.FromRadians(StaticRandom.Float(MathHelper.TwoPi)).Vector * StaticRandom.Float(0)));
            }
        }

        public void Update(UpdateEventArgs e)
        {
            var elapsed = e.ElapsedTimeInS.Seconds();

            this.update(elapsed);
        }

        private void update(TimeSpan t)
        {
            this.time += t;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(t);
            }
        }

        public void Draw()
        {
            var geos = GeometryManager.Instance;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw(geos);
            }
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }
    }
}

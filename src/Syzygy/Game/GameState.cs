using System.Collections.Generic;
using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.SpaceTime;
using Syzygy.Game.Astronomy;
using Syzygy.Rendering;
using TimeSpan = Bearded.Utilities.SpaceTime.TimeSpan;

namespace Syzygy.Game
{
    sealed class GameState
    {
        private Instant time = Instant.Zero;

        // more info on this collection at http://genericgamedev.com/general/on-collections
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();

        private readonly DeletableObjectList<IBody> bodies = new DeletableObjectList<IBody>();
        public DeletableObjectList<IBody> Bodies { get { return this.bodies; } }

        private readonly Dictionary<Id<IBody>, IBody> bodyDictionary = new Dictionary<Id<IBody>, IBody>();
        public Dictionary<Id<IBody>, IBody> BodyDictionary { get { return this.bodyDictionary; } }

        public GameState()
        {
        }

        public void Update(UpdateEventArgs e)
        {
            var elapsed = new TimeSpan(e.ElapsedTimeInS);

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
